using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sun.HtmlEngine.Tags
{
    public abstract class ParseList : ITag
    {
        string innerHtml;
        Attributes _attributes = null;

        public ParseList(string sAttributes, string sInnerHtml)
        {
            _attributes = new Attributes(sAttributes);
            this.innerHtml = sInnerHtml;
        }

        protected abstract IList getCurrentData();

        public string render()
        {
            var html = string.Empty;
            Regex _otherTag = new Regex(@"\[@rowIndex\]", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            IList datalist = this.getCurrentData();

            if ((datalist != null) && (datalist.Count > 0))
            {
                int max = datalist.Count;
                var _itemHtml = "";
                for (int i = 0; i < max; i++)
                {
                    _itemHtml = Sun.HtmlEngine.Format.FormatField.format(datalist[i], this.innerHtml);
                    _itemHtml = _otherTag.Replace(_itemHtml, (i + 1).ToString());

                    html = html + _itemHtml;
                }

            }

            return html;
        }

        public Attributes attributes
        {
            get { return _attributes; }
        }

        public string expresstion
        {
            set;
            get;
        }


    }
}
