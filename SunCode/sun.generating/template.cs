using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sun.generating.tags;

namespace sun.generating
{
    public class template : parser
    {


        public template(string templetePath)
        {
            try
            {
                base.htmlText = Sun.Toolkit.io.getTextFile(templetePath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override string render()
        {
            var html = "";
            //this.outputText = context.replaceForPrefix("@", this.outputText, this);
            html = base.renderChildren(base.htmlText);

            return html;
        }

        //public override string renderContent()
        //{

        //    return this.outputText;
        //}



    }
}
