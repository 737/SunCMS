using System;
using System.Collections.Generic;
using System.Text;
using Sun.UI.Enum;
using Sun.Entity;

namespace Sun.UI
{
    /// <summary>
    /// // admin/frame/header.aspx
    /// </summary>
    public class SystemMenu
    {
        //翻译
        private string GetTranslater(params string[] list)
        {
            return HelperTranslation.GetString(list);
        }

        private SysMenuItem HelperCreateMenuItem(string sName, string sUrl)
        {
            return this.HelperCreateMenuItem(sName, sUrl, "", null, "");
        }
        private SysMenuItem HelperCreateMenuItem(string sName, string sUrl, string sIcoTitle, string icoType, string sIcoUrl)
        {
            SysMenuItem child = new SysMenuItem();
            child.Name = sName;
            child.Url = sUrl;
            child.IcoType = icoType;
            child.IcoTitle = sIcoTitle;
            child.IcoUrl = sIcoUrl;

            return child;
        }

        public SysMenuCollection GetMenusJSON(string sKey)
        {
            SysMenuCollection menus = new SysMenuCollection();

            switch (sKey)
            {
                case "core":
                    SysMenu ShortOperation = new SysMenu();
                    ShortOperation.Name = this.GetTranslater(new string[] { "Common", "Operation" });
                    ShortOperation.Children.AddItem(this.HelperCreateMenuItem(this.GetTranslater(new string[] { "System", "Home" }), "domain/home/index.html"));
                    ShortOperation.Children.AddItem(this.HelperCreateMenuItem("测试页", "/test"));
                    menus.AddItem(ShortOperation);
                    break;
                case "module":
                    SysMenu auxiliary = new SysMenu();
                    auxiliary.Name = this.GetTranslater(new string[] { "auxiliary", "plugin" });
                    auxiliary.Children.AddItem(
                        this.HelperCreateMenuItem(
                            this.GetTranslater(new string[] { "Friendly", "Link" }),
                            "module/friendlink/index.html",
                            this.GetTranslater(new string[] { "Add", "Friendly", "Link" }),
                            EIcoStyle.Add,
                            "module/friendlink/add.html"
                        )
                    );
                    auxiliary.Children.AddItem(
                        this.HelperCreateMenuItem(
                            "友情链接分组",
                            "module/FriendLinkGroup/index.html"
                        )
                    );
                    auxiliary.Children.AddItem(
                        this.HelperCreateMenuItem(
                            this.GetTranslater(new string[] { "Advertisement", "Manage" }),
                            "/advertisement",
                            this.GetTranslater(new string[] { "Add", "Advertisement", "Link" }),
                            EIcoStyle.Add,
                            "/advertisementedit"
                        )
                    );
                    menus.AddItem(auxiliary);
                    break;
                case "html":
                    //SysMenu channel = new SysMenu();
                    //channel.Name = this.GetTranslater(new string[] { "Channel", "Operation" });
                    //channel.Children.AddItem(this.HelperCreateMenuChild(
                    //    this.GetTranslater(new string[] { "Channel", "Manage" }),
                    //    Urls.GetUrl("siteinfo"),
                    //    this.GetTranslater(new string[] { "Create", "Channel" }),
                    //    "add",
                    //    "www.suncms.com")
                    //);
                    //menus.AddItem(channel);
                    //SysMenu channel2 = new SysMenu();
                    //channel2.Name = this.GetTranslater(new string[] { "Channel", "Operation" });
                    //channel2.Children.AddItem(this.HelperCreateMenuChild(
                    //    this.GetTranslater(new string[] { "Channel", "Manage" }),
                    //    Urls.GetUrl("siteinfo"),
                    //    this.GetTranslater(new string[] { "Create", "Channel" }),
                    //    "add",
                    //    "www.suncms.com")
                    //);
                    //menus.AddItem(channel2);
                    break;
                case "system":
                    SysMenu sysSetting = new SysMenu();
                    sysSetting.Name = this.GetTranslater(new string[] { "System", "Setting" });
                    sysSetting.Children.AddItem(this.HelperCreateMenuItem(this.GetTranslater(new string[] { "System", "Base", "Parameter" }), "/information"));
                    menus.AddItem(sysSetting);
                    break;
                default:
                    break;
            }

            return menus;
        }

    }
}
