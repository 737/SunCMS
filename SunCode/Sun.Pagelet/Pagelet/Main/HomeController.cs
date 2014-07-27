using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sun.Zone;

namespace Sun.Pagelet
{
    public class HomeController : PageletBase
    {
        [Action]
        [PageUrl(Url = "/sun/pagelet/home.aspx")]
        public static PageResult Index()
        {
            string url = PageletBase.MainPrefixUrl + "/home.aspx";

            return new PageResult(url, null);
        }



    }
}
