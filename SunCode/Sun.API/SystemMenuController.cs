using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using Sun.UI;

namespace Sun.API
{
    /// <summary>
    /// //返回系统主菜单
    /// </summary>
    public class SystemMenuController : ApiController
    {
        private SystemMenu menu = new SystemMenu();
        private PackJSON packjson;

        public object Get(string sKey)
        {
            object obj = menu.GetMenusJSON(sKey.Trim());

            if (obj != null)
            {
                packjson = new PackJSON(true, obj);
            }
            else
            {
                packjson = new PackJSON(false, "系统数据返回不正确");
            }

            return packjson;
        }

    }



}
