using System;
using System.Collections.Generic;
using System.Text;
using Sun.formatting;

namespace Sun
{
    /// <summary>
    /// //代表sun.config文件中 locations结点
    /// </summary>
    public class Location
    {
        /// <summary>
        /// //返回使用location结点的值格式化后的串
        /// </summary>
        public static string Format(string path)
        {
            try
            {
                HelperNameValueCollection section = ConfigSun.GetSection("locations");
                textFormatting _formatting = new textFormatting();
                foreach (string key in section.instance.Keys)
                {
                    _formatting.Add(key, section.GetKeyValue(key));
                }
                return _formatting.Format(path);
            }
            catch
            {
                return path;
            }
        }

    }
}
