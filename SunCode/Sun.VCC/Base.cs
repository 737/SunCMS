using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;
using Sun.SunControl;

namespace Sun.VCC
{
    public class Base : System.Web.UI.UserControl
    {
        public static string PageletQuery
        {
            get;
            set;
        }

        public void SetControl<T>(T model) where T : class,new()
        {
            if (model != null)
            {
                PropertyInfo[] InfoList = model.GetType().GetProperties();

                foreach (PropertyInfo info in InfoList)
                {
                    ISunControl control = this.FindControl(info.Name) as ISunControl;
                    if (control != null)
                    {
                        object t_value = info.GetValue(model, null);
                        if (t_value != null)
                        {
                            control.Value = t_value;
                        }
                    }
                }
            }
        }

    }
}
