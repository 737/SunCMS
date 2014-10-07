using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections;

namespace sun.generating
{
    /// <summary>
    /// // 格式化额外的字段，全已有字段或上下文字段
    /// // 格式化 sun:context 和 sun:global属性
    /// </summary>
    public class formatExternal
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

        public formatExternal()
        {
            this.__dicts = new Dictionary<string, string>();
        }

        private Dictionary<string, string> __dicts;
        public Dictionary<string, string> dicts
        {
            get
            {
                if (__dicts == null)
                {
                    __dicts = new Dictionary<string, string>();
                }
                return __dicts;
            }
        }

        /// <summary>
        /// // 增加对应的字段和值
        /// </summary>
        /// <param name="prefix">字段</param>
        /// <param name="obj">值</param>
        public void add(string prefix, object obj)
        {
            string key = "";
            object val = "";
            bool flag = true;

            if (obj == null)
            {
                return;
            }
            if (this.parseToIList(obj) != null)
            {
                if (this.parseToIList(obj).Count > 0)
                {
                    obj = this.parseToIList(obj)[0];
                }
                else
                {
                    flag = false;
                }
            }
            if (flag == true)
            {
                PropertyInfo[] props = obj.GetType().GetProperties();
                foreach (var item in props)
                {
                    if (item.CanRead)
                    {
                        key = string.Format("sun:{0}.{1}", prefix.ToLower().Trim(), item.Name.ToLower().Trim());
                        val = item.GetValue(obj, null);
                        if (val == null)
                        {
                            val = "";
                        }
                        if (this.__dicts.ContainsKey(key))
                        {
                            this.__dicts[key] = val.ToString();
                        }
                        else
                        {
                            this.__dicts.Add(key, val.ToString());
                        }
                    }
                }
            }
        }

        public string getValue(string key)
        {
            var val = "";

            key = key.ToLower().Trim();
            if (!string.IsNullOrEmpty(key) && this.__dicts.ContainsKey(key))
            {
                val = __dicts[key];
            }

            return val;
        }

        public string format(string html)
        {
            string prefix = string.Empty;

            return this.format(html, prefix);
        }
        public string format(string html, string prefix)
        {
            //匹配 [sun:context.subject]    [sun:context.subject? - ]   [sun:context.subject? - : = ]  
            string pattern = @"(\[(?<value>sun:" + prefix + @"[A-Za-z_:.]+?[^:\]?])*?\])|(\[(?<value>sun:" + prefix + @"[A-Za-z_:.]+?[^?])\?{1}(?<true>[^:\]]*?)\])|(\[(?<value>sun:" + prefix + @"[A-Za-z_:.]+?[^?])\?{1}(?<true>[^\]]*?):{1}(?<false>[^:\]]*?)\])";
            string key = "",
                _true = "",
                _false = "",
                newValue = "",
                value = "";

            if (string.IsNullOrEmpty(html))
            {
                return html;
            }

            Regex reg = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            for (Match match = reg.Match(html); match.Success; match = match.NextMatch())
            {
                key = match.Groups["value"].Value;
                _true = match.Groups["true"].Value;
                _false = match.Groups["false"].Value;
                newValue = "";
                value = this.getValue(key);

                if ((!string.IsNullOrEmpty(_false)) && (string.IsNullOrEmpty(value)))
                {
                    newValue = string.Format("{0}", _false);
                }
                else if (!string.IsNullOrEmpty(value))
                {
                    newValue = string.Format("{0}{1}", value, _true);
                }

                html = html.Replace(match.Value, newValue);
            }

            return html;
        }
    }
}
