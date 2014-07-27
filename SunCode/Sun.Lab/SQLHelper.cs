/*
 * 创建人：cricket7th
 * cricket7th@sina.com.cn
 * 时间：2010-10-18 21:50
 * 说明：数据库助手类
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System.Data.OleDb;
using System.Configuration;

namespace DAL
{
    public class SQLHelper
    {
        private OleDbConnection conn = null;
        private OleDbCommand cmd = null;
        private OleDbDataReader odr = null;

        /// <summary>
        /// 返回数据库链接字符串
        /// </summary>
        /// <returns></returns>
        protected static string GetConnStr()
        {
            return System.Configuration.ConfigurationManager.AppSettings["provider"].ToString() + System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["dbPath"]) + ";";
        }

        /// <summary>
        /// 获取链接字符串
        /// </summary>
        /// <returns></returns>
        private OleDbConnection Getconn()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }

        //构造函数
        public SQLHelper()
        {
            string connStr;
            connStr = GetConnStr();
            conn = new OleDbConnection(connStr);
        }

        #region 查询数据库
        /// <summary>
        /// 带参数和不带参数的SQL语句数据库查询
        /// </summary>
        /// <param name="cmdTxt">sql命令</param>
        /// <param name="paras">参数集合</param>
        /// <returns></returns>
        public DataTable SelectData(string cmdTxt, OleDbParameter[] paras)
        {
            DataTable dt = new DataTable();
            try
            {
                cmd = new OleDbCommand(cmdTxt, Getconn());
                if (paras != null)
                {
                    cmd.Parameters.AddRange(paras);
                }
                odr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dt.Load(odr);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return dt;
        }
        #endregion

        #region 数据修改
        /// <summary>
        /// 带参数和不带参数的SQL语句数据库修改
        /// </summary>
        /// <param name="cmdTxt">SQL语句命令</param>
        /// <param name="paras">参数集合</param>
        /// <returns></returns>
        public int ModifyData(string cmdTxt, OleDbParameter[] paras)
        {
            int i;
            try
            {
                cmd = new OleDbCommand(cmdTxt, Getconn());
                if (paras != null)
                {
                    cmd.Parameters.AddRange(paras);
                }
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return i;
        }
        #endregion
    }
}
