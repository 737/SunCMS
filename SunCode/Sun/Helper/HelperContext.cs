using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Sun
{
    public class HelperContext
    {
        /// <summary>
        /// //返回 当前的HttpContext.Current对象 
        /// </summary>
        public static HttpContext GetCurrentHttpContext()
        {
            return HttpContext.Current;
        }

        /// <summary>
        /// //当前的 QueryString 键值集合
        /// </summary>
        public static NameValueCollection GetQueryString()
        {
            if (GetCurrentHttpContext() != null)
            {
                return GetCurrentHttpContext().Request.QueryString;
            }
            return null;
        }
        public static string GetQueryString(string key)
        {
            if ((GetCurrentHttpContext() != null) && (!string.IsNullOrEmpty(key)))
            {
                return GetCurrentHttpContext().Request.QueryString[key.Trim()];
            }
            return null;
        }

        public static NameValueCollection GetFrom()
        {
            if (GetCurrentHttpContext() != null)
            {
                return GetCurrentHttpContext().Request.Form;
            }
            return null;
        }

        /// <summary>
        /// // 读取 querystring  和 from 中的全部 数据
        /// // 并将 全部 转成 小写
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetQueryDict()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            NameValueCollection nvc = GetQueryString();
            NameValueCollection nvcForm = GetFrom();

            if (nvc.Count > 0)
            {
                dict = new Dictionary<string, string>(nvc.Count);

                foreach (string item in nvc)
                {
                    string key = item.ToString().Trim().ToLower();
                    string value = nvc[item];
                    dict.Add(key, value);
                }
            }
            if (nvcForm.Count > 0)
            {
                dict = new Dictionary<string, string>(nvcForm.Count);

                foreach (string item in nvcForm)
                {
                    string key = item.ToString().Trim().ToLower();
                    string value = nvcForm[item];
                    dict.Add(key, value);
                }
            }
            return dict;
        }

        public static T GetFillEntity<T>() where T : class,new()
        {
            Dictionary<string, string> queryDict = GetQueryDict();
            T entity = new T();
            Type type = entity.GetType();
            Regex re = new Regex(@"System.\w*", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            string propName, propNameWithType, typeName = "", propType;
            MatchCollection propTypeCollect;

            foreach (PropertyInfo info in type.GetProperties())
            {
                object propValue = null;

                if (info.CanWrite)
                {
                    //以类名 包含的字符窜
                    if (type.Name.StartsWith("entity", StringComparison.OrdinalIgnoreCase))
                    {
                        //去类 类名 前的 entity
                        typeName = type.Name.ToLower().Replace("entity", "");
                    }

                    propName = info.Name.ToLower(); //转成小写 保持一致   e.g.: id
                    propNameWithType = string.Format("{0}.{1}", typeName, info.Name.ToLower());  //

                    if (queryDict.ContainsKey(propNameWithType))
                    {
                        propValue = queryDict[propNameWithType];
                    }
                    else if (queryDict.ContainsKey(propName))
                    {
                        propValue = queryDict[propName];
                    }

                    if (propValue != null)
                    {
                        propType = info.PropertyType.FullName;
                        propTypeCollect = re.Matches(propType);
                        propType = propTypeCollect.Count > 1 ? propTypeCollect[1].ToString() : propTypeCollect[0].ToString();

                        switch (propType)
                        {
                            case "System.Int":
                            case "System.Int16":
                            case "System.Int32":
                                propValue = Sun.Toolkit.Parse.ToInt(propValue);
                                break;
                            case "System.DateTime":
                                propValue = Sun.Toolkit.Parse.ToDateTime(propValue);
                                break;
                            case "System.Boolean":
                                propValue = Sun.Toolkit.Parse.ToBoolean(propValue);
                                break;
                        }

                        try
                        {
                            info.SetValue(entity, propValue, null);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }

            return entity;
        }
    }
}
