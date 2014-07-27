using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Configuration;
using ClownFish;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace Sun.Hubble
{
    public class DataContext : DbContext
    {
        public DataContext(bool useTransaction) : base(useTransaction) { }

        public new void AddParameterWithValue(string paramName, object paramValue)
        {
            DbParameter parameter = base.CurrentCommand.CreateParameter();
            parameter.ParameterName = "@" + paramName;
            parameter.Value = paramValue;
            base.CurrentCommand.Parameters.Add(parameter);
        }

        //public DbParameter SetParamter(string paramName, object paramValue, DbType paramType)
        //{
        //    DbParameter parameter = this.__dbCommand123.CreateParameter();
        //    parameter.ParameterName = "@" + paramName;
        //    parameter.DbType = paramType;
        //    parameter.Direction = ParameterDirection.Input;
        //    //Nullable<Int32> size? = null;
        //    //parameter.Size = size;

        //    if (paramValue != null)
        //    {
        //        parameter.Value = paramValue;
        //    }
        //    this.__dbCommand123.Parameters.Add(parameter);
        //    return parameter;
        //}

        //public new DbCommand CreateCommand(string sqlOrName, CommandType commandType)
        //{
        //    DbCommand command = base.Connection.CreateCommand();
        //    command.CommandText = sqlOrName;
        //    command.CommandType = commandType;
        //    this.__dbCommand123 = command;
        //    return command;
        //}




    }
}
