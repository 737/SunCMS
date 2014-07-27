using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace sun.generating.tags
{
    public abstract class innerTag : tag
    {

        protected iObjectProvider __objectProvider;
        public iObjectProvider objectProvider
        {
            get
            {
                if (this.__objectProvider == null)
                {
                    this.__objectProvider = new objectInfo(base.__attributes);
                }
                return this.__objectProvider;
            }
            set
            {
                this.__objectProvider = value;
            }
        }

        public innerTag() { }
        public innerTag(string exp) : base(exp) { }
        public innerTag(string exp, string innerHtml) : base(exp, innerHtml) { }

        public tag createTag(object obj, string sAttributes, string html)
        {
            attributes attributes = new attributes(sAttributes);
            string str = attributes["tag"];
            tagCreater creater = null;
            if (!string.IsNullOrEmpty(str))
            {
                creater = new tagCreater(str);
            }
            else
            {
                creater = new tagCreater(this.getTagTypeForChildObject(obj));
            }
            if (string.IsNullOrEmpty(html))
            {
                html = this.getDefaultChildFormat();
            }
            tag tag = creater.createInterpreter(sAttributes, html) as tag;
            tag.currentData = obj;
            tag.context = this;
            return tag;
        }

        protected virtual string getDefaultChildFormat()
        {
            return base.__innerHtml;
        }

        protected abstract object getProvider();

        public virtual Type getTagTypeForChildObject(object obj)
        {
            return this.GetType();
        }

        public object getCurrentData()
        {
            if (base.currentData == null)
            {
                base.data = base.currentData = this.getProvider();
            }
            return base.currentData;
        }
    }
}
