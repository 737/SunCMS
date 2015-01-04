using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sun.HtmlEngine.Tags
{
    public abstract class ParseConent : Parse
    {
        public ParseConent(string sAttributes, string sInnerHtml) : base(sAttributes, sInnerHtml) { }

        public override string render() {
            return this.renderContent();
        }

        // 得到html字段
        public abstract string renderContent();
    }
}
