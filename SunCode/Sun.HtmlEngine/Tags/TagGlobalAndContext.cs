using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Sun.HtmlEngine.Tags
{
    public class TagGlobalAndContext
    {


        public TagGlobalAndContext() {
            this.dicts = new Dictionary<string, string>();
        }

        public void addPrefixAndData(string sPrefix, object oData) {
            string key = "";
            object val = "";
            bool isList = true;

            if (oData == null) {
                return;
            }

            var data = this.parseToIList(oData);

            if (data != null && data.Count > 0) {
                oData = data[0];
            } else {
                isList = false;
            }

            if (!isList) {
                PropertyInfo[] props = oData.GetType().GetProperties();

                foreach (var item in props) {
                    if (item.CanRead) {
                        key = string.Format("TAG.{0}.{1}", sPrefix.ToUpper().Trim(), item.Name.ToUpper().Trim());
                        val = item.GetValue(oData, null);

                        if (val == null) {
                            val = "";
                        }

                        if (this.dicts.ContainsKey(key)) {
                            this.dicts[key] = val.ToString();
                        } else {
                            this.dicts.Add(key, val.ToString());
                        }
                    }
                }
            }
        }

        public string render(string sTxt, string sPrefix) {
            // 匹配 [@s:subject]    [@c:subject? - ]   [ @s : subject ? - : = ]
            var pattern = string.Format("{0}|{1}|{2}",
                @"(\[\s*\@(?<key>[A-Za-z]*?)\s*:\s*(?<value>[A-Za-z]*?)\s*\])",
                @"(\[\s*\@(?<key>[A-Za-z]*?)\s*:\s*(?<value>[A-Za-z]*?)\s*\?{1}\s*(?<true>[^:\]]*?)\])",
                @"(\[\s*\@(?<key>[A-Za-z]*?)\s*:\s*(?<value>[A-Za-z]*?)\s*\?{1}\s*(?<true>[^:\]]*?)\s:{1}\s(?<false>[^:\]]*?)\])");

            //匹配 [sun:context.subject]    [sun:context.subject? - ]   [sun:context.subject? - : = ]  
            //string pattern = @"(\[(?<value>sun:" + sPrefix + @"[A-Za-z_:.]+?[^:\]?])*?\])|(\[(?<value>sun:" + sPrefix + @"[A-Za-z_:.]+?[^?])\?{1}(?<true>[^:\]]*?)\])|(\[(?<value>sun:" + sPrefix + @"[A-Za-z_:.]+?[^?])\?{1}(?<true>[^\]]*?):{1}(?<false>[^:\]]*?)\])";

            if (string.IsNullOrEmpty(sPrefix)) {
                return sTxt;
            }

            Regex reg = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            var key = "";
            var _true = "";
            var _false = "";
            var newValue = "";
            var value = "";


            for (Match match = reg.Match(sTxt); match.Success; match = match.NextMatch()) {
                key = match.Groups["key"].Value;
                value = match.Groups["value"].Value;
                _true = match.Groups["true"].Value;
                _false = match.Groups["false"].Value;
                newValue = "";
                value = this.getValue(key, value);

                if (!string.IsNullOrEmpty(_false) && string.IsNullOrEmpty(value)) {
                    newValue = string.Format("{0}", _false);
                } else if (!string.IsNullOrEmpty(value)) {
                    newValue = string.Format("{0}{1}", value, _true);
                }

                sTxt = sTxt.Replace(match.Value, newValue);
            }

            return sTxt;
        }

        public string getValue(string sPrefix, string sValue) {
            var val = "";

            var key = string.Format("TAG.{0}.{1}", sPrefix.ToUpper().Trim(), sValue.ToUpper().Trim());

            if (!string.IsNullOrEmpty(sPrefix) && dicts.ContainsKey(key)) {
                val = dicts[key];
            }

            return val;
        }

        Dictionary<string, string> dicts = null;

        private IList parseToIList(object oData) {
            Type type = oData.GetType();

            if (type.GetInterface("IList") != null) {
                return (IList)oData;
            }

            return null;
        }




    }
}
