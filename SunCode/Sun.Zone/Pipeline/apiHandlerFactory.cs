using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Sun.Zone
{
    internal sealed class apiHandlerFactory : IHttpHandlerFactory
    {
        public IHttpHandler GetHandler(HttpContext context, string requestType, string virtualPath, string physicalPath)
        {
            //    http://localhost:12345/sun/api/AjaxDemo/add.cspx?a=3&b=4



            virtualPath = Toolkit.FormatUrl(virtualPath);

            // 根据请求路径，定位到要执行的Action
            ControllerActionPair pair = UrlParser.ParseAjaxUrl(virtualPath);
            if (pair == null)
                ExceptionHelper.Throw404Exception(context);

            // 获取内部表示的调用信息
            InvokeInfo vkInfo = ReflectionHelper.GetAjaxInvokeInfo(pair, context.Request);
            if (vkInfo == null)
                ExceptionHelper.Throw404Exception(context);

            // 创建能够调用Action的HttpHandler
            return ActionHandler.CreateHandler(vkInfo);
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }
    }

}
