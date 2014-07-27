using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sun.Util
{
    public static class Clone
    {

        public static T getClone<T>(T entity) where T : new()   //这里的new()指的是T必须有构造函数
        {
            T entityClone = new T();

            try
            {
                Type type = entity.GetType();
                FieldInfo[] fieldInfos = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);//GetFields中的参数表示需要查找什么样的成员
                foreach (FieldInfo fieldinfo in fieldInfos)
                {
                    fieldinfo.SetValue(entityClone, fieldinfo.GetValue(entity));//这里可以给新的对象的字段赋值
                }
            }
            catch (Exception)
            {

                return default(T);//如果出错的话就返回默认的值
            }

            return entity;

        }
    }
}
