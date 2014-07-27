using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using Sun;

namespace Sun.SunControl
{
    public class SunButton : Button, ISunControl
    {
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {

            base.Text = HelperTranslation.Replace(this.Text.ToString());
            base.Render(writer);
        }


        public object Value
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
