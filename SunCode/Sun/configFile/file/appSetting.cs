using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;

namespace Sun
{
    /// <summary>
    /// //webConfig中appsettings结点
    /// </summary>
    public class appSetting
    {
        /// <summary>
        /// //返回web.config中AppSettings节点的键值
        /// </summary>
        public static NameValueCollection getSettings()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            if (appSettings == null)
            {
                return null;
            }
            return appSettings;
        }

        /// <summary>
        /// //返回 key --> value
        /// </summary>
        public static string getValue(string key)
        {
            string _value = getSettings()[key];
            if (string.IsNullOrEmpty(_value))
            {
                return "";
            }
            return _value;
        }

        /// <summary>
        /// //返回configs文件夹的路径
        /// </summary>
        public static string configPath
        {
            get
            {
                string _path = getValue("configPath");
                if (_path == string.Empty || _path == null)
                {
                    return "~/suncms/config/";
                }
                if (!_path.EndsWith("/"))
                {
                    _path = _path + "/";
                }
                return _path;
            }
        }

    }
}
