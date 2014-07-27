using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Sun.Util
{
    public static class Context
    {
        public static int GetIntFromQueryString(string key, int _default)
        {
            string str = QueryString[key];
            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    if (str.IndexOf('#') > 0)
                    {
                        str = str.Substring(0, str.IndexOf('#'));
                    }
                    _default = Convert.ToInt16(str);
                }
                catch
                {
                }
            }
            return _default;
        }

        
        /// <summary>
        /// //返回 当前的HttpContext.Current对象 
        /// </summary>
        public static HttpContext CurrentHttpContext
        {
            get
            {
                return HttpContext.Current;
            }
        }

        /// <summary>
        /// //当前的 QueryString 键值集合
        /// </summary>
        public static NameValueCollection QueryString
        {
            get
            {
                if (CurrentHttpContext != null)
                {
                    return CurrentHttpContext.Request.QueryString;
                }
                return null;
            }
        }

        public static bool IsWebRequest
        {
            get
            {
                return (CurrentHttpContext != null);
            }
        }
    }
}
