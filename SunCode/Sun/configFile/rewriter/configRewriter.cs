using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Sun.Toolkit;
using System.IO;

// xml文件中 和 类的属性 对应 是大小写敏感的

namespace Sun
{
    /// <summary>
    /// // url 重写
    /// </summary>
    [Serializable, XmlRoot("rewriter")]
    public class configRewriter
    {
        private static string key = "SUNCACHE_CONFIG_REWRITER";
        private List<rewriterRule> _rules;

        public static configRewriter getConfig()
        {
            XmlSerializer serializer = null;

            if (SunCache.GetValue(key) == null)
            {
                string path = appSetting.configPath + "sun.config";
                path = context.getMapPath(path);

                if (File.Exists(path))
                {
                    serializer = Sun.SerializationHelper.GetXmlSerializer(typeof(configRewriter));
                    SunCache.InsertMaxCache(key, serializer.Deserialize(new XmlNodeReader(Sun.ConfigSun.GetNode("Rewriter"))), new System.Web.Caching.CacheDependency(path));
                }
            }

            return (configRewriter)SunCache.GetValue(key);
        }

        public List<rewriterRule> rules
        {
            get
            {
                return this._rules;
            }
            set
            {
                this._rules = value;
            }
        }
    }
}
