using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Sun.HtmlEngine.Tags;

namespace Sun.HtmlEngine
{
    public class Template
    {
        string _text;
        object _data;

        public Template(string sTemplatePath, object oData)
        {
            _text = Sun.Toolkit.io.getTextFile(sTemplatePath);
            _data = oData;
        }


        public string render()
        {
            var html = _text;

            List<ITag> tagList = this.parseTags(html);

            foreach (var tag in tagList)
            {
                html = html.Replace(tag.expresstion, tag.render());
            }

            for (int i = 0; i < tagList.Count; i++)
            {
                tagList[i] = null;
            }
            tagList = null;

            return html;
        }

        private string parseGlobalTag(string sHtmlText)
        {

            return sHtmlText;
        }

        // 解析HtmlText中所有的 tag
        private List<ITag> parseTags(string sHtmlText)
        {
            if (string.IsNullOrEmpty(sHtmlText))
            {
                return null;
            }

            string _pattern = @"(<(?<namespace>[\w]+?):(?<tag>[\w]+)\s*(?<attribute>[^<]*?)/>)|(<(?<namespace>[\w]+):(?<tag>[\w]+)\s*(?<attribute>[^>]*)>(?<innertext>.*)(</\k<namespace>:\k<tag>>))";

            Regex reg = new Regex(_pattern, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            List<ITag> tagParsers = new List<ITag>();
            ITag tag = null;

            for (Match match = reg.Match(sHtmlText); match.Success; match = match.NextMatch())
            {
                string _namespace = match.Groups["namespace"].Value,
                        _tag = match.Groups["tag"].Value,
                        _attribute = match.Groups["attribute"].Value,
                        _innerHtml = match.Groups["innertext"].Value;


                TagCreater tagCreater = new TagCreater(_namespace + ":" + _tag);
                tag = tagCreater.createParser(_attribute, _innerHtml);

                if (tagCreater != null && tag != null)
                {
                    tag.expresstion = match.Value;

                    tagParsers.Add(tag);
                }
            }

            return tagParsers;

        }
    }
}
