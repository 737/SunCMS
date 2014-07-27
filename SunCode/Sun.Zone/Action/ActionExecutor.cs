using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using System.Collections.Specialized;
using OptimizeReflection;
using Sun.Zone.Serializer;

namespace Sun.Zone
{
	public static class ActionExecutor
	{
		/// <summary>
		/// 执行一次Ajax调用请求。例如："/AjaxOrder/AddOrder.cspx"
		/// </summary>
		/// <param name="context"></param>
		/// <param name="pair"></param>
		public static void ExecuteAjaxAction(HttpContext context, ControllerActionPair pair)
		{
			if( context == null )
				throw new ArgumentNullException("context");
			if( pair == null )
				throw new ArgumentNullException("pair");

			InvokeInfo vkInfo = ReflectionHelper.GetAjaxInvokeInfo(pair, context.Request);
			if( vkInfo == null )
				ExceptionHelper.Throw404Exception(context);

			ExecuteAction(context, vkInfo);
		}

		/// <summary>
		/// 执行指定的页面请求。例如："/Pages/Categories.aspx"
		/// </summary>
		/// <param name="context"></param>
		/// <param name="virtualPath"></param>
		/// <returns></returns>
		public static void ExecutePageAction(HttpContext context, string virtualPath)
		{
			if( context == null )
				throw new ArgumentNullException("context");
			if( string.IsNullOrEmpty(virtualPath) )
				throw new ArgumentNullException("virtualPath");

			// 根据请求路径获取对应的调用信息
			InvokeInfo vkInfo = ReflectionHelper.GetPageActionInvokeInfo(virtualPath);
			if( vkInfo == null )
				ExceptionHelper.Throw404Exception(context);

			ExecuteAction(context, vkInfo);
		}

		/// <summary>
		/// Sun.Zone的版本。（dll文件版本）
		/// </summary>
		private static readonly string MvcVersion
			= System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(ActionExecutor).Assembly.Location).FileVersion;


		private static void SetMvcVersionHeader(HttpContext context)
		{
			context.Response.AppendHeader("X-Sun.Zone-Version", MvcVersion);
		}

		internal static void ExecuteAction(HttpContext context, InvokeInfo vkInfo)
		{
			if( context == null )
				throw new ArgumentNullException("context");
			if( vkInfo == null )
				throw new ArgumentNullException("vkInfo");

			SetMvcVersionHeader(context);

			// 验证请求是否允许访问（身份验证）
			AuthorizeAttribute authorize = vkInfo.GetAuthorize();
			if( authorize != null ) {
				if( authorize.AuthenticateRequest(context) == false )
					ExceptionHelper.Throw403Exception(context);
			}

			// 调用方法
			object result = ExecuteActionInternal(context, vkInfo);

			// 设置OutputCache
			OutputCacheAttribute outputCache = vkInfo.GetOutputCacheSetting();
			if( outputCache != null )
				outputCache.SetResponseCache(context);

			// 处理方法的返回结果
			IActionResult executeResult = result as IActionResult;
			if( executeResult != null ) {
				executeResult.Ouput(context);
			}
			else {
				if( result != null ) {
					// 普通类型结果
					context.Response.ContentType = "text/plain";
					context.Response.Write(result.ToString());
				}
			}
		}

		internal static object ExecuteActionInternal(HttpContext context, InvokeInfo info)
		{
			// 准备要传给调用方法的参数
			object[] parameters = GetActionCallParameters(context, info.Action);

			// 调用方法
			if( info.Action.HasReturn )
				//return info.Action.MethodInfo.Invoke(info.Instance, parameters);
				return info.Action.MethodInfo.FastInvoke(info.Instance, parameters);

			else {
				//info.Action.MethodInfo.Invoke(info.Instance, parameters);
				info.Action.MethodInfo.FastInvoke(info.Instance, parameters);
				return null;
			}
		}


		private static object[] GetActionCallParameters(HttpContext context, ActionDescription action)
		{
			if( action.Parameters == null || action.Parameters.Length == 0 )
				return null;

			IActionParametersProvider provider = ActionParametersProviderFactory.CreateActionParametersProvider(context.Request);
			return provider.GetParameters(context.Request, action);
		}

	}
}
