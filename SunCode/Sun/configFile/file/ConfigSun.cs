using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Web.Caching;
using System.Collections.Specialized;

namespace Sun
{
    /// <summary>
    /// //XML-->sun.config  网站配置中心
    /// </summary>
    public class ConfigSun
    {
        /// <summary>
        /// //返回 所依赖的文件
        /// </summary>
        public static CacheDependency CreateCacheDependency()
        {
            string _path = Util.Path.GetMapPath(Sun.appSetting.configPath + "sun.config");
            return new CacheDependency(_path);
        }

        /// <summary>
        /// //得到sun.config文件中除根节点以外,所有第一级节点值例表
        /// </summary>
        private static XmlNodeList GetFirstNodeList()
        {
            string key = "SUNCACHE_CONFIG_SUNCONFIG_NODELIST";
            XmlNodeList childNodeList = Sun.SunCache.GetValue(key) as XmlNodeList;
            if (childNodeList == null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    string filePath = Util.Path.GetMapPath(Sun.appSetting.configPath + "sun.config");
                    xmlDoc.Load(filePath);
                    childNodeList = xmlDoc.SelectSingleNode("sun").ChildNodes;
                    SunCache.InsertMaxCache(key, childNodeList, CreateCacheDependency());
                }
                catch
                {
                    childNodeList = xmlDoc.ChildNodes;
                }
                return childNodeList;
            }
            return childNodeList;
        }

        /// <summary>
        /// //返回指定节点
        /// </summary>
        public static XmlNode GetNode(string node)
        {
            foreach (XmlNode _node in GetFirstNodeList())
            {
                if (_node.Name.ToLower() == node.ToLower())
                {
                    return _node;
                }
            }
            return null;
        }

        /// <summary>
        /// //返回节点的键值列
        /// </summary>
        public static HelperNameValueCollection GetSection(string section)
        {
            string key = "SUNCACHE_CONFIG_SUNCONFIG_" + section.ToUpper();
            HelperNameValueCollection _nvc = SunCache.GetValue(key) as HelperNameValueCollection;
            if (_nvc == null)
            {
                NameValueCollection onvc = new NameValueCollection();
                XmlNode _node = GetNode(section);   //临时变量
                if (_node != null)
                {
                    foreach (XmlNode item in _node.ChildNodes)
                    {
                        if (item.NodeType != XmlNodeType.Comment)
                        {
                            onvc.Add(item.Attributes["key"].Value, item.Attributes["value"].Value);
                        }
                    }
                    _nvc = new HelperNameValueCollection(onvc);
                    Sun.SunCache.InsertCache(key, _nvc, 0xe10);
                }
                return _nvc;
            }
            return _nvc;
        }



        /// <summary>
        /// //设置xml节点
        /// </summary>
        public static void SetValue(string section, string key, string value)
        {
            string filePath = Util.Path.GetMapPath(Sun.appSetting.configPath + "sun.config");
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(filePath);
                XmlNode _node = xmlDoc.SelectSingleNode("/sun/" + section + "/add[@key='" + key + "']");
                if (_node != null)
                {
                    _node.Attributes["value"].Value = value;
                }
                else
                {
                    _node = xmlDoc.SelectSingleNode("/sun/" + section);
                    if (_node == null)
                    {
                        _node = xmlDoc.CreateNode(XmlNodeType.Element, section, xmlDoc.NamespaceURI);
                        xmlDoc.SelectSingleNode("sun").AppendChild(_node);
                    }
                    XmlNode onode = xmlDoc.CreateNode(XmlNodeType.Element, "add", xmlDoc.NamespaceURI);
                    onode.Attributes.Append(xmlDoc.CreateAttribute("key")).Value = key;
                    onode.Attributes.Append(xmlDoc.CreateAttribute("add")).Value = value;
                    _node.AppendChild(onode);
                }
                xmlDoc.Save(filePath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// //返回被序列化的 本对象
        /// </summary>
        public static ConfigSun Instance()
        {
            return (ConfigHelper.GetConfig(typeof(ConfigSun), "site.config") as ConfigSun);
        }
    }
}
