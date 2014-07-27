using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Sun.Data.SqlServer
{
    /// <summary>
    /// //管理 数据库连接字符窜
    /// </summary>
    class Setting
    {
        /// <summary>
        /// //返回当前所用数据库的版本信息
        /// </summary>
        public static string sql_version(SqlConnection conn)
        {
            string sql = "select  @@version";
            return new SqlDataBase(conn).ExecuteScalar(sql).ToString();
        }

        /// <summary>
        /// //返回web.config中的数据库连接字符窜
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                string str = System.Configuration.ConfigurationManager.AppSettings["connectionStringKey"];
                if (string.IsNullOrEmpty(str))
                {
                    str = "sunsql";
                }
                return System.Configuration.ConfigurationManager.ConnectionStrings[str].ConnectionString;
            }
        }
    }
}
