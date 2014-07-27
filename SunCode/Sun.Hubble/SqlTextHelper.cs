using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Sun.Entity;

namespace Sun.Hubble
{
    /// <summary>
    /// // 生成对应的SQL 语句
    /// </summary>
    public class SqlTextHelper
    {
        private static string[] ignoreIdNames = { "id", "channelId" };

        /// <summary>
        /// // 返回 条件部分
        /// </summary>
        private static string GetSelectSqlTerm(string tableName, object fillEntity, string prefixParam)
        {
            if (fillEntity == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(prefixParam))
            {
                prefixParam = string.Empty;
            }

            PropertyInfo[] listProp = fillEntity.GetType().GetProperties();
            string termStyle = "";
            StringBuilder sqlTerm = new StringBuilder();
            object TT_obj;

            foreach (PropertyInfo info in listProp)
            {
                TT_obj = (info.GetValue(fillEntity, null));
                if ((info.CanRead) && (TT_obj != null))
                {
                    //如果是 对象是泛型整形，如 int?  且默认等于 -1 则忽略
                    if (Sun.Toolkit.Parse.ToInt(TT_obj) == -1)
                    {
                        continue;
                    }
                    // 如果是主题，使用模糊查询
                    if (info.Name.Equals("Subject", StringComparison.CurrentCultureIgnoreCase | StringComparison.InvariantCulture))
                    {
                        termStyle = " and {0}.{1} like '%'+{2}{3}{1}+'%'";
                    }
                    else
                    {
                        termStyle = " and ({0}.{1}={2}{3}{1}) ";
                    }
                    termStyle = string.Format(termStyle, tableName, info.Name, "@", prefixParam);
                    sqlTerm.Append(termStyle);
                }
            }
            return sqlTerm.ToString();
        }
        private static string GetOtherSqlTerm(string tableName, string prefixParam)
        {
            string term = " ({0}.{1} = {2}{3}{1}) ";

            return string.Format(term, tableName, "id", "@", prefixParam);
        }
        private static Sun.Entity.HandlerAttribute GetHandlerAttribute(object obj)
        {
            Sun.Entity.HandlerAttribute attr = null;

            try
            {
                attr = obj as Sun.Entity.HandlerAttribute;
            }
            catch { }

            return attr;
        }

