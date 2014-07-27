using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sun.formatting
{
    public class typeFormatField
    {
        private static readonly Regex _expRegex = new Regex("\\s*(?<name>[^=]+?)\\s*=\\s*(?<child>['\"\"]?)(?<val>.+?)\\2\\s+", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        private string escape(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return Regex.Replace(str, @"\[(\d*)\]", "{$1}");
            }
            return "";
        }
        private void initParameters(string exp)
        {
            if (!string.IsNullOrEmpty(exp))
            {
                exp = exp + " ";
                for (Match match = _expRegex.Match(exp); match.Success; match.NextMatch())
                {
                    string name = match.Groups["name"].Value;
                    string value = match.Groups["value"].Value;

                    value = this.escape(value);
                    this.__parameters.Add(name, value);
                }
            }
        }

        private NameValueCollection __parameters = new NameValueCollection();
        public NameValueCollection parameters
        {
            get
            {
                return this.__parameters;
            }
        }
        private string __name = "";
        public string name
        {
            get
            {
                return this.__name;
            }
            set
            {
                this.__name = value;
            }
        }

        public typeFormatField(string exp)
        {
            string[] strArray = exp.Split(new char[] { ',' });
            if ((strArray.Length > 0) && (strArray[0]) != null)
            {
                this.__name = strArray[0];
            }
            if ((strArray.Length > 1) && (strArray[1] != null))
            {
                this.initParameters(strArray[1]);
            }
        }

        public bool isHasParameters
        {
            get
            {
                return (this.parameters.Count > 0);
            }
        }

        public string getValue(string name)
        {
            if (!this.isHasParameters)
            {
                return "";
            }
            return this.parameters[name].ToString();
        }

        



    }
}
