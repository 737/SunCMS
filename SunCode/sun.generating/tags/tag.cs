using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sun.generating.tags
{
    public abstract class tag : Parser, ITagInterpreter
    {
        public tag() { }
        public tag(string exp) : base(exp) { }
        public tag(string exp, string html) : base(exp, html) { }

        #region 实现iInterpreter，除了render方法
        private string __exp;
        public string expression
        {
            get
            {
                return this.__exp;
            }
            set
            {
                this.__exp = value;
            }
        }

        private Parser __context;
        public Parser context
        {
            get
            {
                if (this.__context == null)
                {

                }
                return this.__context;
            }
            set
            {
                this.__context = value as Parser;
            }
        }

        public object data
        {
            get;
            set;
        }

        public void addAttribute(string name, string value)
        {
            throw new NotImplementedException();
        }
        #endregion

        public virtual string noneText
        {
            get
            {
                string str = base.attributes["none"];
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                }
                return "";
            }
        }

        /// <summary>
        /// // 开始标签
        /// </summary>
        public virtual string beginTag
        {
            get
            {
                string str = base.attributes["beginTag"];
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                }
                return "";
            }
        }
        public virtual string endTag
        {
            get
            {
                string str = base.attributes["endTag"];
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                }
                return "";
            }
        }

    }
}
