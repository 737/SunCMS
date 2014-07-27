using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sun;

namespace Sun.SunControl
{
    public class SunLiteral : Literal, ISunControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            base.Text = HelperTranslation.Replace(this.Text.ToString());
            base.Render(writer);
        }

        public object Value
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = (value == null) ? "" : value.ToString();
            }
        }
    }
}
