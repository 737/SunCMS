using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sun;
using Sun.API.Pagelet;
using Sun.Entity.Pagelet;
using Sun.HtmlEngine;

namespace Sun.Web.UI
{
    public class contentRender : WebControl
    {
        private string generateContent(HtmlTextWriter writer) {
            ApiArchive archive = new ApiArchive();

            var fillEntity = Sun.HelperContext.GetFillEntity<EntityArchive>();
            var html = "";

            if (string.IsNullOrEmpty(fillEntity.id.ToString())) {
                html = "文章ID不正确";
            } else {
                //var tmp = templateHelper.getTemplatePath();

                //if (string.IsNullOrEmpty()) {

                //} else { 
                    
                //}
            }

            Sun.API.Pagelet.ApiChannel apiChannels = new Sun.API.Pagelet.ApiChannel();


            //EntityChannel channel = apiChannels.getChannelWithChildrenById(fillEntity.groupID);


            //var cc = Sun.Toolkit.JSON.stringify(channel);

            //return html;
            return "我是内容";
        }

        protected override void Render(HtmlTextWriter writer) {
            this.Page.Response.ContentEncoding = Encoding.GetEncoding("gb2312");
            this.Page.Response.Charset = "gb2312";

            var html = this.generateContent(writer);

            writer.Write(html);
        }

    }
}
