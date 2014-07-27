using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Sun.Data.SqlServer
{
    /// <summary>
    /// // sql连接 基本类__初始化链接字符,adapter等
    /// </summary>
    public class Base : IDisposable
    {
        protected SqlDataAdapter _dataAdapter;
        protected SqlConnection _dataConnection;
        protected SqlCommand _sqlCommand;
        protected SqlTransaction _sqlTransaction;
        /// <summary>
        /// //构造_初始化 SqlConnection,,SqlDataAdapter
        /// </summary>
        public Base()
        {
            this._dataConnection = new SqlConnection(Setting.ConnectionString);
            this._dataAdapter = new SqlDataAdapter();
        }
        /// <summary>
        /// //构造_用传入的值 初始化 SqlConnection ,,默认 SqlDataAdapter
        /// </summary>
        public Base(SqlConnection conn)
        {
            this._dataConnection = conn;
            this._dataAdapter = new SqlDataAdapter();
        }

        public void BeginTransaction(IsolationLevel isolationLevel, string transactionName)
        {
            this._sqlTransaction = this._dataConnection.BeginTransaction(isolationLevel, transactionName);
            this._sqlCommand = new SqlCommand();
            this._sqlCommand.Connection = this._dataConnection;
            this._dataConnection.Open();
        }

        public void CommitTransaction()
        {
            this._sqlTransaction.Commit();
            this._sqlTransaction.Dispose();
            this._sqlCommand.Dispose();
            this._dataConnection.Close();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._dataAdapter != null)
                {
                    this._dataAdapter.Dispose();
                }
                if (this._dataConnection != null)
                {
                    this._dataConnection.Dispose();
                }
                if (this._sqlTransaction != null)
                {
                    this._sqlTransaction.Dispose();
                }
                if (this._sqlCommand != null)
                {
                    this._sqlCommand.Dispose();
                }
            }
        }
    }
}
