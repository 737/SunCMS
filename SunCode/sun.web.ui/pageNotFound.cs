using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Sun.Web.UI
{
    public static class pageNotFound
    {
        public static void render(HtmlTextWriter writer)
        {
            writer.Write("从前有座山 山里有座庙 庙里有个页面 现在找不到了...");
        }

    }
}
