using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sun.generating.tags
{
    public abstract class dataView : innerTag
    {
        public dataView() { }
        public dataView(string exp) : base(exp) { }
        public dataView(string exp, string innerHtml) : base(exp, innerHtml) { }


        public override string render()
        {
            base.attributes.setInnerHtml(base.__innerHtml);
            var html = string.Empty;

            try
            {
                object currentData = base.getCurrentData();
                if (currentData == null)
                {
                    return html;
                }

                var formatHtml = base.attributes.defaultAttribute;
                html = Sun.formatting.typeFormatting.format(currentData, formatHtml);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                currentData = null;
            }
            return html;
        }

        
    }
}
