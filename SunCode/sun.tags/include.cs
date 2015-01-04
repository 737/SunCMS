using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sun.HtmlEngine;
using Sun.HtmlEngine.Tags;


namespace sun.tags
{
    public class Include : ParseConent
    {
        public Include(string sAttributes, string sInnerHtml) : base(sAttributes, sInnerHtml) { }

        public override string renderContent() {

            var html = "";
            var src = base.attributes["src"];

            src = templateHelper.getTemplatePath(src);

            html = Sun.Toolkit.io.getTextFile(src);

            return html;
        }
    }
}
