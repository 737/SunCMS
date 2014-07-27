using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using Sun;
using System.Reflection;

namespace Sun.SunControl
{
    public class SunRepeater : Repeater
    {
        protected override void OnItemDataBound(RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                PropertyInfo[] InfoList = e.Item.DataItem.GetType().GetProperties();

                foreach (PropertyInfo info in InfoList)
                {
                    ISunControl control = e.Item.FindControl(info.Name) as ISunControl;

                    if (control != null)
                    {
                        control.Value = info.GetValue(e.Item.DataItem, null);
                    }
                }
            }

            if (e.Item.ItemType == ListItemType.Footer)
            {
                ISunControl total =  e.Item.FindControl("totalAmount") as ISunControl;

                if (total != null)
                {
                    total.Value = 123123;
                }
            }


            //((Literal)rep2.Controls[rep2.Controls.Count - 1].FindControl("rep2Sum")).Text = ss.ToString(); 

        }

        public override object DataSource
        {
            get
            {
                return base.DataSource;
            }
            set
            {
                base.DataSource = value;
            }
        }


    }
}
