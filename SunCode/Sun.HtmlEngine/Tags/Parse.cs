using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sun.HtmlEngine.Tags
{
    public abstract class Parse : ITag
    {
        public Attributes attributes { set; get; }

        public string expresstion { set; get; }

        // TAG内部的HTML
        public string tagInnerHTML { set; get; }

        public Parse(string sAttributes, string sInnerHtml) {
            attributes = new Attributes(sAttributes);
            tagInnerHTML = sInnerHtml;
        }

        public abstract string render();


        public object data {
            get {
                return null;
            }
            set {
                throw new NotImplementedException();
            }
        }
    }
}
