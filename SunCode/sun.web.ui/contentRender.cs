using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using sun.generating;
using System.Reflection;

namespace sun.web.ui
{

    [ToolboxData("<{0}:contentRender runat=server></{0}:contentRender>")]
    public class contentRender : WebControl
    {
        private static void setEntityChannelToContext(Sun.Entity.Pagelet.EntityChannel entChannel, Template tmp)
        {
            if (entChannel != null)
            {
                foreach (PropertyInfo item in entChannel.GetType().GetProperties())
                {
                    string value = item.GetValue(entChannel, null).ToString();
                    if ((item.CanRead) && (item.CanWrite))
                    {
                        tmp.attributes.add(item.Name, value);
                    }
                }
            }
        }

        private void generateContent(HtmlTextWriter writer)
        {
            string html = "";

            Sun.API.Pagelet.ApiArchive apiArchive = new Sun.API.Pagelet.ApiArchive();
            Sun.API.Pagelet.ApiChannel apiChannel = new Sun.API.Pagelet.ApiChannel();

            int archiveId = Sun.Toolkit.Parse.ToInt(Sun.Toolkit.context.GetQueryString()["archiveId"], -1);

            Sun.Entity.Pagelet.EntityArchive archiveData = apiArchive.getArchiveById(archiveId);
            if (archiveData != null)
            {
                // 得到文章对应栏目的数据
                Sun.Entity.Pagelet.EntityChannel entChannel = apiChannel.getChannelWithChildrenById(archiveData.groupID);

                if (!string.IsNullOrEmpty(entChannel.templateBody))
                {
                    Template tmp = new Template(templateHelper.getTemplatePath(entChannel.templateBody));

                    tmp.currentData = entChannel;
                    tmp.attributes.add("archiveId", archiveId.ToString());
                    setEntityChannelToContext(entChannel, tmp);
                    html = tmp.render();
                }
                else
                {
                    html = "此文章对应的栏目数据不存在。";
                }

                writer.Write(html);
            }
            else
            {
                pageNotFound.render(writer);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.Page.Response.ContentEncoding = Encoding.GetEncoding("gb2312");
            this.Page.Response.Charset = "gb2312";
            this.generateContent(writer);
        }
    }

}
