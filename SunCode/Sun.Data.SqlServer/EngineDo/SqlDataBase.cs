using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Sun.Data.SqlServer
{
    public class SqlDataBase : IDisposable
    {
        private SqlConnection dataConnection;

        public SqlDataBase()
        {
            dataConnection = new SqlConnection(Setting.ConnectionString);
        }

        public SqlDataBase(SqlConnection conn)
        {
            this.dataConnection = conn;
        }

        /// <summary>
        /// //执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。如果结果集为空，则为空引用
        /// </summary>
        public object ExecuteScalar(string sql)
        {
            object obj;
            if (this.dataConnection.State == ConnectionState.Closed)
            {
                this.dataConnection.Open();
            }
            try
            {
                using (SqlCommand command = new SqlCommand(sql, this.dataConnection))
                {
                    obj = command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (this.dataConnection.State == ConnectionState.Closed)
                {
                    this.dataConnection.Close();
                }
            }
            return obj;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(true);
        }

        /// <summary>
        /// //释放资源
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing == true && (this.dataConnection != null))
            {
                this.dataConnection.Close();
            }
        }

        public SqlDataReader GetDataReader(string sql)
        {
            SqlDataReader reader;
            if (this.dataConnection.State != ConnectionState.Open)
            {
                this.dataConnection.Open();
            }
            try
            {
                using (SqlCommand command = new SqlCommand(sql, this.dataConnection))
                {
                    reader = command.ExecuteReader();
                }
            }
            catch (Exception exception)
            {
                this.dataConnection.Close();
                throw exception;
            }
            return reader;
        }
    }
}
