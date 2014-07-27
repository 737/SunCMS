using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Sun.Data.SqlServer
{
    public class SqlHelper
    {
        private SqlPort _port;

        public SqlHelper(string tableName)
        {
            try
            {
                this._port = new SqlPort(tableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SqlDataReader GetDataReader(string sql, params string[] orderSort)
        {
            SqlDataReader dataReader;
            try
            {
                dataReader = this._port.GetDataReader(null, null);
            }
            catch (Exception)
            {

                throw;
            }

            return dataReader;
        }

    }
}
