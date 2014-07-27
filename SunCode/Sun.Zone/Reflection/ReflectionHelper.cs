using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Web.Compilation;
using System.Web;
using OptimizeReflection;

namespace Sun.Zone
{
    // 说明：
    // 1. 对于处理页面请求的Controller类型，这里不缓存，只缓存所包含的Action
    //    用于页面请求的Action的描述中，已包含Controller的描述信息。
    // 2. 对于处理Ajax请求的Controller以及Action，在初始化时，只查找出Controller，Action采用延迟加载方式

    // 原因：
    // 1. 页面请求是在Action中指定的，因此，只能先找到所有能处理页面请求的Action
    // 2. Ajax调用时，可以从URL中解析出要调用的类名与方法名，因此可以在调用时再去查找。


    internal static class ReflectionHelper
    {
        public static readonly Type VoidType = typeof(void);

        // 保存PageAction的字典
        private static Dictionary<string, ActionDescription> s_PageActionDict;

        // 保存AjaxController的列表
        private static List<ControllerDescription> _ajaxControllerList;
        // 保存AjaxAction的字典
        private static Hashtable s_AjaxActionTable = Hashtable.Synchronized(
                                                new Hashtable(4096, StringComparer.OrdinalIgnoreCase));

        // 用于从类型查找Action的反射标记
        private static readonly BindingFlags ActionBindingFlags =
            BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;

        static ReflectionHelper()
        {
            InitControllers();
        }

        /// <summary>
        /// // 加载所有的Controller
        /// </summary>
        private static void InitControllers()
        {
            _ajaxControllerList = new List<ControllerDescription>(1024);
            var pageControllerList = new List<ControllerDescription>(1024);

            ICollection assemblies = BuildManager.GetReferencedAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                // 过滤以【System.】开头的程序集，加快速度
                if (assembly.FullName.StartsWith("System.", StringComparison.OrdinalIgnoreCase))
                    continue;

                try
                {
                    foreach (Type t in assembly.GetExportedTypes())
                    {
                        if (t.Name.StartsWith("Api", StringComparison.OrdinalIgnoreCase))
                            _ajaxControllerList.Add(new ControllerDescription(t));

                        else if (t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
                            pageControllerList.Add(new ControllerDescription(t));
                    }
                }
                catch { }
            }


            // 提前加载Page Controller中的所有Action方法
            s_PageActionDict = new Dictionary<string, ActionDescription>(4096, StringComparer.OrdinalIgnoreCase);

            foreach (ControllerDescription controller in pageControllerList)
            {
                foreach (MethodInfo m in controller.ControllerType.GetMethods(ActionBindingFlags))
                {
                    PageUrlAttribute[] pageUrlAttrs = m.GetMyAttributes<PageUrlAttribute>();
                    ActionAttribute actionAttr = m.GetMyAttribute<ActionAttribute>();

                    if (pageUrlAttrs.Length > 0 && actionAttr != null)
                    {
                        ActionDescription actionDescription =
                            new ActionDescription(m, actionAttr) { PageController = controller };

                        foreach (PageUrlAttribute attr in pageUrlAttrs)
                        {
                            if (string.IsNullOrEmpty(attr.Url) == false)
                                s_PageActionDict.Add(attr.Url, actionDescription);
                        }
                    }
                }
            }

            // 用于Ajax调用的Action信息则采用延迟加载的方式。
        }

        internal static T GetMyAttribute<T>(this MemberInfo m, bool inherit) where T : Attribute
        {
            T[] array = m.GetCustomAttributes(typeof(T), inherit) as T[];

            if (array.Length == 1)
                return array[0];

            if (array.Length > 1)
                throw new InvalidProgramException(string.Format("方法 {0} 不能同时指定多次 [{1}]。", m.Name, typeof(T)));

            return default(T);
        }

        internal static T GetMyAttribute<T>(this MemberInfo m) where T : Attribute
        {
            return GetMyAttribute<T>(m, false);
        }


        internal static T[] GetMyAttributes<T>(this MemberInfo m, bool inherit) where T : Attribute
        {
            return m.GetCustomAttributes(typeof(T), inherit) as T[];
        }

        internal static T[] GetMyAttributes<T>(this MemberInfo m) where T : Attribute
        {
            return m.GetCustomAttributes(typeof(T), false) as T[];
        }


        /// <summary>
        /// 根据要调用的controller名返回对应的Controller （适用于Ajax调用）
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        private static ControllerDescription GetAjaxController(string controller)
        {
            if (string.IsNullOrEmpty(controller))
                throw new ArgumentNullException("controller");


            // 查找类型的方式：如果有点号，则按全名来查找(包含命名空间)，否则只看名字。
            // 本框架对于多个匹配条件的类型，将返回第一个匹配项。
            if (controller.IndexOf('.') > 0)
                return _ajaxControllerList.FirstOrDefault(
                    t => string.Compare(t.ControllerType.FullName, controller, true) == 0);
            else
                return _ajaxControllerList.FirstOrDefault(
                    t => string.Compare(t.ControllerType.Name, controller, true) == 0);
        }

