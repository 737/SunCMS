using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Sun.Zone
{
	/// <summary>
	/// 表示一个用户控件结果（用户控件将由框架执行）
	/// </summary>
	public sealed class UcResult : IActionResult
	{
		public string VirtualPath { get; private set; }
		public object Model { get; private set; }

		public UcResult(string virtualPath, object model)
		{
			if( string.IsNullOrEmpty(virtualPath) )
				throw new ArgumentNullException("virtualPath");

			this.VirtualPath = virtualPath;
			this.Model = model;
		}

		void IActionResult.Ouput(HttpContext context)
		{
			context.Response.ContentType = "text/html";
			string html = UcExecutor.Render(VirtualPath, Model);
			context.Response.Write(html);
		}
	}

}
