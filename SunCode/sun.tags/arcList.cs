using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sun.generating.tags;

namespace sun.tags
{
    public class arcList : dataList
    {
        public arcList(string exp) : base(exp) { }
        public arcList(string exp, string innerHtml) : base(exp, innerHtml) { }

        protected override object getProvider()
        {
            Sun.API.Pagelet.ApiArchive apiArchive = new Sun.API.Pagelet.ApiArchive();

            int gid = Sun.Toolkit.Parse.ToInt(base.attributes["channelId"], -1);

            if (gid == -1)
            {
                gid = Sun.Toolkit.Parse.ToInt(base.context.attributes["channelId"], -1);
            }

            List<Sun.Entity.Pagelet.EntityArchive> archives = apiArchive.getListByGroupId(gid, true);

            return archives;
        }
    }
}
