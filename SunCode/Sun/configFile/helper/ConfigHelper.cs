using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web.Caching;

namespace Sun
{
    /// <summary>
    /// //config文件  管理中心
    /// </summary>
    public class ConfigHelper
    {
        public static object lockHelper = new object();

        /// <summary>
        /// //返回 config对象__如果存在文件则 反序，否则返回传入的type对象
        /// </summary>
        public static object GetConfig(Type configType, string configName)
        {
            string key = "SUNCMS_CACHE_" + configName.ToUpper();
            object obj = Sun.SunCache.GetValue(key);
            if ((obj == null) && (configType != null))
            {
                try
                {
                    lock (lockHelper)
                    {
                        string path = Util.Path.GetMapPath(Sun.appSetting.configPath) + configName.ToLower();
                        if (File.Exists(path))  //如果存在在反序
                        {
                            obj = SerializationHelper.Load(configType, path);
                            Sun.SunCache.InsertMaxCache(key, obj, new CacheDependency(path));
                            return obj;
                        }
                        return Activator.CreateInstance(configType);
                    }
                }
                catch (Exception ex)
                {
                    //TODO:: 错误日志   tip:有可能是文件编码格式不正确
                    throw ex;
                }
            }
            return obj;
        }

        public static T getConfig<T>(string configName) where T : class
        {
            string key = "SUNCMS_CACHE_" + configName.ToUpper();
            Type entType = typeof(T);

            T ent = Sun.SunCache.GetValue(key) as T;
            if ((ent == null) && (entType != null))
            {
                try
                {
                    lock (lockHelper)
                    {
                        string path = Sun.Toolkit.context.getMapPath(Sun.appSetting.configPath) + configName.ToLower();
                        if (File.Exists(path))  //如果文件存在则 反序列化
                        {
                            ent = SerializationHelper.Load(entType, path) as T;
                            Sun.SunCache.InsertMaxCache(key, ent, new CacheDependency(path));
                            return ent;
                        }
                        return Activator.CreateInstance<T>();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return ent;
        }

        /// <summary>
        /// //保存config
        /// </summary>
        public static void SaveConfig(object config, string FileFullName)
        {
            SerializationHelper.Save(config, Util.Path.GetMapPath(Sun.appSetting.configPath) + FileFullName);
        }

    }
}
