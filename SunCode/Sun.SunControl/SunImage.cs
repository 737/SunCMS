using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sun.SunControl
{
    public class SunImage : Image, ISunControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            this.ImageUrl = this.Value as string;
            base.Render(writer);
        }

        public object Value
        {
            get;
            set;
        }
    }
}
