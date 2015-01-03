using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Sun.formatting
{
    public static class typeFormatting
    {
        public static string[] htmlTags = new string[] { 
            "DOCTYPE", "HTML", "HEAD", "META", "TITLE", "BODY", "H1", "H6", "P", "FONT", "BASEFONT", "B", "I", "U", "BIG", "SMALL", 
            "S", "STRIKE", "TT", "SUB", "SUP", "RB", "RP", "RT", "RUBY", "MARQUEE", "BLINK", "BDO", "BR", "NOBR", "WBR", "EM", 
            "STRONG", "DFN", "DEL", "INS", "ADDRESS", "BLOCKQUOTE", "Q", "CITE", "CODE", "VAR", "SAMP", "KBD", "ABBR", "ACRONYM", "A", "MAP", 
            "AREA", "BASE", "LINK", "BUTTON", "IMG", "EMBED", "NOEMBED", "OBJECT", "APPLET", "PARAM", " APPLET", "EMBED ", " OBJECT ", "BGSOUND", "TABLE", "TR", 
            "TH", "TD", "THEAD", "TFOOT", "TBODY", "CAPTION", "COL", "COLGROUP", "CENTER", "SPACER", "MULTICOL", "HR", "FRAMESET", "FRAME", "NOFRAMES", "IFRAME", 
            "LAYER", "ILAYER", "NOLAYER", "FORM", "INPUT", "SELECT", "OPTGROUP", "OPTION", "SELECT ", "TEXTAREA", "LEGEND", "FIELDSET", "LABEL", "ISINDEX", "OL", "UL", 
            "LI", "DL", "DT", "DD", "DIR", "MENU", "SCRIPT", "NOSCRIPT", "STYLE", "DIV", "SPAN", "PRE", "PLAINTEXT", "XMP", "LISTING"
         };

        public static string format(object fillEntity, string tagInnerHTML)
        {
            if (fillEntity == null)
            {
                return tagInnerHTML;
            }

            // 取出所有字段的正则表达式
            string pattern = @"\{(?<field>[一-龥A-Za-z_]+?[^;]*?)\}";
            Regex reg = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            FormatField field = null;
            string result = tagInnerHTML;

            for (Match match = reg.Match(tagInnerHTML); match.Success; match = match.NextMatch())
            {
                field = new FormatField(match.Groups["field"].Value);

                object propertyValue = getPropertyValue(fillEntity, field.name);

                if ((propertyValue != null) && (propertyValue.ToString() != ""))
                {
                    string htmlTxt = propertyValue.ToString();

                    // 格式化时间
                    if ((field.isHasParameters) && (field.parameters.Get("t") != ""))
                    {
                        var style = field.parameters.Get("t");

                        htmlTxt = Sun.Toolkit.Date.formatTime(htmlTxt, style);
                    }


                    result = result.Replace(match.Value, htmlTxt);
                }
                else
                {
                    result = result.Replace(match.Value, "SunCMS提示：" + field.name + ":相应的字段不存在");
                }
            }
            reg = null;

            return result;
        }


        private static PropertyInfo getPropertyInfo(PropertyInfo[] props, string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                foreach (PropertyInfo info in props)
                {
                    if (info.Name.ToLower() == name.ToLower())
                    {
                        if (name.ToLower() == "item")
                        {
                            if (info.GetIndexParameters()[0].ParameterType == typeof(string))
                            {
                                return info;
                            }
                        }
                        else
                        {
                            return info;
                        }
                    }
                }
            }
            return null;
        }

        //private static object getPropertyValue111(object obj, string sFormat)
        //{
        //    string pattern = @"\s*(?<fun>[^\(]+?)\((?<params>.*?)\)\s*|(?<fun>[^_]+?)_(?<params>.*)";
        //    Match match = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase).Match(sFormat);
        //    if (match.Success)
        //    {
        //        string name = match.Groups["fun"].Value;
        //        string value = match.Groups["params"].Value;
        //        object[] args = format(obj, value).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        //        try
        //        {
        //            return obj.GetType().InvokeMember(name, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase, null, obj, args);
        //        }
        //        catch (Exception)
        //        {

        //            throw;
        //        }
        //    }
        //    return null;
        //}

        private static object getPropertyValue(object filledEntity, string propName)
        {
            if ((filledEntity.GetType() == typeof(string)) && (propName.ToLower() == "item"))
            {
                return filledEntity.ToString();
            }

            PropertyInfo[] props = filledEntity.GetType().GetProperties();
            PropertyInfo propertyInfo = getPropertyInfo(props, propName);

            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(filledEntity, null);
            }

            return null;
        }

    }
}
