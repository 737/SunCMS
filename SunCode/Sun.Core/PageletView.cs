using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Sun.SunControl;
using System.IO;

namespace Sun.Core
{
    public class PageletView : TemplateControl
    {
        private string __pageletModelu;
        private string __pageletName;
        private Sun.EPageletAction __pageletActon;

        public PageletView(string pageletModule, string pageletName, Sun.EPageletAction pageletAction)
        {
            this.__pageletModelu = pageletModule;
            this.__pageletName = pageletName;
            this.__pageletActon = pageletAction;
        }

        public Control GetView()
        {
            Control child = null;
            string errorMessage = "", controlPath;

            if (!string.IsNullOrEmpty(this.__pageletName))
            {
                controlPath = this.GetResolveUrl();
                if (File.Exists(Util.Path.GetMapPath(controlPath)))
                {
                    try
                    {
                        child = base.LoadControl(controlPath);
                        //child.ID = "suncms_"+ this.Pagelet ;
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message + "加载失败：文件有内部错误!";
                    }
                }
                else
                {
                    errorMessage = "加载失败：文件不存在!";
                }
            }
            else
            {
                errorMessage = "加载失败：文件名输入有错误!";
            }

            if (child == null)
            {
                SunLiteral lit = new SunLiteral();
                lit.Text = errorMessage;

                child = lit;
            }
            return child;
        }

        public string GetResolveUrl()
        {
            string virtualPath,
                   pageRoot = "~/suncms/pagelet/";   //TODO:: pageRoot = global 定义在config文件中的一个值

            if ((!string.IsNullOrEmpty(this.__pageletModelu)) && (!string.IsNullOrEmpty(this.__pageletName)))
            {
                switch (this.__pageletActon)
                {
                    case EPageletAction.INSERT:
                    case EPageletAction.DELETE:
                    case EPageletAction.UPDATE:
                        virtualPath = string.Format("{0}{1}/{2}.{3}.{4}", pageRoot, this.__pageletModelu, this.__pageletName, "handler", "ascx");
                        break;
                    case EPageletAction.SELECT:
                    default:
                        virtualPath = string.Format("{0}{1}/{2}.{3}", pageRoot, this.__pageletModelu, this.__pageletName, "ascx");
                        break;
                }
            }
            else
            {
                virtualPath = pageRoot;
            }

            return virtualPath;
        }

        public string GetName()
        {
            return this.GetName(this.PageletName);
        }
        public string GetName(List<string> pageletName)
        {
            return string.Join("", this.PageletName);
        }

        public List<string> PageletName { get; set; }
    }
}
