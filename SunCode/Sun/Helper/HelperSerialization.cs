using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Sun
{
    /// <summary>
    /// //序列化 助手
    /// </summary>
    public class SerializationHelper
    {
        /// <summary>
        /// //返回 type参数 的 XmlSerializer(执行者)
        /// </summary>
        public static XmlSerializer GetXmlSerializer(Type type)
        {
            string key = "SUNCMS_CACHE_" + type.FullName.ToUpper().Replace('.', '_');
            XmlSerializer serializer = Sun.SunCache.GetValue(key) as XmlSerializer;
            if (serializer == null)
            {
                try
                {
                    serializer = new XmlSerializer(type);
                    Sun.SunCache.InsertCache(key, serializer);
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
                
            }
            return serializer;
        }

        /// <summary>
        /// //返回 反序 后的对象
        /// </summary>
        public static object Load(Type type, string path)
        {
            FileStream stream = null;
            object obj;
            try
            {
                stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                obj = GetXmlSerializer(type).Deserialize(stream);
            }
            catch (Exception ex)
            {
                //sc.log
                ex = new Exception("配置文件读取错误！" + "错误信息为:" + ex.Message + "路径为：" + path);
                throw ex;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return obj;
        }

        public static void Save(object objModel, string filPath)
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(filPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                GetXmlSerializer(objModel.GetType()).Serialize(stream, objModel);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

    }
}
