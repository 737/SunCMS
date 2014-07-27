using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace Sun.Data.SqlServer
{
    /// <summary>
    /// //对“表”操作
    /// </summary>
    public class SqlPort : Base
    {
        private SqlColumnCollection _columnCollection;
        private SqlTables _sqltables;
        private string _tableName;

        /// <summary>
        /// //得到 “表” 的column___________________
        /// //根据 参数(表名) ___将表的各字段添加到  SqlColumnCollection.cs中
        /// </summary>
        public SqlPort(string tableName)
        {
            try
            {
                this._tableName = tableName;
                base._dataAdapter.TableMappings.Add("Table", this._tableName);
                this._sqltables = new SqlTables();
                using (SqlColumnHelper helper = new SqlColumnHelper())       //得到 表 的所有字段
                {
                    this._columnCollection = helper.GetColumn(this._tableName);
                }
            }
            catch (Exception exception)
            {
                throw new Exception("数据库：" + exception.Message);
            }
        }

        

        public SqlDataReader GetDataReader( SqlQuery query, params string[] orderSort)
        {
            SqlDataReader reader;
            using (SqlCommand command = new SqlCommand("select * from sun_friendlink",base._dataConnection))
            {
                if (base._dataConnection.State == ConnectionState.Closed)
                {
                    base._dataConnection.Open();
                }

                reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            }

            return reader;
        }

        private string GetSelectSql()
        {
            string str = this._columnCollection.ToSqlText();
            return (("select " + str + " from " + this._tableName) + this._sqltables.ToSqlText());
        }



    }
}
