using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.Drawing;

namespace Sun.SunControl
{
    [ToolboxBitmap(typeof(AuthCode), "Resources.Image.Control.bmp"), ToolboxData("<{0}:AuthCode runat=server ></{0}:AuthCode>")]
    public class AuthCode : System.Web.UI.WebControls.Image
    {
        protected override void Render(HtmlTextWriter writer)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(this.BackGroundColor))
            {
                list.Add("backGroundColor = " + this.BackGroundColor);
            }
            if (this.FontSize > 0)
            {
                list.Add("fontSize =" + this.FontSize);
            }
            if (!string.IsNullOrEmpty(this.FontColor))
            {
                list.Add("fontColor=" + this.FontColor);
            }
            if (!this.IgnoreCase)
            {
                list.Add("ignoreCase=" + this.IgnoreCase);
            }
            if (this.MaxLength > 0)
            {
                list.Add("maxLength=" + this.MaxLength);
            }
            string _str = string.Join("&", list.ToArray());
            this.Attributes.Add("onclick", "this.src='" + base.ResolveUrl("~/authcode.ashx?code ='+Math.random()+") + "'" + (String.IsNullOrEmpty(_str) ? "" : _str.Insert(0, "&")) + "'");
            this.ImageUrl = base.ResolveUrl("~/authcode.ashx") + (String.IsNullOrEmpty(_str) ? "" : _str.Insert(0, "?"));
            base.Render(writer);
        }

        /// <summary>
        /// //进行匹配
        /// </summary>
        /// <returns></returns>
        public bool isMatch()
        {
            string inputCode = Util.Context.CurrentHttpContext.Request.Params[this.ValidateControlId];
            if (inputCode == null)
            {
                TextBox t = this.Parent.FindControl(this.ValidateControlId) as TextBox;
                if (t != null)
                {
                    inputCode = t.Text;
                }
            }
            return AuthCodeHandler.IsMatch(inputCode);
        }

        [DefaultValue(true), Description("设置背景色"), Category("SunCMS")]
        public string BackGroundColor
        {
            get
            {
                object obj = this.ViewState["BackGroundColor"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["BackGroundColor"] = value;
            }
        }

        [Description("设置字体大小"), Category("SunCMS")]
        public byte FontSize
        {
            get
            {
                object obj = this.ViewState["FontSize"];
                if (obj != null)
                {
                    return (byte)obj;
                }
                return 0;
            }
            set
            {
                this.ViewState["FontSize"] = value;
            }
        }

        [DefaultValue(true), Description("字体颜色"), Category("SunCMS")]
        public string FontColor
        {
            get
            {
                object obj = this.ViewState["FontColor"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["FontColor"] = value;
            }
        }

        [DefaultValue(true), Description("忽略大小写"), Category("SunCMS")]
        public bool IgnoreCase
        {
            get
            {
                object obj = this.ViewState["IgnoreCase"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["IgnoreCase"] = value;
            }
        }

        [Description("验证码最大长度"), Category("SunCMS")]
        public int MaxLength
        {
            get
            {
                object obj = this.ViewState["MaxLength"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 0;
            }
            set
            {
                this.ViewState["MaxLength"] = value;
            }
        }

        [Description("需要验证控件的ID"), Category("SunCMS")]
        public string ValidateControlId
        {
            get
            {
                object obj = this.ViewState["ValidateControlId"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return null;
            }
            set
            {
                this.ViewState["ValidateControlId"] = value;
            }
        }

    }
}
