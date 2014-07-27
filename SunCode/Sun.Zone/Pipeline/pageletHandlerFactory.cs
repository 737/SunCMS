using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace Sun.Zone
{

    internal sealed class AspnetPageHandlerFactory : PageHandlerFactory { }

    public sealed class pageletHandlerFactory : IHttpHandlerFactory
    {
        private AspnetPageHandlerFactory _msPageHandlerFactory;

        /// <summary>
        /// 尝试根据当前请求，获取一个有效的Action，并返回ActionHandler
        /// 此方法可以在HttpModule中使用，用于替代httpHandler的映射配置
        /// </summary>
        public static IHttpHandler TryGetHandler(HttpContext context)
        {
            InvokeInfo vkInfo = ReflectionHelper.GetPageActionInvokeInfo(context.Request.FilePath);
            if (vkInfo == null)
                return null;

            return ActionHandler.CreateHandler(vkInfo);
        }

        #region IHttpHandlerFactory类实现
        IHttpHandler IHttpHandlerFactory.GetHandler(HttpContext context, string requestType, string virtualPath, string physicalPath)
        {
            // 尝试根据请求路径获取Action
            InvokeInfo vkInfo = ReflectionHelper.GetPageActionInvokeInfo(virtualPath);

            // 如果没有找到合适的Action，并且请求的是一个ASPX页面，则按ASP.NET默认的方式来继续处理
            if (vkInfo == null && virtualPath.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
            {
                if (_msPageHandlerFactory == null)
                {
                    _msPageHandlerFactory = new AspnetPageHandlerFactory();
                }

                // 调用ASP.NET默认的Page处理器工厂来处理
                return _msPageHandlerFactory.GetHandler(context, requestType, virtualPath, physicalPath);
            }

            return ActionHandler.CreateHandler(vkInfo);
        }

        void IHttpHandlerFactory.ReleaseHandler(IHttpHandler handler) { }
        #endregion
    }


}
