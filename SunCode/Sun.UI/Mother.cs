using Sun;
using Sun.Core;
using Sun.Data;
using Sun.SunControl;
using Sun.UI.SunControl;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sun.UI
{
    public class Mother : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            if (true)
            {
                //string title = HelperTranslation.GetString(this.HelperPagelet.GetQuery().GetPageletName());

                //if (!string.IsNullOrEmpty(title))
                //{
                //    base.Title = title;
                //}
                if (this.Region != null)
                {
                    this.Region.SetRegion(this.HelperPagelet);

                    Response.Write(Sun.Util.JSON.GetJSON(Sun.Toolkit.context.GetQueryDict()));
                }
            }
            else
            {
                Response.Redirect("main.aspx");
            }

            //---------------测试专用  有可能放到render事件当中
            //this.Test();
        }

        ////---------------测试页面专业
        //protected void Test()
        //{
        //    Sun.Console.WriterBR(Util.JSON.GetJSON(this.HelperPagelet.GetModel()));

        //    Repeater peater = this.FindControl("repetaer") as Repeater;
        //    peater.DataSource = this.HelperPagelet.GetModel();
        //    peater.DataBind();
        //}

        protected override void Render(HtmlTextWriter writer)
        {
            if (Context.Request.HttpMethod.ToLower() == "get")
            {
                //加这个判断是为了，只对test页面进行测试使用,,,
                if (base.Context.Request.QueryString["page"] == "main.test")
                {
                    //Util.Context.CurrentHttpContext.Response.Write(Sun.Util.JSON.GetPackJSON(true, this.Pagelet));
                    Sun.Console.Writer(Sun.Util.JSON.GetPackJSON(true, this.HelperPagelet.GetModel().GetData()));
                }
                else
                {
                    base.Render(writer);
                    //Sun.Console.Writer(Sun.Util.JSON.GetPackJSON(true, this.HelperPagelet.GetModel().GetData()));
                }
            }
            else
            {
                string actionType = Context.Request.Form["SunActionType"].Trim().ToLower();
                if ((actionType != null) && (this.HelperPagelet.GetModel() != null) && (Context.Request.Form.Count > 0))
                {
                    switch (actionType)
                    {
                        case "update":
                            //Data.Pagelet.PageletModel model = this.Region.UpdateRegionModel(this.HelperPagelet.GetModel());
                            //this.HelperPagelet.Update(model);
                            break;
                        case "delete":
                            break;
                    }
                }
                //TODO:: 只是为了测试数据有没有update 成功
                Util.Context.CurrentHttpContext.Response.Write(Sun.Util.JSON.GetPackJSON(true, this.HelperPagelet.GetModel()));
            }
        }

        private Region _region;
        public Region Region
        {
            get
            {
                if (this._region == null)
                {
                    this._region = Region.GetRegion("RegionPlace");
                }
                return this._region;
            }
        }

        private HelperPagelet __helperPagelet;
        public HelperPagelet HelperPagelet
        {
            get
            {
                if (this.__helperPagelet == null)
                {
                    this.__helperPagelet = new HelperPagelet();
                }
                return this.__helperPagelet;
            }
            set
            {
                if (this.__helperPagelet != value)
                {
                    this.__helperPagelet = value;
                }
            }
        }
    }
}