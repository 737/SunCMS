using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sun.Hubble
{
    public static class DataHelper
    {
        /// <summary>
        /// // old method
        /// </summary>
        //public object GetData(Sun.Entity.Pagelet.EntityInfo info)
        //{
        //    object returnObj = null;
        //    DataTable dt = new DataTable();
        //    string tablePrifix = Sun.ConfigSun.GetSection("data").GetKeyValue("tablePrefix"),
        //           tableName = "";

        //    if (string.IsNullOrEmpty(tablePrifix))
        //    {
        //        tablePrifix = "Sun_";
        //    }
        //    if (info.MappingName.Length < 0)
        //    {
        //        return returnObj;
        //    }
        //    tableName = tablePrifix + info.MappingName;

        //    _dbContext.CreateCommand("select * from " + tableName, System.Data.CommandType.Text);

        //    dt = _dbContext.FillDataTable();

        //    returnObj = ConvertDataTableToGenericObject(dt, info.EntityType);

        //    return returnObj;
        //}

        /// <summary>
        /// // 返回数据库字段 类型
        /// </summary>
        public static DbType GetDbType(string type)
        {
            if (type == "String")
            {
                return DbType.String;
            }

            DbType dtType = DbType.String;
            switch (type)
            {
                case "String":
                    dtType = DbType.String;
                    break;
                case "Boolean":
                    dtType = DbType.Boolean;
                    break;
                case "Int16":
                    dtType = DbType.Int16;
                    break;
                case "Int32":
                    dtType = DbType.Int32;
                    break;
                case "Int64":
                    dtType = DbType.Int64;
                    break;
                case "Date":
                    dtType = DbType.Date;
                    break;
                case "DateTime":
                    dtType = DbType.DateTime;
                    break;
                case "Decimal":
                    dtType = DbType.Decimal;
                    break;
            }

            return dtType;
        }

        public static List<T> GetFillList<T>(SunQuery sunQuery) where T : class,new()
        {
            object inputParam = sunQuery.FillEntity;
            using (DataContext context = new DataContext(false))
            {
                if (inputParam == null)
                {
                    SetCommand(sunQuery.SqlText, null, CommandKind.SqlTextNoParams, context);
                }
                else
                {
                    SetCommand(sunQuery.SqlText, inputParam, CommandKind.SqlTextWithParams, context, sunQuery.PrefixParam);
                }
                return context.FillList<T>();
            }
        }
        public static List<T> GetFillList<T>(string nameOrSqlTxt) where T : class,new()
        {
            return GetFillList<T>(nameOrSqlTxt, null, CommandKind.SqlTextNoParams);
        }
        public static List<T> GetFillList<T>(string nameOrSqlTxt, object inputParam) where T : class,new()
        {
            return GetFillList<T>(nameOrSqlTxt, inputParam, CommandKind.SqlTextWithParams);
        }
        public static List<T> GetFillList<T>(string nameOrSqlTxt, object inputParam, CommandKind cmdKind) where T : class, new()
        {
            if (string.IsNullOrEmpty(nameOrSqlTxt))
            {
                throw new Exception("没有sql语句");
            }
            using (DataContext context = new DataContext(false))
            {
                return GetFillList<T>(nameOrSqlTxt, inputParam, cmdKind, context);
            }

        }
        public static List<T> GetFillList<T>(string nameOrSqlTxt, object inputParam, DataContext context) where T : class,new()
        {
            return GetFillList<T>(nameOrSqlTxt, inputParam, CommandKind.SqlTextWithParams, context);
        }
        public static List<T> GetFillList<T>(string nameOrSqlTxt, object inputParam, CommandKind cmdKind, DataContext context) where T : class, new()
        {
            if (context == null)
            {
                throw new Exception("没有sqlcontext");
            }
            if (string.IsNullOrEmpty(nameOrSqlTxt))
            {
                throw new Exception("没有sql语句");
            }
            SetCommand(nameOrSqlTxt, inputParam, cmdKind, context);
            return context.FillList<T>();
        }

        public static List<T> GetFillScalarList<T>(string nameOrSqlTxt)
        {
            return GetFillScalarList<T>(nameOrSqlTxt, null, CommandKind.SqlTextNoParams);
        }
        public static List<T> GetFillScalarList<T>(string nameOrSqlTxt, object inputParam)
        {
            return GetFillScalarList<T>(nameOrSqlTxt, inputParam, CommandKind.SqlTextWithParams);
        }
        public static List<T> GetFillScalarList<T>(string nameOrSqlTxt, object inputParam, CommandKind cmdKind)
        {
            if (string.IsNullOrEmpty(nameOrSqlTxt))
            {
                throw new Exception("没有sql语句");
            }
            using (DataContext context = new DataContext(false))
            {
                return GetFillScalarList<T>(nameOrSqlTxt, inputParam, cmdKind, context);
            }
        }
        public static List<T> GetFillScalarList<T>(string nameOrSqlTxt, object inputParam, DataContext context)
        {
            return GetFillScalarList<T>(nameOrSqlTxt, inputParam, CommandKind.SqlTextWithParams, context);
        }
        public static List<T> GetFillScalarList<T>(string nameOrSqlTxt, object inputParam, CommandKind cmdKind, DataContext context)
        {
            if (context == null)
            {
                throw new Exception("没有sqlcontext");
            }
            if (string.IsNullOrEmpty(nameOrSqlTxt))
            {
                throw new Exception("没有sql语句");
            }
            SetCommand(nameOrSqlTxt, inputParam, cmdKind, context);
            return context.FillScalarList<T>();
        }
        
        // 返回 List<T>装箱后的 object
        public static IList GetFillListEntity(SunQuery sunQuery)
        {
            object inputParam = sunQuery.FillEntity;
            using (DataContext context = new DataContext(false))
            {
                if (inputParam == null)
                {
                    SetCommand(sunQuery.SqlText, null, CommandKind.SqlTextNoParams, context);
                }
                else
                {
                    SetCommand(sunQuery.SqlText, inputParam, CommandKind.SqlTextWithParams, context, sunQuery.PrefixParam);
                }
                return GetGenericFillListMethod(context, inputParam.GetType()).Invoke(context, null) as IList;
            }
        }
        public static IList GetFillListEntity(string nameOrSqlTxt, Type entityType)
        {
            object entity = Activator.CreateInstance(entityType);
            return GetFillListEntity(nameOrSqlTxt, entity, CommandKind.SqlTextNoParams);
        }
        public static IList GetFillListEntity(string nameOrSqlTxt, object inputParam)
        {
            return GetFillListEntity(nameOrSqlTxt, inputParam, CommandKind.SqlTextWithParams);
        }
        public static IList GetFillListEntity(string nameOrSqlTxt, object inputParam, CommandKind cmdKind)
        {
            using (DataContext context = new DataContext(false))
            {
                return GetFillListEntity(nameOrSqlTxt, inputParam, cmdKind, context);
            }
        }
        public static IList GetFillListEntity(string nameOrSqlTxt, object inputParam, CommandKind cmdKind, DataContext context)
        {
            SetCommand(nameOrSqlTxt, inputParam, cmdKind, context);
            return GetGenericFillListMethod(context, inputParam.GetType()).Invoke(context, null) as IList;
        }

        public static DataTable GetFillDataTable(string nameOrSqlTxt)
        {
            return GetFillDataTable(nameOrSqlTxt, null, CommandKind.SqlTextNoParams);
        }
        public static DataTable GetFillDataTable(string nameOrSqlTxt, object inputParam)
        {
            return GetFillDataTable(nameOrSqlTxt, inputParam, CommandKind.SqlTextWithParams);
        }
        public static DataTable GetFillDataTable(string nameOrSqlTxt, object inputParam, CommandKind cmdKind)
        {
            if (string.IsNullOrEmpty(nameOrSqlTxt))
            {
                throw new Exception("SQL语句为空");
            }
            using (DataContext context = new DataContext(false))
            {
                return GetFillDataTable(nameOrSqlTxt, inputParam, cmdKind, context);
            }
        }
        public static DataTable GetFillDataTable(string nameOrSqlTxt, object inputParam, CommandKind cmdKind, DataContext context)
        {
            if (context == null)
            {
                throw new Exception("没有sqlcontext");
            }
            if (string.IsNullOrEmpty(nameOrSqlTxt))
            {
                throw new Exception("没有sql语句");
            }
            SetCommand(nameOrSqlTxt, inputParam, cmdKind, context);
            return context.FillDataTable();
        }

        public static object GetExecuteScalar(string nameOrSqlTxt)
        {
            return GetExecuteScalar(nameOrSqlTxt, null, CommandKind.SqlTextNoParams);
        }
        public static object GetExecuteScalar(string nameOrSqlTxt, object inputParam)
        {
            return GetExecuteScalar(nameOrSqlTxt, inputParam, CommandKind.SqlTextWithParams);
        }
        public static object GetExecuteScalar(string nameOrSqlTxt, object inputParam, CommandKind cmdKind)
        {
            using (DataContext context = new DataContext(false))
            {
                return GetExecuteScalar(nameOrSqlTxt, inputParam, cmdKind, context);
            }
        }
        public static object GetExecuteScalar(string nameOrSqlTxt, object inputParam, DataContext context)
        {
            return GetExecuteScalar(nameOrSqlTxt, inputParam, CommandKind.SqlTextWithParams, context);
        }
        public static object GetExecuteScalar(string nameOrSqlTxt, object inputParam, CommandKind cmdKind, DataContext context)
        {
            if (context == null)
            {
                throw new Exception("没有sqlcontext");
            }
            if (string.IsNullOrEmpty(nameOrSqlTxt))
            {
                throw new Exception("没有sql语句");
            }
            SetCommand(nameOrSqlTxt, inputParam, cmdKind, context);
            return context.ExecuteScalar();
        }

        public static int SetExecuteNonQuery(SunQuery sunQuery)
        {
            object inputParam = sunQuery.FillEntity;
            using (DataContext context = new DataContext(false))
            {
                if (inputParam == null)
                {
                    return -1;
                }
                else
                {
                    SetCommand(sunQuery.SqlText, inputParam, CommandKind.SqlTextWithParams, context, sunQuery.PrefixParam);
                }
                return context.ExecuteNonQuery();
            }
        }
        public static int SetExecuteNonQuery(string nameOrSqlTxt)
        {
            return SetExecuteNonQuery(nameOrSqlTxt, null, CommandKind.SqlTextNoParams);
        }
        public static int SetExecuteNonQuery(string nameOrSqlTxt, object inputParam)
        {
            return SetExecuteNonQuery(nameOrSqlTxt, inputParam, CommandKind.SqlTextWithParams);
        }
        public static int SetExecuteNonQuery(string nameOrSqlTxt, object inputParam, CommandKind cmdKind)
        {
            using (DataContext context = new DataContext(false))
            {
                return SetExecuteNonQuery(nameOrSqlTxt, inputParam, cmdKind, context);
            }
        }
        public static int SetExecuteNonQuery(string nameOrSqlTxt, object inputParam, DataContext context)
        {
            return SetExecuteNonQuery(nameOrSqlTxt, inputParam, CommandKind.SqlTextWithParams, context);
        }
        public static int SetExecuteNonQuery(string nameOrSqlTxt, object inputParam, CommandKind cmdKind, DataContext context)
        {
            if (context == null)
            {
                throw new Exception("没有sqlcontext");
            }
            if (string.IsNullOrEmpty(nameOrSqlTxt))
            {
                throw new Exception("没有sql语句");
            }
            SetCommand(nameOrSqlTxt, inputParam, cmdKind, context);
            return context.ExecuteNonQuery();
        }

        /// <summary>
        /// // 为Command对象 初始化
        /// </summary>
        private static void SetCommand(string nameOrSqlTxt, object inputParam, CommandKind cmdKind, DataContext context)
        {
            SetCommand(nameOrSqlTxt, inputParam, cmdKind, context, null);
        }
        // SunQuery 专用
        private static void SetCommand(string nameOrSqlTxt, object inputParam, CommandKind cmdKind, DataContext context, string prefixParam)
        {
            switch (cmdKind)
            {
                case CommandKind.SpOrXml:
                    break;
                case CommandKind.SqlTextNoParams:
                    context.CreateCommand(nameOrSqlTxt, CommandType.Text);
                    break;
                case CommandKind.SqlTextWithParams:
                    context.CreateCommand(nameOrSqlTxt, CommandType.Text);
                    SetCommandParamter(inputParam, context, prefixParam);
                    break;
                case CommandKind.StoreProcedure:
                    break;
                case CommandKind.XmlCommand:
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// // 此方法为 上面的SetCommand的 补充
        /// </summary>
        private static void SetCommandParamter(object inputParams, DataContext context, string prefixParam)
        {
            Type type = inputParams.GetType();
            DbType dbType;
            object propValue = null;
            string preFixParam = string.Empty;

            if (string.IsNullOrEmpty(prefixParam))
            {
                prefixParam = string.Empty;
            }
            if ((context == null) || (context.Connection == null) || (context.CurrentCommand == null))
            {
                throw new Exception("setting2 error");
            }
            if (inputParams != null)
            {
                IDictionary dictionary = inputParams as IDictionary;
                if (dictionary != null)
                {
                    foreach (DictionaryEntry entry in dictionary)
                    {
                        context.AddParameterWithValue(entry.Key.ToString(), entry.Value);
                    }
                }
                else
                {
                    foreach (PropertyInfo info in type.GetProperties())
                    {
                        propValue = info.GetValue(inputParams, null);
                        if (propValue != null)
                        {
                            dbType = GetDbType(propValue.GetType().Name);
                            // 拼接param参数
                            context.AddParameter(prefixParam + info.Name, propValue, dbType);
                        }
                    }
                }
            }
        }

        private static MethodInfo __fillListMethod;
        private static MethodInfo GetFillListMethod(DataContext context)
        {
            if (__fillListMethod == null)
            {
                __fillListMethod = context.GetType().GetMethod("FillList");
            }
            return __fillListMethod;
        }
        private static MethodInfo GetGenericFillListMethod(DataContext context, Type type)
        {
            MethodInfo _methodInfo = GetFillListMethod(context);
            if (_methodInfo != null)
            {
                return _methodInfo.MakeGenericMethod(type);
            }
            return null;
        }



    }


}
