﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sun.HtmlEngine.Tags;

namespace Sun.HtmlEngine
{
    public class Template : TemplateParse
    {
        string _text;
        object _data;

        string text
        {
            get
            {
                return _text;
            }
        }

        object data
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
