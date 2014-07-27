using System;
using System.Collections;
using System.IO;
using System.Resources;
using System.Web.Caching;
using System.Web.Handlers;
using System.Reflection;
using System.Collections.Generic;

namespace Sun
{
    /// <summary>
    /// //SunCMS 翻译 中心
    /// </summary>
    public class HelperTranslation
    {
        /// <summary>
        /// //返回指定的字符串翻译后的结果---翻译带$的词
        /// </summary>
        public static string Replace(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }
            if ((text.Length > 1) && (text[0] == '$'))
            {
                return GetString(text.Substring(1).Split(new char[] { '$' }));
            }
            return text;
        }

        /// <summary>
        /// //返回 翻译后的字符
        /// </summary>
        public static string GetString(string text)
        {
            string culture = Sun.ConfigSun.GetSection("appsettings").GetKeyValue("culture");
            return GetString(text, culture);
        }

        /// <summary>
        /// //返回指定的字符串(数组)翻译后的结果
        /// </summary>
        public static string GetString(string[] text)
        {
            if (text == null)
            {
                return "";
            }
            string culture = Sun.ConfigSun.GetSection("appsettings").GetKeyValue("culture");
            string str = "";
            foreach (string item in text)
            {
                str = str + GetString(item, culture);
            }
            return str;
        }

        public static string GetString(List<string> text)
        {
            if (text == null)
            {
                return "";
            }
            string culture = Sun.ConfigSun.GetSection("appsettings").GetKeyValue("culture");
            string str = "";
            foreach (string item in text)
            {
                str = str + GetString(item, culture);
            }
            return str;
        }

        public static string GetString(string text, string culture)
        {
            return GetString(text, culture, true);
        }

        public static string GetString(string text, string culture, bool assert)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }
            string key = "SUNCACHE_CULTURE_" + culture.ToUpper();
            Hashtable hashtab = Sun.SunCache.GetValue(key) as Hashtable;
            if (hashtab == null)
            {
                string path = Sun.ConfigSun.GetSection("appsettings").GetKeyValue("resxpath");
                path = Location.Format(path) + "culture." + culture + ".resx";
                path = Util.Path.GetMapPath(path);

                if (!File.Exists(path))
                {
                    return text;
                }
                ResXResourceReader reader = new ResXResourceReader(path);
                hashtab = new Hashtable();
                foreach (DictionaryEntry entry in reader)
                {
                    hashtab.Add(entry.Key.ToString().ToLower(), entry.Value.ToString());
                }
                CacheDependency depend = new CacheDependency(path);
                if (culture == Sun.ConfigSun.GetSection("appsettings").GetKeyValue("culture"))
                {
                    Sun.SunCache.InsertMaxCache(key, hashtab, depend);
                }
                else
                {
                    Sun.SunCache.InsertCache(key, hashtab, depend);
                }
                reader.Close();
                reader = null;
            }
            string znch = hashtab[text.ToLower().Trim()] as string;
            hashtab = null;
            if (znch == null)
            {
                znch = text;
            }
            return znch;
        }

        /// <summary>
        /// //返回 webresource 的url
        /// </summary>
        public static string GetWebResourceUrl(Type type, string name)
        {
            Type type2 = typeof(AssemblyResourceLoader);
            object[] args = new object[] { type, name, true };
            BindingFlags invokeAttr = BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly;
            return (string)type2.InvokeMember("GetWebResourceUrl", invokeAttr, null, null, args);
        }
    }
}
