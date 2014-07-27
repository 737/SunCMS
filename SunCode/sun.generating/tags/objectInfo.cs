using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace sun.generating.tags
{
    public class objectInfo : iObjectProvider
    {
        private tags.attributes _attributes;
        private object _provider;

        private string __provider;
        public string provider
        {
            get
            {
                if (this.__provider == null)
                {
                    this.__provider = this._attributes["provider"];
                }
                return this.__provider; ;
            }
            set
            {
                this.__provider = value;
            }
        }
        private string __method;
        public string method
        {
            get
            {
                if (this.__method == null)
                {
                    this.__method = this._attributes["method"];
                }
                return this.__method;
            }
            set
            {
                this.__method = value;
            }
        }


        public objectInfo(tags.attributes attr)
        {
            this._attributes = attr;
            this._provider = Sun.assemblies.getAssembly(this.provider);
        }

        public object getObject()
        {
            object obj;
            try
            {
                Type type = this._provider.GetType();
                MethodInfo[] methods = type.GetMethods();
                MethodInfo info = null;
                foreach (MethodInfo item in methods)
                {
                    if (item.Name.ToLower() == this.method.ToLower())
                    {
                        info = item;
                        break;
                    }
                }
                if (info == null)
                {
                    throw new Exception("没有找到方法" + this.method);
                }
                ParameterInfo[] parameters = info.GetParameters();
                object[] objParams = new object[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    ParameterInfo forInfo = parameters[i];
                    string str = this._attributes[forInfo.Name];
                    if (forInfo.ParameterType.Equals(typeof(int)))
                    {
                        objParams[i] = string.IsNullOrEmpty(str) ? -1 : int.Parse(str);
                    }
                    else if ((forInfo.ParameterType.Equals(typeof(int))))
                    {
                        objParams[i] = string.IsNullOrEmpty(str) ? DateTime.Today : DateTime.Parse(str);
                    }
                    else
                    {
                        objParams[i] = string.IsNullOrEmpty(str) ? "" : str;
                    }
                }
                if (info.IsStatic)
                {
                    return info.Invoke(null, objParams);
                }
                obj = info.Invoke(type.Assembly.CreateInstance(type.FullName), objParams);
            }
            catch (Exception)
            {

                throw;
            }
            return obj;
        }
    }
}
