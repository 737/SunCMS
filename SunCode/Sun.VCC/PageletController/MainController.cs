using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sun.Zone;

namespace Sun.Core.Pagelet
{
    public class MainController
    {
        [Action]
        [PageUrl(Url = "/suncms/main.aspx")]
        public static PageResult LoadModel()
        {
            MainModel model = new MainModel();

            Sun.Entity.SysMenu menu = new Entity.SysMenu() { Name = "alex" };
            model.Menu = menu;

            string url = "/suncms/main.aspx";

            return new PageResult(url, model);
        }

    }
}
