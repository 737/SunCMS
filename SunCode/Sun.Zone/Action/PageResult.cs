using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Sun.Zone
{
	
	/// <summary>
	/// 表示一个页面结果（页面将由框架执行）
	/// </summary>
	public sealed class PageResult : IActionResult
	{
		public string VirtualPath { get; private set; }
		public object Model { get; private set; }

		public PageResult(string virtualPath, object model)
		{
			this.VirtualPath = virtualPath;
			this.Model = model;
		}

		void IActionResult.Ouput(HttpContext context)
		{
			if( string.IsNullOrEmpty(this.VirtualPath) )
				this.VirtualPath = context.Request.FilePath;

			context.Response.ContentType = "text/html";
			string html = PageExecutor.Render(context, VirtualPath, Model);
			context.Response.Write(html);
		}
	}


}
