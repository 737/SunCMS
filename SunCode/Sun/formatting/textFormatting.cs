using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Sun.formatting
{
    public class textFormatting
    {
        NameValueCollection nvc = new NameValueCollection();

        /// <summary>
        /// //存入要进行格式化的键/值
        /// </summary>
        public void Add(string name, string value)
        {
            this.nvc.Add(name, value);
        }

        /// <summary>
        /// //格式化(其实是替换)
        /// </summary>
        public string Format(string text)
        {
            if (nvc.Count > 0)
            {
                foreach (string key in this.nvc.AllKeys)
                {
                    string replaceMent = this.nvc[key];
                    if (replaceMent != null)
                    {
                        text = Regex.Replace(text, this.FormatName(key), replaceMent, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    }
                }
            }
            return text;
        }

        public static string format(object obj, string text)
        {
            if (obj == null)
            {
                return text;
            }
            PropertyInfo[] properties = obj.GetType().GetProperties();
            string html = text;
            string pattern = @"\{(?<value>[一-龥A-Za-z_]+?[^;]*?)\}";
            Regex reg = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            for (Match match = reg.Match(text); match.Success; match = match.NextMatch())
            {
                FormatField field = new FormatField(match.Groups["value"].Value);
                if (string.IsNullOrEmpty(field.name))
                {
                    html = html.Replace(match.Value, field.getValue("n"));
                }
                else
                {
                    object propertyValue = null;
                }
            }

            //TODO::  继续写
            return html;
        }

        /// <summary>
        /// //将名称加"[]"
        /// </summary>
        private string FormatName(string name)
        {
            return string.Format(@"\[{0}\]", name);
        }

    }
}
