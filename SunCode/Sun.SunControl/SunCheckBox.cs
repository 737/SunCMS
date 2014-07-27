using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Sun.SunControl
{

    public class SunCheckBox : CheckBox, ISunControl
    {


        public object Value
        {
            get
            {
                return this.Checked;
            }
            set
            {
                if (value == null)
                {
                    this.Checked = false;
                }
                else if (value.GetType() == typeof(bool))
                {
                    this.Checked = (bool)value;
                }
                else
                {
                    string str = value.ToString();
                    if ((str.ToLower() == "true") || (str == "1"))
                    {
                        this.Checked = true;
                    }
                    else
                    {
                        this.Checked = false;
                    }
                }
            }
        }
    }
}
