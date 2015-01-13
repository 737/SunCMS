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
        TagGlobalAndContext globalContext;
        
        public Template(string sTemplatePath, object oData) {
            globalContext = new TagGlobalAndContext();

            _text = Sun.Toolkit.io.getTextFile(sTemplatePath);
            _data = oData;
        }

        public string render() {
            // 系统数据
            globalContext.addPrefixAndData("s", Sun.configSystem.getConfig());
            // 当前环境数据
            globalContext.addPrefixAndData("c", _data);

            return this.iteratorRender(_text);
        }
        
        // 迭代处理所有标签
        // include标签中可能有标签
        private string iteratorRender(string sHtmlText) {
            List<ITag> tagList = parseTags(sHtmlText);

            if (tagList.Count > 0) {
                foreach (var tag in tagList) {
                    sHtmlText = sHtmlText.Replace(tag.expresstion, tag.render());

                    sHtmlText = parseGlobalAndContextTag(tag.data, sHtmlText);
                }

                // 递归调用，直到处理所有标签为止
                sHtmlText = this.iteratorRender(sHtmlText);
            }

            for (int i = 0; i < tagList.Count; i++) {
                tagList[i] = null;
            }
            tagList = null;

            return sHtmlText;
        }

        private string parseGlobalAndContextTag(object data, string sHtmlText) {
            sHtmlText = globalContext.render(sHtmlText, "s");
            sHtmlText = globalContext.render(sHtmlText, "c");

            return sHtmlText;
        }

        // 解析HtmlText中所有的 tag
        private List<ITag> parseTags(string sHtmlText) {
            if (string.IsNullOrEmpty(sHtmlText)) {
                return null;
            }

            string _pattern = @"(<(?<namespace>[\w]+?):(?<tag>[\w]+)\s*(?<attribute>[^<]*?)/>)|(<(?<namespace>[\w]+):(?<tag>[\w]+)\s*(?<attribute>[^>]*)>(?<innertext>.*?)(</\k<namespace>:\k<tag>>))";

            Regex reg = new Regex(_pattern, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            List<ITag> tagParsers = new List<ITag>();
            ITag tag = null;

            for (Match match = reg.Match(sHtmlText); match.Success; match = match.NextMatch()) {
                string _namespace = match.Groups["namespace"].Value,
                        _tag = match.Groups["tag"].Value,
                        _attribute = match.Groups["attribute"].Value,
                        _innerHtml = match.Groups["innertext"].Value;


                TagCreater tagCreater = new TagCreater(_namespace + ":" + _tag);
                tag = tagCreater.createParser(_attribute, _innerHtml);

                if (tagCreater != null && tag != null) {
                    tag.expresstion = match.Value;

                    tagParsers.Add(tag);
                }
            }

            return tagParsers;

        }
    }
}
