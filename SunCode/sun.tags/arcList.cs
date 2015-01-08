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
            
            int cid = Sun.Toolkit.Parse.ToInt(base.attributes["channelId"], -1);

            if (cid == -1) {
                var _cid =  Sun.HelperContext.GetQueryString("channelid");

                cid = Sun.Toolkit.Parse.ToInt(_cid, -1);
            }

            if (cid == -1) {
                return null;
            }

            List<Sun.Entity.Pagelet.EntityArchive> archives = apiArchive.getListByGroupId(cid, true);

            return archives;
        }
    }
}
