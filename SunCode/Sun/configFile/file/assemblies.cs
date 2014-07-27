using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Xml;

namespace Sun
{
    /// <summary>
    /// //程序集合  xml ==> assebmly.config
    /// </summary>
    public class assemblies
    {
        /// <summary>
        /// //返回Assembly.Config中所有的键值
        /// </summary>
        private static NameValueCollection getNodeList()
        {
            string key = "SUNCACHE_CONFIG_ASSEMBLY";
            NameValueCollection nvc = SunCache.GetValue(key) as NameValueCollection;
            if (nvc == null)
            {
                string path = Util.Path.GetMapPath(appSetting.configPath + "Assembly.config");
                if (!Util.IO.FileExist(path))
                {
                    return nvc;
                }
                CacheDependency depend = new CacheDependency(path);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(path);
                nvc = new NameValueCollection();
                foreach (XmlNode node in xmldoc.SelectSingleNode("assembly").ChildNodes)
                {
                    if (node.NodeType != XmlNodeType.Comment)
                    {
                        nvc.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                    }
                }
                SunCache.InsertMaxCache(key, nvc, depend);
            }
            return nvc;
        }

        /// <summary>
        /// //判读Assemblies中是否有此集合
        /// </summary>
        public static bool isHasKey(string key)
        {
            string value = getNodeList()[key];
            if (!string.IsNullOrEmpty(value))
            {
                return true;
            }
            return false;
        }

        public static Type getAssemblyType(string name)
        {
            Type type;
            string dll = getNodeList()[name];
            if (string.IsNullOrEmpty(dll))
            {
                throw new Exception("没有找到对应的assembly:" + name);
            }
            try
            {
                type = Type.GetType(dll);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return type;
        }

        /// <summary>
        /// //返回指定的集合
        /// </summary>
        public static object getAssembly(string name)
        {
            string usedKey = "SUNCACHE_CONFIG_ASSEMBLY_USED";
            Hashtable hashtable = SunCache.GetValue(usedKey) as Hashtable;
            if (hashtable == null)
            {
                hashtable = new Hashtable();
            }
            object classObj = hashtable[name];
            if (classObj == null)
            {
                string assemblyName = getNodeList()[name];
                if (string.IsNullOrEmpty(assemblyName))
                {
                    throw new ApplicationException("没有找到 对应的 obj:" + name);
                }
                try
                {
                    classObj = Activator.CreateInstance(Type.GetType(assemblyName));
                    hashtable[name] = classObj;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            hashtable = null;
            return classObj;
        }
    }
}
