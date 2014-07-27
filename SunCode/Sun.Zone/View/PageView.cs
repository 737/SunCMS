using System;
using System.Collections.Generic;
using System.Text;
using System.Web;


namespace Sun.Zone
{
	/// <summary>
	/// 页面视图的基类
	/// </summary>
	/// <typeparam name="TModel">传递给页面呈现时所需的数据实体对象类型</typeparam>
	public class PageView<TModel> : MyBasePage
	{	
		/// <summary>
		/// 用于页面呈现时所需的数据实体对象
		/// </summary>
		public TModel Model { get; set; }


		public override void SetModel(object model)
		{
			try {
				this.Model = (TModel)model;
			}
			catch( Exception ex ) {
				throw new ArgumentException("参数model与目标类型不匹配。", ex);
			}
		}
	}



}
