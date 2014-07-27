using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sun
{
    /// <summary>
    /// //数据库 提供者
    /// </summary>
    public class ProviderData
    {
        private static Hashtable allProvider;

        public static object GetProvider(string name)
        {
            if (allProvider == null)
            {
                allProvider = new Hashtable();
            }
            object classObj = allProvider[name];
            if (classObj == null)
            {
                if (assemblies.isHasKey(name))
                {
                    classObj = assemblies.getAssembly(name);
                }
                if (classObj == null)
                {
                    Type type = Type.GetType(string.Format("Sun.Data.{1}.{0}, Sun.Data.{1}", name, "SqlServer"));
                    if (type != null)
                    {
                        classObj = Activator.CreateInstance(type);
                    }
                }
                allProvider[name] = classObj;
            }
            return classObj;
        }

    }
}
