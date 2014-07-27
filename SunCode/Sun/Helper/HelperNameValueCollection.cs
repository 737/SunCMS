using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Sun
{
    /// <summary>
    /// //NameValueCollection  助手
    /// </summary>
    public class HelperNameValueCollection
    {
        private NameValueCollection _nvc;

        internal HelperNameValueCollection(NameValueCollection nvc)
        {
            this._nvc = nvc;
        }

        /// <summary>
        /// //返回 key --> value
        /// </summary>
        public string GetKeyValue(string key)
        {
            string str = _nvc[key];
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return str;
        }

        /// <summary>
        /// //返回value 转为 bool类型
        /// </summary>
        public bool GetKeyBoolValue(string key)
        {
            return (this.GetKeyValue(key).ToLower() == "true");
        }

        /// <summary>
        /// //返回其实例对象
        /// </summary>
        public NameValueCollection instance
        {
            get
            {
                return this._nvc;
            }
        }
    }
}
