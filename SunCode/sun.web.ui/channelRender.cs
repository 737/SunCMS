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
    [ToolboxData("<{0}:channelRender runat=server></{0}:channelRender>")]
    public class channelRender : WebControl
    {
        private static void setContextItem(Sun.Entity.Pagelet.EntityChannel channel, template tmp)
        {
            if (channel != null)
            {
                foreach (PropertyInfo item in channel.GetType().GetProperties())
                {
                    string value = item.GetValue(channel, null).ToString();
                    if ((item.CanRead) && (item.CanWrite))
                    {
                        tmp.attributes.add(item.Name, value);
                    }
                }
            }
        }

        private void generateContent(HtmlTextWriter writer)
        {
            Sun.API.Pagelet.ApiChannel apiChannels = new Sun.API.Pagelet.ApiChannel();

            var pid = Sun.Toolkit.context.getValueToInt("channelId");
            var html = "";

            if (pid == null)
            {
                html = "栏目传参不正确!";
            }
            else
            {
                Sun.Entity.Pagelet.EntityChannel channel = apiChannels.getChannelWithChildrenById(pid);

                if ((channel != null) && (!string.IsNullOrEmpty(channel.templateList)))
                {
                    template tmp = new template(templateHelper.getTemplatePath(channel.templateList));

                    tmp.currentData = channel;
                    setContextItem(channel, tmp);
                    html = tmp.render();
                }
                else
                {
                    html = "栏目不存在!";
                }
            }
            writer.Write(html);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.Page.Response.ContentEncoding = Encoding.GetEncoding("gb2312");
            this.Page.Response.Charset = "gb2312";
            this.generateContent(writer);
        }
    }
}
