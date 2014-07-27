using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;
using System.ComponentModel;

namespace sun.generating.tags
{
    public abstract class dataList : innerTag
    {
        public dataList() { }
        public dataList(string exp) : base(exp) { }
        public dataList(string exp, string innerHtml) : base(exp, innerHtml) { }

        private string __itemFormat;
        public string itemFormat
        {
            get
            {
                if (string.IsNullOrEmpty(this.__itemFormat))
                {
                    this.__itemFormat = base.attributes["itemFormat"];
                }
                if (string.IsNullOrEmpty(this.__itemFormat))
                {
                    this.__itemFormat = base.attributes.defaultAttribute;
                }
                if (this.__itemFormat != null)
                {
                    return this.__itemFormat;
                }
                return "";
            }
            set
            {
                this.__itemFormat = value;
            }
        }
        private string __alternatingItemFormat;
        public string alternatingItemFormat
        {
            get
            {
                if (this.__alternatingItemFormat == null)
                {
                    this.__alternatingItemFormat = base.attributes["alternatingItemFormat"];
                }
                if (this.__alternatingItemFormat != null)
                {
                    return this.__alternatingItemFormat;
                }
                return "";
            }
            set
            {
                this.__alternatingItemFormat = value;
            }
        }
        public string __endItemFormat;
        public string endItemFormat
        {
            get
            {
                if (this.__endItemFormat == null)
                {
                    this.__endItemFormat = base.attributes["endItemformat"];
                }
                if (this.__endItemFormat != null)
                {
                    return this.__endItemFormat;
                }
                return "";
            }
            set
            {
                this.__endItemFormat = value;
            }
        }

        public string separater
        {
            get
            {
                return base.attributes["separater"];
            }
        }

        private IList parseToIList(object obj)
        {

            Type type = obj.GetType();
            if (type.GetInterface("IList") != null)
            {
                return (IList)obj;
            }

            return null;
        }

        #region 实现iInterpreter的render方法
        /// <summary>
        /// // render出来的内容将替换整个标签的内容
        /// </summary>
        public override string render()
        {
            base.__attributes.setInnerHtml(base.__innerHtml);
            //int baseIndex = base.attributes.getValueToInt("baseIndex", 0) + 1;
            var html = string.Empty;
            try
            {
                //TODO:: 大概整理了一下模板引擎实现的思路，就是这个几类的 迭代调用，   下面的currentData已经拿到数了。
                object currentData = base.getCurrentData();

                if (currentData == null)
                {
                    return "";
                }

                IList source = this.parseToIList(currentData);
                if ((source != null) && (source.Count > 0))
                {
                    int max = source.Count;
                    for (int i = 0; i < max; i++)
                    {
                        var itemData = source[i];
                        string itemHtml = this.itemFormat;

                        //分部写 start
                        var tag = base.createTag(itemData, null, itemHtml);
                        itemHtml = tag.render();
                        //替换[@rowIndex]=>代表的是当前是第几行
                        Regex _otherTag = new Regex(@"\[@rowIndex\]", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
                        itemHtml = _otherTag.Replace(itemHtml, (i + 1).ToString());
                        //分部写 end

                        html = html + itemHtml;
                    }
                }
                else
                {
                    html = html + Sun.formatting.typeFormatting.format(currentData, this.itemFormat);
                }
                currentData = null;
            }
            catch (Exception ex)
            {
                return "数据错误__dataList.css__render()" + this.GetType().FullName + ex.Message;
            }
            return html;
        }
        #endregion
    }
}
