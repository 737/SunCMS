using System;
using System.Collections.Generic;
using System.Data;

namespace Sun.Data
{
    /// <summary>
    /// //通用方法
    /// </summary>
    public class Util
    {
        /// <summary>
        /// //指定要用于SqlParameter中的字段和属性的 SQL Server 特定的数据类型。
        /// </summary>
        public static Type GetType(SqlDbType type)
        {
            switch (type)
            {
                case SqlDbType.BigInt:
                    return typeof(long);

                case SqlDbType.Binary:
                    return typeof(byte[]);

                case SqlDbType.Bit:
                    return typeof(bool);

                case SqlDbType.Char:
                    return typeof(string);

                case SqlDbType.DateTime:
                    return typeof(DateTime);

                case SqlDbType.Decimal:
                    return typeof(decimal);

                case SqlDbType.Float:
                    return typeof(double);

                case SqlDbType.Image:
                    return typeof(byte[]);

                case SqlDbType.Int:
                    return typeof(int);

                case SqlDbType.Money:
                    return typeof(decimal);

                case SqlDbType.NChar:
                    return typeof(string);

                case SqlDbType.NText:
                    return typeof(string);

                case SqlDbType.NVarChar:
                    return typeof(string);

                case SqlDbType.Real:
                    return typeof(float);

                case SqlDbType.UniqueIdentifier:
                    return typeof(Guid);

                case SqlDbType.SmallDateTime:
                    return typeof(DateTime);

                case SqlDbType.SmallInt:
                    return typeof(short);

                case SqlDbType.SmallMoney:
                    return typeof(double);

                case SqlDbType.Text:
                    return typeof(string);

                case SqlDbType.Timestamp:
                    return typeof(TimeSpan);

                case SqlDbType.TinyInt:
                    return typeof(byte);

                case SqlDbType.VarBinary:
                    return typeof(byte[]);

                case SqlDbType.VarChar:
                    return typeof(string);

                case SqlDbType.Variant:
                    return typeof(byte[]);
            }
            return typeof(string);
        }

    }
}
