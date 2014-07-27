using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web.Caching;
using System.Xml;

namespace Sun
{
    public class configTag
    {
        private static string key = "SUNCMS_CACHE_TAGS";

        /// <summary>
        /// // 根据nameSpace找到相关的 依赖数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static tagEntity getTag(string tagNameSpace)
        {
            var provider = getProvider();
            if (provider == null)
            {
                return null;
            }
            return getProvider()[tagNameSpace] as tagEntity;
        }

        private static Hashtable getProvider()
        {
            Hashtable hashtable = SunCache.GetValue(key) as Hashtable;
            if (hashtable == null)
            {
                string filePath = Sun.Toolkit.context.getMapPath(appSetting.configPath + "tags.config");
                CacheDependency dep = new CacheDependency(filePath);
                hashtable = new Hashtable();
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(filePath);
                foreach (XmlNode node in xmldoc.SelectSingleNode("tags").ChildNodes)
                {
                    if (node.NodeType != XmlNodeType.Comment)
                    {
                        tagEntity tag = new tagEntity(node);
                        if (hashtable[tag.tagPrefix] == null)
                        {
                            hashtable.Add(tag.tagPrefix, tag);
                        }
                    }
                }
                SunCache.InsertMaxCache(key, hashtable, dep);
            }
            return hashtable;
        }

    }
}