        /// <summary>
        /// 根据要调用的方法名返回对应的 Action （适用于Ajax调用）
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private static ActionDescription GetAjaxAction(Type controller, string action, HttpRequest request)
        {
            if (controller == null)
                throw new ArgumentNullException("controller");
            if (string.IsNullOrEmpty(action))
                throw new ArgumentNullException("action");

            // 首先尝试从缓存中读取
            string key = request.HttpMethod + "#" + controller.FullName + "@" + action;
            ActionDescription mi = (ActionDescription)s_AjaxActionTable[key];

            if (mi == null)
            {
                bool saveToCache = true;

                MethodInfo method = FindAction(action, controller, request);

                if (method == null)
                {
                    // 如果Action的名字是submit并且是POST提交，则需要自动寻找Action
                    // 例如：多个提交都采用一样的方式：POST /AjaxProduct/submit
                    if (action.IsSame("submit") && request.HttpMethod.IsSame("POST"))
                    {
                        // 自动寻找Action
                        method = FindSubmitAction(controller, request);
                        saveToCache = false;
                    }
                }

                if (method == null)
                    return null;

                var attr = method.GetMyAttribute<ActionAttribute>();
                mi = new ActionDescription(method, attr);

                if (saveToCache)
                    s_AjaxActionTable[key] = mi;
            }

            return mi;
        }


        private static MethodInfo FindAction(string action, Type controller, HttpRequest request)
        {
            foreach (MethodInfo method in controller.GetMethods())
            {
                if (method.Name.IsSame(action))
                {
                    if (MethodActionIsMatch(method, request))
                        return method;
                }
            }
            return null;
        }

        private static MethodInfo FindSubmitAction(Type controller, HttpRequest request)
        {
            string[] keys = request.Form.AllKeys;

            foreach (MethodInfo method in controller.GetMethods())
            {
                string key = keys.FirstOrDefault(x => method.Name.IsSame(x));
                if (key != null && MethodActionIsMatch(method, request))
                    return method;
            }

            return null;
        }

        private static bool MethodActionIsMatch(MethodInfo method, HttpRequest request)
        {
            var attr = method.GetMyAttribute<ActionAttribute>();
            if (attr != null)
            {
                if (attr.AllowExecute(request.HttpMethod))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// // 根据一个AJAX的调用信息（类名与方法名），返回内部表示的调用信息。
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        public static InvokeInfo GetAjaxInvokeInfo(ControllerActionPair pair, HttpRequest request)
        {
            if (pair == null)
                throw new ArgumentNullException("pair");

            InvokeInfo vkInfo = new InvokeInfo();

            vkInfo.Controller = GetAjaxController(pair.Controller);
            if (vkInfo.Controller == null)
                return null;

            vkInfo.Action = GetAjaxAction(vkInfo.Controller.ControllerType, pair.Action, request);
            if (vkInfo.Action == null)
                return null;

            if (vkInfo.Action.MethodInfo.IsStatic == false)
                //vkInfo.Instance = Activator.CreateInstance(vkInfo.Controller.ControllerType);
                vkInfo.Instance = vkInfo.Controller.ControllerType.FastNew();

            return vkInfo;
        }

        /// <summary>
        /// 根据一个页面请求路径，返回内部表示的调用信息。
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static InvokeInfo GetPageActionInvokeInfo(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");

            ActionDescription action = null;
            if (s_PageActionDict.TryGetValue(url, out action) == false)
                return null;

            InvokeInfo vkInfo = new InvokeInfo();
            vkInfo.Controller = action.PageController;
            vkInfo.Action = action;

            if (vkInfo.Action.MethodInfo.IsStatic == false)
                //vkInfo.Instance = Activator.CreateInstance(vkInfo.Controller.ControllerType);
                vkInfo.Instance = vkInfo.Controller.ControllerType.FastNew();

            return vkInfo;
        }


        private static Hashtable s_modelTable = Hashtable.Synchronized(
                                            new Hashtable(4096, StringComparer.OrdinalIgnoreCase));

        /// <summary>
        /// 返回一个实体类型的描述信息（全部属性及字段）。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ModelDescripton GetModelDescripton(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            string key = type.FullName;
            ModelDescripton mm = (ModelDescripton)s_modelTable[key];

            if (mm == null)
            {
                List<DataMember> list = new List<DataMember>();

                (from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                 select new PropertyMember(p)).ToList().ForEach(x => list.Add(x));

                (from f in type.GetFields(BindingFlags.Instance | BindingFlags.Public)
                 select new FieldMember(f)).ToList().ForEach(x => list.Add(x));

                mm = new ModelDescripton { Fields = list.ToArray() };
                s_modelTable[key] = mm;
            }
            return mm;
        }

        /// <summary>
        /// 返回一个实体类型的指定名称的数据成员
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DataMember GetMemberByName(Type type, string name, bool ifNotFoundThrowException)
        {
            ModelDescripton description = GetModelDescripton(type);
            foreach (DataMember member in description.Fields)
                if (member.Name == name)
                    return member;

            if (ifNotFoundThrowException)
                throw new ArgumentOutOfRangeException(
                        string.Format("指定的成员 {0} 在类型 {1} 中并不存在。", name, type.ToString()));

            return null;
        }


        /// <summary>
        /// 判断是否是一个可支持的参数类型。仅包括：基元类型，string ，decimal，DateTime，Guid, string[], 枚举
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSupportableType(this Type type)
        {
            return type.IsPrimitive
                || type == typeof(string)
                || type == typeof(DateTime)
                || type == typeof(decimal)
                || type == typeof(Guid)
                || type.IsEnum
                || type == typeof(string[]);
        }

        /// <summary>
        /// // 得到一个实际的类型（排除Nullable类型的影响）。比如：int? 最后将得到int
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetRealType(this Type type)
        {
            if (type.IsGenericType)
                return Nullable.GetUnderlyingType(type) ?? type;
            else
                return type;
        }

    }
}
