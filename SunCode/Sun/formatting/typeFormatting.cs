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

        private static object getPropertyValue(object obj, string sFormat)
        {
            string pattern = @"\s*(?<fun>[^\(]+?)\((?<params>.*?)\)\s*|(?<fun>[^_]+?)_(?<params>.*)";
            Match match = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase).Match(sFormat);
            if (match.Success)
            {
                string name = match.Groups["fun"].Value;
                string value = match.Groups["params"].Value;
                object[] args = format(obj, value).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    return obj.GetType().InvokeMember(name, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase, null, obj, args);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return null;
        }
        private static object getPropertyValue(object obj, PropertyInfo[] props, string propName)
        {
            if ((obj.GetType() == typeof(string)) && (propName.ToLower() == "item"))
            {
                return obj.ToString();
            }
            PropertyInfo propertyInfo = getPropertyInfo(props, propName);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(obj, null);
            }
            propertyInfo = getPropertyInfo(obj.GetType().GetProperties(), "item");
            if (propertyInfo != null)
            {
                try
                {
                    return propertyInfo.GetValue(obj, new object[] { propName });
                }
                catch (Exception)
                {
                    return propName;
                }
            }
            return null;
        }


        public static string format(object fillEntity, string html)
        {
            if (fillEntity == null)
            {
                return html;
            }
            PropertyInfo[] properties = fillEntity.GetType().GetProperties();
            string pattern = @"\{(?<value>[一-龥A-Za-z_]+?[^;]*?)\}";
            string methodResult = html;
            Regex reg = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            for (Match match = reg.Match(html); match.Success; match = match.NextMatch())
            {
                Sun.diagnostics.log.recordError("--------" + match.Groups["value"].Value);

                typeFormatField field = new typeFormatField(match.Groups["value"].Value);
                if (string.IsNullOrEmpty(field.name))
                {
                    methodResult = methodResult.Replace(match.Value, field.getValue("n"));
                }
                else
                {
                    object propertyValue = null;

                    propertyValue = getPropertyValue(fillEntity, properties, field.name);

                    if ((propertyValue == null) || (propertyValue.ToString() == ""))
                    {
                        methodResult = methodResult.Replace(match.Value, "SunCMS提示：没有找到相应的内容");
                    }
                    else
                    {
                        string htmlTxt = propertyValue.ToString();
                        methodResult = methodResult.Replace(match.Value, htmlTxt);
                    }
                }
            }
            reg = null;
            properties = null;
            return methodResult;
        }

    }
}
