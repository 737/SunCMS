using System;
using System.Web;

namespace Sun.Util
{
    /// <summary>
    /// //SunCMS Cookies 管理中心
    /// </summary>
    public class Cookie
    {
        public static string GetCookie(string sKey)
        {
            if (string.IsNullOrEmpty(sKey))
            {
                return null;
            }
            if ((Context.CurrentHttpContext.Request.Cookies != null) && (Context.CurrentHttpContext.Request.Cookies[sKey]) != null)
            {
                return (Context.CurrentHttpContext.Request.Cookies[sKey].Value);
            }
            return null;
        }

        public static void WriteCookie(string sKey, string sValue)
        {
            HttpCookie cookie = Context.CurrentHttpContext.Request.Cookies[sKey];
            if (cookie == null)
            {
                cookie = new HttpCookie(sKey);
            }
            cookie.Value = HttpUtility.UrlEncode(sValue);
            Context.CurrentHttpContext.Response.AppendCookie(cookie);
        }

        public static void WriteCookie(string sKey, string sValue, double dExpiresSecond)
        {
            HttpCookie cookie = Context.CurrentHttpContext.Request.Cookies[sKey];
            if (cookie == null)
            {
                cookie = new HttpCookie(sKey);
            }
            cookie.Value = HttpUtility.UrlEncode(sValue);
            cookie.Expires = DateTime.Now.AddSeconds((double)dExpiresSecond);
            Context.CurrentHttpContext.Response.AppendCookie(cookie);
        }
    }
}
