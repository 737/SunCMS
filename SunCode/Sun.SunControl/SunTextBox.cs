using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace Sun.SunControl
{
    public class SunTextBox : TextBox, ISunControl
    {
    //    public SunTextBoxTextStyle TextStyle
    //    {
    //        get
    //        {
    //            object obj = this.ViewState["ValueStyle"];
    //            if (obj != null)
    //            {
    //                return (SunTextBoxTextStyle)obj;
    //            }
    //            return SunTextBoxTextStyle.String;
    //        }
    //        set
    //        {
    //            this.ViewState["ValueStyle"] = value;
    //        }
    //    }

        //public override string Text
        //{
        //    get
        //    {
        //        object obj = this.ViewState["SunText"];
        //        if (obj != null)
        //        {
        //            return (string)obj;
        //        }
        //        return "";
        //    }
        //    set
        //    {
        //        this.ViewState["SunText"] = value;
        //    }
        //}

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
