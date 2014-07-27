using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sun.generating;
using sun.generating.tags;

namespace sun.tags
{
    public class include : tag
    {
        public include() { }

        public include(string exp) : base(exp) { }

        public include(string exp, string html) : base(exp, html) { }

        public override string renderContent()
        {

            var html = "";
            var src = base.attributes["src"];

            src = templateHelper.getTemplatePath(src);

            html = Sun.Toolkit.io.getTextFile(src);

            return html;
        }

    }
}
