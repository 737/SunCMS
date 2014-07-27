using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Sun.diagnostics;
using System.Text.RegularExpressions;

namespace Sun
{
    /// <summary>
    /// // 作用：
    /// // 1.url 重写
    /// </summary>
    public class sunHttpModule : IHttpModule
    {   
        /// <summary>
        /// // 解析 lookfor 使用的路径
        /// </summary>
        private string parseAbsolutePath(string absPath, string lookForUrl)
        {
            if ((lookForUrl.Length == 0) || (lookForUrl[0] != '~'))
            {
                return lookForUrl;
            }
            if (lookForUrl.Length == 1)
            {
                return absPath;
            }
            if ((lookForUrl[1] == '/') || (lookForUrl[1] == '\\'))
            {
                return ("/" + lookForUrl.Substring(2));
            }
            if (absPath.Length > 1)
            {
                return (absPath + "/" + lookForUrl.Substring(1));
            }
            return (absPath + lookForUrl.Substring(1));
        }

        private void rewriterUrl(HttpContext context, string sendToUrl)
        {
            string sendToUrlLessQuery = "",
                   queryStrings = "";

            if (context.Request.QueryString.Count > 0)
            {
                if (sendToUrl.IndexOf('?') != -1)
                {
                    sendToUrl = sendToUrl + "&" + context.Request.QueryString.ToString();
                }
                else
                {
                    sendToUrl = sendToUrl + "?" + context.Request.QueryString.ToString();
                }
            }
            if (sendToUrl.IndexOf('?') > 0)
            {
                sendToUrlLessQuery = sendToUrl.Substring(0, sendToUrl.IndexOf('?'));
                queryStrings = sendToUrl.Substring(sendToUrl.IndexOf('?') + 1);
            }
            context.RewritePath(sendToUrlLessQuery, string.Empty, queryStrings);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Init(HttpApplication app)
        {
            app.BeginRequest += app_BeginRequest;
        }

        protected virtual void app_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;

            this.rewriter(app.Request.Url.AbsolutePath, app);
        }

        /// <summary>
        /// // 重写URL
        /// </summary>
        protected virtual void rewriter(string requestPath, HttpApplication app)
        {
            Regex reg;
            string pattern = "", sendToUrl = "";
            List<rewriterRule> rules = configRewriter.getConfig().rules;

            for (int i = 0; i < rules.Count; i++)
            {
                pattern = string.Format("^{0}$", this.parseAbsolutePath(requestPath, rules[i].lookFor));
                reg = new Regex(pattern, RegexOptions.IgnoreCase);
                
                if (reg.IsMatch(requestPath))
                {
                    sendToUrl = this.parseAbsolutePath(requestPath, reg.Replace(requestPath, rules[i].sendTo));

                    this.rewriterUrl(app.Context, sendToUrl);
                    return;
                }
            }
        }

    }
}
