using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sun.generating.tags;
using Sun.API.Pagelet;
using Sun.Entity.Pagelet;

namespace sun.tags
{
    public class view : dataView
    {
        public view(string exp) : base(exp) { }
        public view(string exp, string innerHtml) : base(exp, innerHtml) { }

        protected override object getProvider()
        {
            ApiArchive api = new ApiArchive();

            int id = Sun.Toolkit.Parse.ToInt(base.attributes["id"], -1);

            if (id == -1)
            {
                id = Sun.Toolkit.Parse.ToInt(base.context.attributes["archiveId"], -1);
            }

            EntityArchive archive = api.getArchiveById(id);

            return archive;
        }

    }
}
