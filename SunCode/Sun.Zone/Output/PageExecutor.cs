using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Compilation;
using System.IO;
using System.Web.UI;

namespace Sun.Zone
{
	public static class PageExecutor
	{
		internal static readonly Type MyPageViewOpenType = typeof(PageView<>);

		public static void TrySetPageModel(HttpContext context)
		{
			if( context == null || context.Handler == null )
				return;

			IHttpHandler handler = context.Handler;

			// 判断当前处理器是否从MyPageView<TModel>继承过来
			Type handlerType = handler.GetType().BaseType;
			if( handlerType.IsGenericType &&
				handlerType.GetGenericTypeDefinition() == MyPageViewOpenType ) {

				// 查找能响应这个请求的Action，并获取视图数据。
				InvokeInfo vkInfo = ReflectionHelper.GetPageActionInvokeInfo(
										context.Request.FilePath);

				object model = ActionExecutor.ExecuteActionInternal(context, vkInfo);

				// 设置页面Model
				SetPageModel(context.Handler, model);
			}
		}


		private static void SetPageModel(IHttpHandler handler, object model)
		{
			if( handler == null )
				return;

			if( model != null ) {
				MyBasePage page = handler as MyBasePage;
				if( page != null )
					page.SetModel(model);
			}
		}


		/// <summary>
		/// 用指定的页面路径以及视图数据呈现结果，最后返回生成的HTML代码。
		/// 页面应从MyPageView&lt;T&gt;继承
		/// </summary>
		/// <param name="context">HttpContext对象</param>
		/// <param name="pageVirtualPath">Page的虚拟路径</param>
		/// <param name="model">视图数据</param>
		/// <returns>生成的HTML代码</returns>
		public static string Render(HttpContext context, string pageVirtualPath, object model)
		{
			if( context == null )
				throw new ArgumentNullException("context");
			if( string.IsNullOrEmpty(pageVirtualPath) )
				throw new ArgumentNullException("pageVirtualPath");


			Page handler = BuildManager.CreateInstanceFromVirtualPath(
											pageVirtualPath, typeof(object)) as Page;
			if( handler == null )
				throw new InvalidOperationException(
					string.Format("指定的路径 {0} 不是一个有效的页面。", pageVirtualPath));


			SetPageModel(handler, model);

			StringWriter output = new StringWriter();
			context.Server.Execute(handler, output, false);
			return output.ToString();
		}

		/// <summary>
		/// 用指定的Page以及视图数据呈现结果，
		/// 然后将产生的HTML代码写入HttpContext.Current.Response
		/// 用户控件应从MyPageView&lt;T&gt;继承
		/// </summary>
		/// <param name="pageVirtualPath">Page的虚拟路径</param>
		/// <param name="model">视图数据</param>
		/// <param name="flush">是否需要在输出html后调用Response.Flush()</param>
		[Obsolete("这个方法即将被移除，请调用ResponseWriter中的WritePage方法来代替。")]
		public static void ResponseWrite(string pageVirtualPath, object model, bool flush)
		{
			ResponseWriter.WritePage(pageVirtualPath, model, flush);
		}


	}
}
