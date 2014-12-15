using System;
using System.Collections;
using System.Linq;
using System.Text;
using sun.generating.tags;

namespace sun.generating.Tag
{
    /// <summary>
    /// // List 标签解析
    /// </summary>
    public abstract class MakeList : ITagInterpreter
    {
        private IList parseToIList(object obj)
        {

            Type type = obj.GetType();
            if (type.GetInterface("IList") != null)
            {
                return (IList)obj;
            }

            return null;
        }

        protected Attributes attributes
        {
            set;
            get;
        }

        protected string innerHtml
        {
            set;
            get;
        }

        protected abstract object getCurrentData();

        public MakeList(string attributes, string innerHtml)
        {
            this.attributes = new Attributes(attributes);
            this.innerHtml = innerHtml;
        }

        public string expression
        {
            set;
            get;
        }

        public object data
        {
            set;
            get;
        }

        public Parser context
        {
            set;
            get;
        }

        public void addAttribute(string name, string value)
        {
            throw new NotImplementedException();
        }

        public string render()
        {
            var html = string.Empty;
            var currentData = this.getCurrentData();

            if (currentData == null)
            {
                return "";
            }


            this.attributes.setInnerHtml(this.innerHtml);

            IList dataList = this.parseToIList(currentData);
            if ((dataList != null) && (dataList.Count > 0))
            {
                int max = dataList.Count;

                for (int i = 0; i < max; i++)
                {
                    var item = dataList[i];
                    var itemHtml = "我是itemhtml";

                    html = html + itemHtml;
                }
            }

            currentData = null;

            return html;
        }

    }


}
