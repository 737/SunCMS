using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sun;

namespace Sun.SunControl
{
    public class SunBoolean : Literal, ISunControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            string txt = this.Value.ToString().Trim().ToLower();

            switch (txt)
            {
                case "yes":
                case "1":
                case "true":
                    txt = "yes";
                    break;
                default:
                    txt = "no";
                    break;
            }

            base.Text = HelperTranslation.GetString(txt);
            base.Render(writer);

        }

        public object Value
        {
            get;
            set;
        }
    }
}
