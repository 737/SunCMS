using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace sun.generating.tags
{
    public abstract class parser
    {
        
        protected string htmlText;

        private string __expression;
        /// <summary>
        /// // 标签内的内容
        /// </summary>
        protected string __innerHtml;

        protected attributes __attributes;
        public attributes attributes
        {
            get
            {
                return this.__attributes;
            }
        }

        protected object __currentData;
        public virtual object currentData
        {
            get
            {
                return this.__currentData;
            }
            set
            {
                this.__currentData = value;
            }
        }

        public parser()
        {
            this.__expression = "";
            this.__attributes = new attributes("");
        }
        public parser(string expression)
        {
            this.__expression = "";
            this.__attributes = new attributes(expression);
        }
        public parser(string attributes, string innerHtml)
        {
            this.__expression = "";
            this.__attributes = new attributes(attributes);
            this.__innerHtml = innerHtml;
        }

        /// <summary>
        /// // 输出
        /// </summary>
        public virtual string render()
        {   
            if (!string.IsNullOrEmpty(this.__innerHtml))
            {
                return this.__innerHtml;
            }
            return this.renderContent();
        }

        /// <summary>
        /// // 输出内容
        /// </summary>
        public virtual string renderContent()
        {
            return "<span style='dispaly: none'>suncms:提示对应标签没有内容</span>";
        }

        /// <summary>
        /// // 解析页面的 标签
        /// </summary>
        public virtual string renderChildren(string html)
        {
            string _pattern = @"(<(?<namespace>[\w]+?):(?<tag>[\w]+)\s*(?<attribute>[^<]*?)/>)|(<(?<namespace>[\w]+?):(?<tag>[\w]+)\s*(?<attribute>[^>]*)>(?<innertext>((?<Nested><\k<namespace>:(\k<tag>>|\k<tag>\s+[^>]*>))|</\k<namespace>:\k<tag>>(?<-Nested>)|.*?)*)(</\k<namespace>:\k<tag>>|\z))";
            formatExternal fmExternal = new formatExternal();

            if (!string.IsNullOrEmpty(html))
            {
                Regex reg = new Regex(_pattern, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
                List<iInterpreter> interpreterList = new List<iInterpreter>();

                fmExternal.add("global", Sun.configSystem.getConfig());
                html = fmExternal.format(html, "global");
                if (this.currentData != null)
                {
                    fmExternal.add("menu", this.currentData);
                    html = fmExternal.format(html, "menu");
                }

                for (Match match = reg.Match(html); match.Success; match = match.NextMatch())
                {

                    string _namespace = match.Groups["namespace"].Value;
                    string _tag = match.Groups["tag"].Value;
                    string attribute = match.Groups["attribute"].Value;
                    string _innerHtml = match.Groups["innertext"].Value;
                    //attribute = this.renderAttributes(attribute);

                    iInterpreter interpreter = null;
                    try
                    {
                        tagCreater creater = new tagCreater(_namespace + ":" + _tag);
                        interpreter = creater.createInterpreter(attribute, _innerHtml);
                        interpreter.expression = match.Value;
                        interpreter.context = this;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    interpreterList.Add(interpreter);
                }
                foreach (iInterpreter interpreterItem in interpreterList)
                {
                    html = html.Replace(interpreterItem.expression, interpreterItem.render());

                    //TODO::: 替换全局标签
                    var __data = interpreterItem.data;
                    fmExternal.add(interpreterItem.GetType().Name, __data);
                }
                for (int i = 0; i < interpreterList.Count; i++)
                {
                    interpreterList[i] = null;
                }
                interpreterList = null;
                // 解析 include 文件
                html = fmExternal.format(html);
            }

            return html;
        }


        //public virtual string renderAttributes(string attributes)
        //{
        //    if ((!string.IsNullOrEmpty(attributes)) && (this.currentData != null))
        //    {
        //        Sun.formatting.typeFormatting.format(this.currentData, attributes);
        //    }
        //    return attributes;
        //}
    }
}
