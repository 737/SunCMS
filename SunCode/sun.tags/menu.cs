using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sun.generating.tags;
using Sun.API;
using Sun.Entity;
using Sun.HtmlEngine.Tags;
using System.Collections;

namespace sun.tags
{
    public class menu : ParseList
    {
        public menu(string attributes, string innerHtml) : base(attributes, innerHtml) { }

        protected override IList getCurrentData()
        {
            Sun.API.Pagelet.ApiChannel apiChannel = new Sun.API.Pagelet.ApiChannel();

            int cid = Sun.Toolkit.Parse.ToInt(base.attributes["channelId"], -1);

            var channelList = apiChannel.getChannelsWithChildrenById(cid);

            return channelList;
        }
    }
}
