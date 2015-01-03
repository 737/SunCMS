using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sun.HtmlEngine.Format
{
    /// <summary>
    /// // 格式化字段
    /// </summary>
    public class Filed
    {
        public string name {
            set;
            get;
        }

        public NameValueCollection parameters {
            get {
                return __parameters;
            }
        }

        public bool isHasParameters {
            get {
                return (this.parameters.Count > 0);
            }
        }

        public Filed(string exp) {
            string[] strArray = exp.Split(new char[] { ',' });
            if ((strArray.Length > 0) && (strArray[0]) != null) {
                this.name = strArray[0];
            }
            if ((strArray.Length > 1) && (strArray[1] != null)) {
                this.initParameters(strArray[1]);
            }
        }

        public string getValue(string name) {
            string result = null;

            if (!this.isHasParameters) {
                return "";
            }

            result = this.parameters[name];

            if (string.IsNullOrEmpty(result)) {
                result = result.ToString();
            }

            return result;
        }


        NameValueCollection __parameters = new NameValueCollection();

        private string escape(string str) {
            if (!string.IsNullOrEmpty(str)) {
                return Regex.Replace(str, @"\[(\d*)\]", "{$1}");
            }
            return "";
        }

        private void initParameters(string exp) {
            if (!string.IsNullOrEmpty(exp)) {
                Regex _expRegex = new Regex(@"\s*(?<name>[^=]+?)\s*=\s*(?<symbol>['""]?)(?<value>.+?)\2\s*", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
                string name = "";
                string value = "";

                exp = exp + " ";

                for (Match match = _expRegex.Match(exp); match.Success; match = match.NextMatch()) {
                    name = match.Groups["name"].Value;
                    value = match.Groups["value"].Value;

                    value = this.escape(value);
                    __parameters.Add(name, value);
                }
            }
        }

    }
}
