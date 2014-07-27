using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Sun.Toolkit
{
    public static class context
    {
        /// <summary>
        /// //返回 当前的HttpContext.Current对象 
        /// </summary>
        public static HttpContext getCurrentHttpContext()
        {
            return HttpContext.Current;
        }

        /// <summary>
        /// //当前的 QueryString 键值集合
        /// </summary>
        public static NameValueCollection GetQueryString()
        {
            if (getCurrentHttpContext() != null)
            {
                return getCurrentHttpContext().Request.QueryString;
            }
            return null;
        }

        /// <summary>
        /// // 并将 全部 转成 小写
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetQueryDict()
        {
            Dictionary<string, string> dict = null;
            NameValueCollection nvc = GetQueryString();

            if (nvc != null)
            {
                dict = new Dictionary<string, string>(GetQueryString().Count);

                foreach (string item in nvc)
                {
                    string key = item.ToString().Trim().ToLower();
                    string value = nvc[item];
                    dict.Add(key, value);
                }
            }
            return dict;
        }

        public static int? getValueToInt(string key)
        {
            var val = Sun.Toolkit.Parse.ToInt(GetQueryString()[key], -1);

            if (val == -1)
            {
                return null;
            }
            return val;
        }

        /// <summary>
        /// // 返回程序的绝对路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string getMapPath(string path)
        {

            try
            {
                if ((path.IndexOf(@":\") > 0) || (path.IndexOf("://") > 0))
                {
                    return path;
                }
                if (getCurrentHttpContext() != null)
                {
                    return getCurrentHttpContext().Server.MapPath(path);
                }
                if (path.StartsWith("~/"))
                {
                    path = path.Substring(2);
                }
                else if (path.StartsWith("/"))
                {
                    path = path.Substring(1);
                }
                return (AppDomain.CurrentDomain.BaseDirectory + path.Replace("/", @"\"));
            }
            catch
            {
                return path;
            }
        }

        /// <summary>
        /// //返回用户当前的IP地址
        /// </summary>
        public static string getUserIP()
        {
            string userIP = "000.000.000.000";
            try
            {
                userIP = getUserIP(null);
            }
            catch
            {
            }
            return userIP;
        }

        /// <summary>
        /// //返回指定了环境的用户当前的IP地址
        /// </summary>
        public static string getUserIP(HttpContext context)
        {
            string str = string.Empty;
            if (context == null)
            {
                context = getCurrentHttpContext();
            }
            str = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            switch (str)
            {
                case null:
                case "":
                    str = context.Request.ServerVariables["REMOTE_ADDR"];
                    break;
            }
            if ((str != null) && !(str == string.Empty))
            {
                return str;
            }
            return context.Request.UserHostAddress;
        }
    }
}
