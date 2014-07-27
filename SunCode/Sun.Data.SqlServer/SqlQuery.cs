    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Text.RegularExpressions;

namespace Sun.Data.SqlServer
{
    public class SqlQuery
    {
        private List<SqlParameter> sqlparam;
        private string sqlString;

        public SqlQuery(string sql)
        {
            this.sqlparam = new List<SqlParameter>();
            this.sqlString = sql;
        }


        public List<SqlParameter> SqlParam
        {
            get
            {
                return this.sqlparam;
            }
        }

        public string SqlString
        {
            get
            {
                return this.sqlString;
            }
            set
            {
                this.sqlString = value;
            }
        }
    }
}
