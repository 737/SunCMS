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
            ApiChannel apiChannels = new ApiChannel();

            var fillEntity = Sun.HelperContext.GetFillEntity<EntityArchive>();
            var html = "文章ID不正确或内容不存在";

            if (!string.IsNullOrEmpty(fillEntity.id.ToString())) {
                EntityArchive entArchive = archive.getArchiveById(fillEntity.id);

                if (entArchive != null) {
                    EntityChannel channel = apiChannels.getChannelWithChildrenById(entArchive.groupID);

                    if (channel != null) {
                        var path = templateHelper.getTemplatePath(channel.templateBody);

                        Template tmp = new Template(path, channel);

                        html = tmp.render();
                    }
                }
                
            }

            return html;
        }

        protected override void Render(HtmlTextWriter writer) {
            this.Page.Response.ContentEncoding = Encoding.GetEncoding("gb2312");
            this.Page.Response.Charset = "gb2312";

            var html = this.generateContent(writer);

            writer.Write(html);
        }

    }
}
