using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sun.HtmlEngine.Tags
{
    public class Attributes : NameValueCollection
    {
        string __exPresstion = "";
        // 标签属性正则
        Regex regAttr = new Regex(@"\s*(?<name>[^=]+?)\s*=\s*(?<sign>[\'""]?)(?<value>.+?)\2", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);

        public Attributes(string expresstion)
        {
            __exPresstion = expresstion;

            parseAttr(__exPresstion);
        }
        
        internal void parseAttr(string sExpresstion)
        {
            string name = "";
            string value = "";

            if (!string.IsNullOrEmpty(sExpresstion))
            {
                for (Match match = regAttr.Match(sExpresstion); match.Success; match = match.NextMatch())
                {
                    name = match.Groups["name"].Value.Trim().ToLower();
                    value = match.Groups["value"].Value.Trim();

                    if (this[name] != null)
                    {
                        this.Remove(name);
                    }

                    this.Add(name, value);
                }

            }
        }

        public string expresstion
        {
            get
            {
                return __exPresstion;
            }
        }
    }
}
