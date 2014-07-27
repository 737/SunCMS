using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace sun.generating.tags
{
    public sealed class attributes : NameObjectCollectionBase, ICloneable, IList, ICollection, IEnumerable
    {
        public static readonly string _defaultAtrributeKey = "__defaultInnerHtmlProperty";
        internal static readonly Regex _outRegex = new Regex("\\s*(?<name>[^=]+?)\\s*=\\s*(?<child>['\"\"]?)(?<val>.+?)\\2\\s+", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
        internal static readonly Regex _subRegex = new Regex(@"<(?<name>[\w]+?)>(?<value>.*?)</\1>", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        /// <summary>
        /// // 如果tag内部没有写itemFormat 或者 alterNativeItemFormat的话，就将整个innterHtml作为此标签的值
        /// </summary>
        public string defaultAttribute
        {
            get
            {
                return this[_defaultAtrributeKey];
            }
        }

        internal attributes(string exp)
        {
            this.setOutsideHtml(exp);
        }

        /// <summary>
        /// // 设置外壳的html,比如： id=16 pagesize=20
        /// </summary>
        internal void setOutsideHtml(string outsideHtml)
        {
            if (!string.IsNullOrEmpty(outsideHtml))
            {
                outsideHtml = outsideHtml + " ";
                for (Match match = _outRegex.Match(outsideHtml); match.Success; match = match.NextMatch())
                {
                    string name = match.Groups["name"].Value;
                    string val = match.Groups["val"].Value;
                    this.add(name, val);
                    //Sun.diagnostics.log.recordError(Sun.Toolkit.JSON.GetJSON(new
                    //{
                    //    setOutsideHtml = "setOutsideHtml",
                    //    _name = name,
                    //    _value = val
                    //}));
                }
            }
        }

        /// <summary>
        /// // 设置tag内部的html
        /// </summary>
        internal void setInnerHtml(string innerHtml)
        {
            if (!string.IsNullOrEmpty(innerHtml))
            {
                // 每次都将innerHtml作为 _defaultAtrributeKey的值
                this.add(_defaultAtrributeKey, innerHtml);
                for (Match match = _subRegex.Match(innerHtml); match.Success; match = match.NextMatch())
                {
                    string name = match.Groups["name"].Value;
                    string value = match.Groups["value"].Value;

                    //Sun.diagnostics.log.recordError(Sun.Toolkit.JSON.GetJSON(new
                    //{
                    //    setInnerHtml = "setInnerHtml",
                    //    _name = name,
                    //    _value = value
                    //}));
                    this.add(name, value);
                }
            }
        }

        public int getValueToInt(string name)
        {
            return Sun.Toolkit.Parse.ToInt(this[name], -1);
        }
        public int getValueToInt(string name, int defaultValue)
        {
            return Sun.Toolkit.Parse.ToInt(this[name], defaultValue);
        }

        public string this[string name]
        {
            get
            {
                if (!string.IsNullOrEmpty(name))
                {
                    object obj = base.BaseGet(name.ToLower());
                    if (obj != null)
                    {
                        return obj.ToString();
                    }
                }
                return "";
            }
            set
            {
                this.add(name, value);
            }
        }

        public void add(string name, string value)
        {
            name = name.ToLower().Trim();
            if (this[name] != null)
            {
                base.BaseRemove(name);
            }
            base.BaseAdd((string)name.Clone(), value.Clone());
        }

        public int getValue(string name)
        {
            return Sun.Toolkit.Parse.ToInt(this[name], -1);
        }
        public int getValue(string name, int defaultValue)
        {
            return Sun.Toolkit.Parse.ToInt(this[name], defaultValue);
        }

        #region ICloneable的实现方法
        public object Clone()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IList的实现方法
        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public new bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public object this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region ICollection的实现方法
        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        int ICollection.Count
        {
            get { throw new NotImplementedException(); }
        }

        bool ICollection.IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        object ICollection.SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
