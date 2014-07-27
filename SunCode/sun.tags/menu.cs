using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sun.generating.tags;
using Sun.API;
using Sun.Entity;

namespace sun.tags
{
    public class menu : dataList
    {
        public menu(string exp) : base(exp) { }
        public menu(string exp, string innerHtml)
            : base(exp, innerHtml) { }

        protected override object getProvider()
        {
            Sun.API.Pagelet.ApiChannel apiChannel = new Sun.API.Pagelet.ApiChannel();

            int cid = Sun.Toolkit.Parse.ToInt(base.attributes["channelId"], -1);
            if (cid == -1)
            {
                cid = Sun.Toolkit.Parse.ToInt(base.context.attributes["channelId"], -1);
            }

            var channelList = apiChannel.getChannelsWithChildrenById(cid);

            return channelList;
        }
    }
}
