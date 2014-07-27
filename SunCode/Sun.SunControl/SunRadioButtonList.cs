using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Sun.SunControl
{
    public class SunRadioButtonList : System.Web.UI.WebControls.RadioButtonList, ISunControl
    {

        private object __value;
        public object Value
        {
            get
            {
                return this.__value;
            }
            set
            {
                this.__value = value;
            }
        }
    }
}
