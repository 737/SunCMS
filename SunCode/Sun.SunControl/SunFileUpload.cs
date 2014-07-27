using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Sun.SunControl
{
    public class SunFileUpload : FileUpload, ISunControl
    {
        public object Value
        {
            get
            {
                return this.Value;
            }
            set
            {
                if (value == null)
                {
                    this.Value = value;
                }
            }
        }
    }
}
