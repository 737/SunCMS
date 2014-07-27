using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Sun.Util
{
    public static class Path
    {
        /// <summary>
        /// //返回程序的绝对路径
        /// </summary>
        public static string GetMapPath(string path)
        {
            try
            {
                if ((path.IndexOf(@":\") > 0) || (path.IndexOf("://") > 0))
                {
                    return path;
                }
                if (Context.CurrentHttpContext != null)
                {
                    return Context.CurrentHttpContext.Server.MapPath(path);
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

        

        public static string ApplicationPath
        {
            get
            {
                string applicationPath = "/";
                if (Context.CurrentHttpContext != null)
                {
                    applicationPath = Context.CurrentHttpContext.Request.ApplicationPath;
                }
                else
                {
                    applicationPath = HttpRuntime.AppDomainAppVirtualPath;
                }
                if (applicationPath == "/")
                {
                    return string.Empty;
                }
                return applicationPath;
            }
        }



    }
}
