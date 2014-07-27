using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Sun.Hubble
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

        public static object ConvertDataTableToGenericObject(DataTable dt, Type type)
        {
            Type ex = typeof(Util);
            object[] genericArgs = { dt };

            MethodInfo mi = ex.GetMethod("ConvertDataTableToList");
            MethodInfo miConstructed = mi.MakeGenericMethod(type);

            object dataObj = miConstructed.Invoke(null, genericArgs);

            return dataObj;
        }

        /// <summary> 
        /// 单表查询结果转换成泛型集合 
        /// </summary> 
        /// <typeparam name="T">泛型集合类型</typeparam> 
        /// <param name="dt">查询结果DataTable</param> 
        /// <returns>以实体类为元素的泛型集合</returns> 
        public static List<T> ConvertDataTableToList<T>(DataTable dt) where T : new()
        {
            // 定义集合 
            List<T> ts = new List<T>();

            // 获得此模型的类型 
            Type type = typeof(T);
            //定义一个临时变量 
            string tempName = string.Empty;
            //遍历DataTable中所有的数据行  
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性 
                PropertyInfo[] propertys = t.GetType().GetProperties();
                //遍历该对象的所有属性 
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;//将属性名称赋值给临时变量   
                    //检查DataTable是否包含此列（列名==对象的属性名）     
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter   
                        if (!pi.CanWrite) continue;//该属性不可写，直接跳出   
                        //取值   
                        object value = dr[tempName];
                        //如果非空，则赋给对象的属性   
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                //对象添加到泛型集合中 
                ts.Add(t);
            }

            return ts;
        }


        private static string[] SQLRETAIN_KEYWORDS = new string[] { "ADD", "EXCEPT", "PERCENT", "ALL", "EXEC", "PLAN", "ALTER", "EXECUTE", "PRECISION", "AND", "EXISTS", "PRIMARY", "ANY", "EXIT", "PRINT", "AS", "FETCH", "PROC", "ASC", "FILE", "PROCEDURE", "AUTHORIZATION", "FILLFACTOR", "PUBLIC", "BACKUP", "FOR", "RAISERROR", "BEGIN", "FOREIGN", "READ", "BETWEEN", "FREETEXT", "READTEXT", "BREAK", "FREETEXTTABLE", "RECONFIGURE", "BROWSE", "FROM", "REFERENCES", "BULK", "FULL", "REPLICATION", "BY", "FUNCTION", "RESTORE", "CASCADE", "GOTO", "RESTRICT", "CASE", "GRANT", "RETURN", "CHECK", "GROUP", "REVOKE", "CHECKPOINT", "HAVING", "RIGHT", "CLOSE", "HOLDLOCK", "ROLLBACK", "CLUSTERED", "IDENTITY", "ROWCOUNT", "COALESCE", "IDENTITY_INSERT", "ROWGUIDCOL", "COLLATE", "IDENTITYCOL", "RULE", "COLUMN", "IF", "SAVE", "COMMIT", "IN", "SCHEMA", "COMPUTE", "INDEX", "SELECT", "CONSTRAINT", "INNER", "SESSION_USER", "CONTAINS", "INSERT", "SET", "CONTAINSTABLE", "INTERSECT", "SETUSER", "CONTINUE", "INTO", "SHUTDOWN", "CONVERT", "IS", "SOME", "CREATE", "JOIN", "STATISTICS", "CROSS", "KEY", "SYSTEM_USER", "CURRENT", "KILL", "TABLE", "CURRENT_DATE", "LEFT", "TEXTSIZE", "CURRENT_TIME", "LIKE", "THEN", "CURRENT_TIMESTAMP", "LINENO", "TO", "CURRENT_USER", "LOAD", "TOP", "CURSOR", "NATIONAL", "TRAN", "DATABASE", "NOCHECK", "TRANSACTION", "DBCC", "NONCLUSTERED", "TRIGGER", "DEALLOCATE", "NOT", "TRUNCATE", "DECLARE", "NULL", "TSEQUAL", "DEFAULT", "NULLIF", "UNION", "DELETE", "OF", "UNIQUE", "DENY", "OFF", "UPDATE", "DESC", "OFFSETS", "UPDATETEXT", "DISK", "ON", "USE", "DISTINCT", "OPEN", "USER", "DISTRIBUTED", "OPENDATASOURCE", "VALUES", "DOUBLE", "OPENQUERY", "VARYING", "DROP", "OPENROWSET", "VIEW", "DUMMY", "OPENXML", "WAITFOR", "DUMP", "OPTION", "WHEN", "ELSE", "OR", "WHERE", "END", "ORDER", "WHILE", "ERRLVL", "OUTER", "WITH", "ESCAPE", "OVER", "WRITETEXT" };
        // 解析是不是 SQL server 保留字  例如： index => [index]
        public static string parseSqlRetain(string word)
        {
            foreach (var item in SQLRETAIN_KEYWORDS)
            {
                if (string.Compare(item, word, true) == 0)
                {
                    word = string.Format("{0}{1}{2}", new string[] { "[", word, "]" });
                    return word;
                }
            }

            return word;
        }
    }
}
