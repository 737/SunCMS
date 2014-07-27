using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Sun.Toolkit
{
    public class HelperType
    {
        /// <summary>
        /// 得到一个实际的类型（排除Nullable类型的影响）。比如：int? 最后将得到int
        /// </summary>
        public static Type GetRealType(Type type)
        {
            if (type.IsGenericType)
                return Nullable.GetUnderlyingType(type) ?? type;
            else
                return type;
        }
        /// <summary>
        /// 判断一个类型是否是泛型。比如：int?
        /// </summary>
        public static bool IsGeneric(Type type)
        {
            if (type.IsGenericType)
            {
                return true;
            }
            return false;
        }

    }
}
