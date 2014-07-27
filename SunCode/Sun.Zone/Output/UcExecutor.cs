using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;
using System.IO;

namespace Sun.Zone
{
	/// <summary>
	/// 用于呈现（Render）用户控件的执行器
	/// </summary>
	public static class UcExecutor
	{

		/// <summary>
		/// 用指定的用户控件以及视图数据呈现结果，最后返回生成的HTML代码。
		/// 用户控件应从MyUserControlView&lt;T&gt;继承
		/// </summary>
		/// <param name="ucVirtualPath">用户控件的虚拟路径</param>
		/// <param name="model">视图数据</param>
		/// <returns>生成的HTML代码</returns>
		public static string Render(string ucVirtualPath, object model)
		{
			if( string.IsNullOrEmpty(ucVirtualPath) )
				throw new ArgumentNullException("ucVirtualPath");
			
			Page page = new Page();
			Control ctl = page.LoadControl(ucVirtualPath);
			if( ctl == null )
				throw new InvalidOperationException(
					string.Format("指定的用户控件 {0} 没有找到。", ucVirtualPath));

			if( model != null ) {
				MyBaseUserControl myctl = ctl as MyBaseUserControl;
				if( myctl != null )
					myctl.SetModel(model);
			}

			// 将用户控件放在Page容器中。
			page.Controls.Add(ctl);

			StringWriter output = new StringWriter();
			HtmlTextWriter write = new HtmlTextWriter(output, string.Empty);
			page.RenderControl(write);

			// 用下面的方法也可以的。
			//HttpContext.Current.Server.Execute(page, output, false);

			return output.ToString();
		}



		/// <summary>
		/// 用指定的用户控件以及视图数据呈现结果，
		/// 然后将产生的HTML代码写入HttpContext.Current.Response
		/// 用户控件应从MyUserControlView&lt;T&gt;继承
		/// </summary>
		/// <param name="ucVirtualPath">用户控件的虚拟路径</param>
		/// <param name="model">视图数据</param>
		/// <param name="flush">是否需要在输出html后调用Response.Flush()</param>
		[Obsolete("这个方法即将被移除，请调用ResponseWriter中的WriteUserControl方法来代替。")]
		public static void ResponseWrite(string ucVirtualPath, object model, bool flush)
		{
			ResponseWriter.WriteUserControl(ucVirtualPath, model, flush);
		}



	}


}
