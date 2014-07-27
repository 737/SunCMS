using System;
using System.Collections.Specialized;
using System.Text;

namespace Sun.Data.SqlServer
{
    /// <summary>
    /// //只有一个方法,,left outer join 组合多表
    /// </summary>
    public class SqlTables : NameValueCollection
    {
        /// <summary>
        /// ///组合多表 left outer join 
        /// </summary>
        internal string ToSqlText()
        {
            string strResult = string.Empty;
            foreach (string eachStr in this)
            {
                string str3 = strResult;
                strResult = str3 + " LEFT OUTER JOIN  " + eachStr + " on " + base[eachStr];
            }
            return strResult;
        }

    }
}
