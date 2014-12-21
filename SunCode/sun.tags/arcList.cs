using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sun.HtmlEngine.Tags;

namespace sun.tags
{
    public class arcList : ParseList
    {
        public arcList(string attributes, string innerHtml) : base(attributes, innerHtml) { }

        protected override IList getCurrentData()
        {

            Sun.API.Pagelet.ApiArchive apiArchive = new Sun.API.Pagelet.ApiArchive();

            //int gid = Sun.Toolkit.Parse.ToInt(base.attributes["channelId"], -1);
            int gid = Sun.Toolkit.Parse.ToInt(10, -1);

            List<Sun.Entity.Pagelet.EntityArchive> archives = apiArchive.getListByGroupId(gid, true);

            return archives;
        }
    }
}
