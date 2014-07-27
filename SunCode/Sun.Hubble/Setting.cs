using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using ClownFish;
using System.IO;
using System.Web;

namespace Sun.Hubble
{
    /// <summary>
    /// //管理 数据库连接字符窜
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// //返回当前所用数据库的版本信息
        /// </summary>
        //public static string sql_version(SqlConnection conn)
        //{
        //    string sql = "select  @@version";
        //    return new SqlDataBase(conn).ExecuteScalar(sql).ToString();
        //}

        public static void Init()
        {
            // 注册SQLSERVER数据库连接字符串
            DbContext.RegisterDbConnectionInfo("suncmssql", "System.Data.SqlClient", "@", ConnectionString);

            // 启动自动编译数据实体加载器的工作模式。
            // 编译的触发条件：请求实体加载器超过2000次，或者，等待编译的类型数量超过100次
            BuildManager.StartAutoCompile(() => BuildManager.RequestCount > 2000 || BuildManager.WaitTypesCount > 100);

            BuildManager.OnBuildException += new BuildExceptionHandler(BuildManager_OnBuildException);
        }

        //public static void AppInit2()
        //{
        //    ConnectionStringSettings appSetting = ConfigurationManager.ConnectionStrings["sunDbStr"];
        //    string connectStr = appSetting.ConnectionString;
        //    ClownFish.DbContext.RegisterDbConnectionInfo("sunDbStr", appSetting.ProviderName, "@", appSetting.ConnectionString);

        //    Type[] types = ClownFish.BuildManager.FindModelTypesFromCurrentApplication(x => x.Namespace == "ClownFishTest");
        //    ClownFish.BuildManager.CompileModelTypesSync(types, true);
        //}

        static void BuildManager_OnBuildException(Exception ex)
        {
            CompileException ce = ex as CompileException;
            if (ce != null)
                SafeLogException(ce.GetDetailMessages());
            else
                // 未知的异常类型
                SafeLogException(ex.ToString());
        }
        public static void SafeLogException(string message)
        {
            try
            {
                string logfilePath = Path.Combine(HttpRuntime.AppDomainAppPath, @"App_Data\ErrorLog.txt");

                File.AppendAllText(logfilePath, "=========================================\r\n" + message, System.Text.Encoding.UTF8);
            }
            catch { }
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
