using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sun.HtmlEngine.Tags
{
    public abstract class Parse
    {

        private string parseGlobalTag(string sHtmlText)
        {

            return sHtmlText;
        }

        // 解析HtmlText中所有的 tag
        private List<ITagParse> parseTags(string sHtmlText)
        {
            if (string.IsNullOrEmpty(sHtmlText))
            {
                return null;
            }

            string _pattern = @"(<(?<namespace>[\w]+?):(?<tag>[\w]+)\s*(?<attribute>[^<]*?)/>)|(<(?<namespace>[\w]+):(?<tag>[\w]+)\s*(?<attribute>[^>]*)>(?<innertext>.*)(</\k<namespace>:\k<tag>>))";

            Regex reg = new Regex(_pattern, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            List<ITagParse> tagParsers = new List<ITagParse>();
            ITagParse parser = null;

            for (Match match = reg.Match(sHtmlText); match.Success; match = match.NextMatch())
            {
                string _namespace = match.Groups["namespace"].Value,
                        _tag = match.Groups["tag"].Value,
                        _attribute = match.Groups["attribute"].Value,
                        _innerHtml = match.Groups["innertext"].Value;


                TagCreater tagCreater = new TagCreater(_namespace + ":" + _tag);
                parser = tagCreater.createParser(_attribute, _innerHtml);
                parser.expresstion = match.Value;

                if (tagCreater != null)
                {
                    tagParsers.Add(parser);
                }
            }

            return tagParsers;

        }

        protected string parseHtml(string sHtmlText)
        {
            List<ITagParse> tagParsers = this.parseTags(sHtmlText);

            var html = "";
            
            foreach (var parser in tagParsers)
            {
                html = sHtmlText.Replace(parser.expresstion, parser.render());
            }

            for (int i = 0; i < tagParsers.Count; i++)
            {
                tagParsers[i] = null;
            }
            tagParsers = null;

            return html;
        }


    }
}
