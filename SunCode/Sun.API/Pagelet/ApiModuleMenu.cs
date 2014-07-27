using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using Sun.UI;
using Sun.Zone;
using Sun.Toolkit;

namespace Sun.API.Pagelet
{
    /// <summary>
    /// //返回系统主菜单
    /// </summary>
    public class ApiModuleMenu : ApiController
    {
        private SystemMenu menu = new SystemMenu();

        [Action(Verb = "GET")]
        public string Menu(string key)
        {
            string json;

            if (string.IsNullOrEmpty(key))
            {
                key = "core";
            }
            object obj = menu.GetMenusJSON(key.Trim());

            if (obj != null)
            {
                json = JSON.GetPackJSON(true, obj);
            }
            else
            {
                json = JSON.GetPackJSON(false, "系统数据返回不正确");
            }

            return json;
        }

    }



}
