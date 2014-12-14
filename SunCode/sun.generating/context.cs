using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace sun.generating
{
    public static class context
    {
        private static Template meTempalte;

        private static string getValueByKey(Template tmp, string key)
        {
            string txt = "";

            if (tmp.currentData != null)
            {
                Type t = tmp.currentData.GetType();
                PropertyInfo propInfo = t.GetProperty(key, BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (propInfo != null)
                {
                    txt = propInfo.GetValue(tmp.currentData, null).ToString();
                }
            }

            return txt;
        }

        public static string regexReplace(Match match)
        {
            string key = match.Groups["def"].Value;

            if (meTempalte != null)
            {
                return getValueByKey(meTempalte, key);
            }

            return "___tagpasdfasdfrefix";
        }

        public static string replaceForPrefix(string prefix, string text, Template tmp)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }
            string pattern = @"\{" + prefix + @"(?<def>[^\}@#$&*!%]+)\}";
            meTempalte = tmp;
            //Sun.diagnostics.log.recordError(pattern);
            //Sun.diagnostics.log.recordError(text);

            return Regex.Replace(text, pattern, new MatchEvaluator(context.regexReplace), RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
        }
    }
}
