using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sun.HtmlEngine.Tags;

namespace Sun.HtmlEngine
{
    public abstract class TemplateParse
    {

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
            ITag parser = null;

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
            List<ITag> tagList = this.parseTags(sHtmlText);

            var html = "";
            
            foreach (var tag in tagList)
            {
                html = sHtmlText.Replace(tag.expresstion, tag.render());
            }

            for (int i = 0; i < tagList.Count; i++)
            {
                tagList[i] = null;
            }
            tagList = null;

            return html;
        }


    }
}
