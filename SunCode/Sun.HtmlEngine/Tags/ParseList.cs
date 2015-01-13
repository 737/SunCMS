using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sun.HtmlEngine.Tags
{
    public abstract class ParseList : Parse
    {
        public ParseList(string sAttributes, string sInnerHtml) : base(sAttributes, sInnerHtml) { }

        protected abstract IList getCurrentData();

        public override string render() {
            var html = string.Empty;
            Regex _otherTag = new Regex(@"\[@rowIndex\]", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            IList datalist = this.getCurrentData();

            if ((datalist != null) && (datalist.Count > 0)) {
                var _itemHtml = "";

                for (int i = 0; i < datalist.Count; i++) {
                    _itemHtml = Sun.HtmlEngine.Format.FormatField.format(datalist[i], base.tagInnerHTML);
                    _itemHtml = _otherTag.Replace(_itemHtml, (i + 1).ToString());

                    html = html + _itemHtml;
                }

            }

            return html;
        }
    }
}
