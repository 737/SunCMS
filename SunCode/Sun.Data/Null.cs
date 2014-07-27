using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Sun.Data
{
    public class Null
    {
        public static object GetNull(object objField, object objDBNull)
        {
            object obj2 = objField;
            if (objField == null)
            {
                return objDBNull;
            }
            if (objField is short)
            {
                if (Convert.ToInt16(objField) == NullShort)
                {
                    obj2 = objDBNull;
                }
                return obj2;
            }
            if (objField is int)
            {
                if (Convert.ToInt32(objField) == NullInteger)
                {
                    obj2 = objDBNull;
                }
                return obj2;
            }
            if (objField is float)
            {
                if (Convert.ToSingle(objField) == NullSingle)
                {
                    obj2 = objDBNull;
                }
                return obj2;
            }
            if (objField is double)
            {
                if (Convert.ToDouble(objField) == NullDouble)
                {
                    obj2 = objDBNull;
                }
                return obj2;
            }
            if (objField is decimal)
            {
                if (Convert.ToDecimal(objField) == NullDecimal)
                {
                    obj2 = objDBNull;
                }
                return obj2;
            }
            if (objField is DateTime)
            {
                if (Convert.ToDateTime(objField) == NullDate.Date)
                {
                    obj2 = objDBNull;
                }
                return obj2;
            }
            if (objField is string)
            {
                if (objField == null)
                {
                    obj2 = objDBNull;
                }
                return obj2;
            }
            if (objField is bool)
            {
                if (Convert.ToBoolean(objField) == NullBoolean)
                {
                    obj2 = objDBNull;
                }
                return obj2;
            }
            if (objField is Guid)
            {
                Guid guid = (Guid)objField;
                if (guid.Equals(NullGuid))
                {
                    obj2 = objDBNull;
                }
            }
            return obj2;
        }

        public static bool IsNull(object objField)
        {
            if (objField != null)
            {
                if (objField is int)
                {
                    return objField.Equals(NullInteger);
                }
                if (objField is float)
                {
                    return objField.Equals(NullSingle);
                }
                if (objField is double)
                {
                    return objField.Equals(NullDouble);
                }
                if (objField is decimal)
                {
                    return objField.Equals(NullDecimal);
                }
                if (objField is DateTime)
                {
                    return Convert.ToDateTime(objField).Equals(NullInteger);
                }
                if (objField is string)
                {
                    return objField.Equals(NullString);
                }
                if (objField is bool)
                {
                    return objField.Equals(NullBoolean);
                }
                return ((objField is Guid) && objField.Equals(NullGuid));
            }
            return true;
        }

        public static object SetNull(FieldInfo objFieldInfo)
        {
            switch (objFieldInfo.FieldType.ToString())
            {
                case "System.Int16":
                    return NullShort;

                case "System.Int32":
                case "System.Int64":
                    return NullInteger;

                case "System.Single":
                    return NullSingle;

                case "System.Double":
                    return NullDouble;

                case "System.Decimal":
                    return NullDecimal;

                case "System.DateTime":
                    return NullDate;

                case "System.String":
                case "System.Char":
                    return NullString;

                case "System.Boolean":
                    return NullBoolean;

                case "System.Guid":
                    return NullGuid;
            }
            Type fieldType = objFieldInfo.FieldType;
            if (fieldType.BaseType.Equals(typeof(Enum)))
            {
                Array values = Enum.GetValues(fieldType);
                Array.Sort(values);
                return Enum.ToObject(fieldType, values.GetValue(0));
            }
            return null;
        }

        /// <summary>
        /// //设置为空情况下的值
        /// </summary>
        public static object SetNull(PropertyInfo objPropertyInfo)
        {
            switch (objPropertyInfo.PropertyType.ToString())
            {
                case "System.Int16":
                    return NullShort;

                case "System.Int32":
                case "System.Int64":
                    return NullInteger;

                case "System.Single":
                    return NullSingle;

                case "System.Double":
                    return NullDouble;

                case "System.Decimal":
                    return NullDecimal;

                case "System.DateTime":
                    return NullDate;

                case "System.String":
                case "System.Char":
                    return NullString;

                case "System.Boolean":
                    return NullBoolean;

                case "System.Guid":
                    return NullGuid;
            }
            Type propertyType = objPropertyInfo.PropertyType;
            if (propertyType.BaseType.Equals(typeof(Enum)))
            {
                Array values = Enum.GetValues(propertyType);
                Array.Sort(values);
                return Enum.ToObject(propertyType, values.GetValue(0));
            }
            return null;
        }


        public static object SetNull(object objValue, object objField)
        {
            if (objValue is DBNull)
            {
                if (objField is short)
                {
                    return NullShort;
                }
                if (objField is int)
                {
                    return NullInteger;
                }
                if (objField is float)
                {
                    return NullSingle;
                }
                if (objField is double)
                {
                    return NullDouble;
                }
                if (objField is decimal)
                {
                    return NullDecimal;
                }
                if (objField is DateTime)
                {
                    return NullDate;
                }
                if (objField is string)
                {
                    return NullString;
                }
                if (objField is bool)
                {
                    return NullBoolean;
                }
                if (objField is Guid)
                {
                    return NullGuid;
                }
                return null;
            }
            return objValue;
        }
        /// <summary>
        /// //返回 false
        /// </summary>
        public static bool NullBoolean
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// //返回  new DateTime(0x76c, 1, 1) ____  1900/1/1 0:00:00
        /// </summary>
        public static DateTime NullDate
        {
            get
            {
                return new DateTime(0x76c, 1, 1);
            }
        }
        /// <summary>
        /// //返回 -79228162514264337593543950335M;
        /// </summary>
        public static decimal NullDecimal
        {
            get
            {
                return -79228162514264337593543950335M;
            }
        }
        /// <summary>
        /// //返回 表示 System.Double 的最小可能值。此字段为常数。 MinValue = -1.79769e+308;
        /// </summary>
        public static double NullDouble
        {
            get
            {
                return double.MinValue;
            }
        }
        /// <summary>
        /// //返回 Guid.Empty，其值保证均为零。
        /// </summary>
        public static Guid NullGuid
        {
            get
            {
                return Guid.Empty;
            }
        }
        /// <summary>
        /// //返回 -1
        /// </summary>
        public static int NullInteger
        {
            get
            {
                return -1;
            }
        }
        /// <summary>
        /// //返回 -1
        /// </summary>
        public static short NullShort
        {
            get
            {
                return -1;
            }
        }
        /// <summary>
        /// //返回 表示 System.Single 的最小可能值。此字段为常数。MinValue = -3.40282e+038f;
        /// </summary>
        public static float NullSingle
        {
            get
            {
                return float.MinValue;
            }
        }
        /// <summary>
        /// //返回 ""
        /// </summary>
        public static string NullString
        {
            get
            {
                return "";
            }
        }
    }
}