        // 插入 SQL语句
        private static string GetFormatCreateSqlTxt(string sqlTableName, object fillEntity, string prefixParam)
        {
            string sql = "insert into {0} ({1}) values({2})";
            List<SqlColumn> columnList = SqlColumnHelper.GetFillSqlColumn(sqlTableName);
            PropertyInfo[] entityPropList = fillEntity.GetType().GetProperties();
            object TT_obj;
            bool isFirst = true;
            StringBuilder sqlTxtColum = new StringBuilder(), sqlTxtValue = new StringBuilder();

            foreach (PropertyInfo info in entityPropList)
            {
                var handerAttr = info.GetCustomAttributes(false);

                TT_obj = (info.GetValue(fillEntity, null));

                if ((info.CanRead) && (TT_obj != null))
                {
                    //如果是 对象是泛型整形，如 int?  且默认等于 -1 则忽略
                    if (Sun.Toolkit.HelperType.IsGeneric(info.GetType()))
                    {
                        if (Sun.Toolkit.Parse.ToInt(TT_obj) == -1)
                        {
                            continue;
                        }
                    }
                    // 如果 是 id 则跳过
                    if (string.Compare("id", info.Name, true) == 0)
                    {
                        continue;
                    }
                    //如果是被忽略的属性
                    if (handerAttr.Length > 0)
                    {
                        Sun.Entity.HandlerAttribute attr = null;
                        for (int i = 0; i < handerAttr.Length; i++)
                        {
                            attr = GetHandlerAttribute(handerAttr[i]);
                        }
                        if (attr == null || attr.IsIgnored == true)
                        {
                            continue;
                        }
                    }
                    if (isFirst)
                    {
                        sqlTxtColum.AppendFormat("{0}.{1}", sqlTableName, Util.parseSqlRetain(info.Name));
                        sqlTxtValue.AppendFormat("{0}{1}{2}", "@", prefixParam, info.Name);
                        isFirst = false;
                    }
                    else
                    {
                        sqlTxtColum.AppendFormat(",{0}.{1}", sqlTableName, Util.parseSqlRetain(info.Name));
                        sqlTxtValue.AppendFormat(",{0}{1}{2}", "@", prefixParam, info.Name);
                    }
                }
            }
            return string.Format(sql, sqlTableName, sqlTxtColum.ToString(), sqlTxtValue.ToString());
        }
        // 删除 SQL语句
        private static string GetFormatDeleteSqlTxt(string sqlTableName, string sqlTerm)
        {
            string sql = "DELETE FROM {0} WHERE {1}";
            return string.Format(sql, sqlTableName, sqlTerm);
        }
        // 查询 SQL语句
        private static string GetFormatRetrieveSqlTxt(string sqlTableName, string sqlField, string sqlTerm, Dictionary<string, string> sqlOrderBy)
        {
            string sql = "select {1} from {0} where (1=1) {2} {3}";
            if (string.IsNullOrEmpty(sqlTableName))
            {
                sql = null;
            }
            if (string.IsNullOrEmpty(sqlField))
            {
                sqlField = "*";
            }
            if (string.IsNullOrEmpty(sqlTerm))
            {
                sqlTerm = "";
            }

            string orderby = "order by ";
            if (sqlOrderBy != null)
            {
                bool isFirst = true;
                foreach (KeyValuePair<string, string> item in sqlOrderBy)
                {
                    if (isFirst)
                    {
                        orderby += string.Format("{0}.{1} {2}", sqlTableName, item.Key, item.Value);
                        isFirst = false;
                    }
                    else
                    {
                        orderby += string.Format(",{0}.{1} {2}", sqlTableName, item.Key, item.Value);
                    }
                }
            }
            else
            {
                //orderby = "order by " + sqlTableName + ".id desc";
                orderby = "";
            }

            return string.Format(sql, sqlTableName, sqlField, sqlTerm, orderby);
        }
        // 更新 SQL语句
        private static string GetFormatUpdateSqlTxt(string sqlTableName, object fillEntity, string prefixParam, string sqlTerm)
        {
            string sql = "update {0} set {1} where (1=1) and {2}";
            List<SqlColumn> columnList = SqlColumnHelper.GetFillSqlColumn(sqlTableName);

            PropertyInfo[] listProp = fillEntity.GetType().GetProperties();
            object TT_obj;
            string sqlTxt = "";
            bool isFirst = true;

            foreach (PropertyInfo info in listProp)
            {
                var handerAttr = info.GetCustomAttributes(false);

                TT_obj = (info.GetValue(fillEntity, null));

                if ((info.CanRead) && (TT_obj != null))
                {
                    //如果是 对象是泛型整形，如 int?  且默认等于 -1 则忽略
                    if (Sun.Toolkit.HelperType.IsGeneric(info.GetType()))
                    {
                        if (Sun.Toolkit.Parse.ToInt(TT_obj) == -1)
                        {
                            continue;
                        }
                    }
                    //如果是被忽略的属性
                    if (handerAttr.Length > 0)
                    {
                        foreach (var ignoreIdNameItem in ignoreIdNames)
                        {
                            if (string.Compare(ignoreIdNameItem, info.Name, true) == 0)
                            {
                                string term = " ({0}.{1} = {2}{3}{1}) ";

                                sqlTerm = string.Format(term, sqlTableName, ignoreIdNameItem, "@", prefixParam);
                            }
                        }

                        Sun.Entity.HandlerAttribute attr = null;
                        for (int i = 0; i < handerAttr.Length; i++)
                        {
                            attr = GetHandlerAttribute(handerAttr[i]);
                        }
                        if (attr == null || attr.IsIgnored == true)
                        {
                            continue;
                        }
                    }

                    if (string.Compare("id", info.Name, true) == 0)
                    {
                        continue;
                    }
                    string cc = string.Format(" {0}.{1}={2}{3}{4} ", sqlTableName, Util.parseSqlRetain(info.Name), "@", prefixParam, info.Name);
                    if (isFirst)
                    {
                        sqlTxt = cc;
                        isFirst = false;
                    }
                    else
                    {
                        sqlTxt = sqlTxt + " , " + cc;
                    }
                }
            }
            return string.Format(sql, sqlTableName, sqlTxt, sqlTerm);
        }

        public static string GetSqlTextWithNoParam(string tableName)
        {
            return GetSqlTextWithParam(tableName, CrudType.Select, null, null);
        }
        public static string GetSqlTextWithParam(string tableName, CrudType crud, object fillEntity, string prefixParam)
        {
            Dictionary<string, string> orderby = new Dictionary<string, string>();
            orderby = null;
            //orderby.Add("id", "desc");
            string sqlTxt = "", tiaojian = "";

            switch (crud)
            {
                case CrudType.Insert:
                    sqlTxt = GetFormatCreateSqlTxt(tableName, fillEntity, prefixParam);
                    break;
                case CrudType.Delete:
                    tiaojian = GetOtherSqlTerm(tableName, prefixParam);
                    sqlTxt = GetFormatDeleteSqlTxt(tableName, tiaojian);
                    break;
                case CrudType.Update:
                    tiaojian = ""; //GetOtherSqlTerm(tableName, prefixParam);
                    sqlTxt = GetFormatUpdateSqlTxt(tableName, fillEntity, prefixParam, tiaojian);
                    break;
                case CrudType.Select:
                    if (fillEntity == null)
                    {
                        //select {1} from {0} where (1=1)
                        sqlTxt = GetFormatRetrieveSqlTxt(tableName, SqlColumnHelper.GetTableField(tableName), null, null);
                    }
                    else
                    {
                        //select {1} from {0} where (1=1) {2} {3}
                        sqlTxt = GetFormatRetrieveSqlTxt(tableName, SqlColumnHelper.GetTableField(tableName), GetSelectSqlTerm(tableName, fillEntity, prefixParam), orderby);
                    }
                    break;
                default:
                    break;
            }

            return sqlTxt;
        }
    }
}
