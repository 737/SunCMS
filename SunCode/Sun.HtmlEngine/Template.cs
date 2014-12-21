using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sun.HtmlEngine.Tags;

namespace Sun.HtmlEngine
{
    public class Template : TemplateParse
    {
        public string _text;

        public object _data;

        public string text
        {
            get
            {
                return _text;
            }
        }

        public object data
        {
            get
            {
                return _data;
            }
        }

        public Template(string sTemplatePath, object oData)
        {
            _text = Sun.Toolkit.io.getTextFile(sTemplatePath);
            _data = oData;
        }


        public string render()
        {
            var html = "";

            html = base.parseHtml(_text);

            return html;
        }
    }
}
