using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Sun.Util
{
    public static class Info
    {
        /// <summary>
        /// //返回指定了环境的用户当前的IP地址
        /// </summary>
        public static string GetUserIP(HttpContext context)
        {
            string str = string.Empty;
            if (context == null)
            {
                return str;
            }
            str = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            switch (str)
            {
                case null:
                case "":
                    str = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    break;
            }
            if ((str != null) && !(str == string.Empty))
            {
                return str;
            }
            return HttpContext.Current.Request.UserHostAddress;
        }

        /// <summary>
        /// //返回用户当前的IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetUserIP()
        {
            string userIP = "000.000.000.000";
            try
            {
                userIP = GetUserIP(Context.CurrentHttpContext);
            }
            catch
            {
            }
            return userIP;
        }
    }
}
