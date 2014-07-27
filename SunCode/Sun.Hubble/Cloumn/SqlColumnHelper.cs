using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Sun.Hubble
{
    /// <summary>
    /// //从 表名 得到的每一个 Column 
    /// </summary>
    public class SqlColumnHelper
    {
        

        /// <summary>
        /// //下面这句话的意思是,,根据 表名 得到此表的所有字段
        /// </summary>
        private static string _sqlStr = "\r\nSELECT\r\n a.colid as 'id',a.name 'Name',\r\n COLUMNPROPERTY( a.id,a.name,'IsIdentity') 'Identity',\r\n (case when (SELECT count(*)\r\n FROM sysobjects\r\n WHERE (name in\r\n           (SELECT name\r\n          FROM sysindexes\r\n          WHERE (id = a.id) AND (indid in\r\n                    (SELECT indid\r\n                   FROM sysindexkeys\r\n                   WHERE (id = a.id) AND (colid in\r\n                             (SELECT colid\r\n                            FROM syscolumns\r\n                            WHERE (id = a.id) AND (name = a.name))))))) AND\r\n        (xtype = 'PK'))>0 then 1 else 0 end) 'PK',\r\n b.name 'Type',\r\n a.length 'length',\r\n COLUMNPROPERTY(a.id,a.name,'PRECISION') as 'PRECISION',\r\n isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0) as 'Scale',\r\n a.isnullable 'Null',\r\n isnull(e.text,'') 'default',\r\n isnull(g.[value],'') AS 'description' \r\n FROM  syscolumns  a left join systypes b\r\non  a.xtype=b.xusertype\r\ninner join sysobjects d\r\non a.id=d.id  and  d.xtype='U' and  d.name=@tablename\r\nleft join syscomments e\r\non a.cdefault=e.id\r\nleft join {0} g\r\non a.id=g.{1} AND a.colid = g.{2} \r\norder by object_name(a.id),a.colorder";
        //sqlStr的格式化结果如下   【注： line62就是{0} line68就是{1} {2}    on a.id=g.minor_id AND a.colid = g.minor_id 】
        //
        //      declare @tablename NVarChar(50)
        //      set @tablename = 'sun_Product'

        //      SELECT
        //       a.colid as 'id',a.name 'Name',
        //       COLUMNPROPERTY( a.id,a.name,'IsIdentity') 'Identity',
        //       (case when (SELECT count(*)
        //       FROM sysobjects
        //       WHERE (name in
        //                 (SELECT name
        //                FROM sysindexes
        //                WHERE (id = a.id) AND (indid in
        //                          (SELECT indid
        //                         FROM sysindexkeys
        //                         WHERE (id = a.id) AND (colid in
        //                                   (SELECT colid
        //                                  FROM syscolumns
        //                                  WHERE (id = a.id) AND (name = a.name))))))) AND
        //              (xtype = 'PK'))>0 then 1 else 0 end) 'PK',
        //       b.name 'Type',
        //       a.length 'length',
        //       COLUMNPROPERTY(a.id,a.name,'PRECISION') as 'PRECISION',
        //       isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0) as 'Scale',
        //       a.isnullable 'Null',
        //       isnull(e.text,'') 'default',
        //       isnull(g.[value],'') AS 'description' 
        //       FROM  syscolumns  a left join systypes b
        //      on  a.xtype=b.xusertype
        //      inner join sysobjects d
        //      on a.id=d.id  and  d.xtype='U' and  d.name= +@tablename
        //      left join syscomments e
        //      on a.cdefault=e.id
        //      left join sys.extended_properties g
        //      on a.id=g.minor_id AND a.colid = g.minor_id 
        //      order by object_name(a.id),a.colorder

        public static List<SqlColumn> GetFillSqlColumn(string tableName)
        {
            string sqlTxt = GetCommandText();
            SqlColumn cloumn = new SqlColumn() { TableName = tableName };
            List<SqlColumn> list = DataHelper.GetFillList<SqlColumn>(sqlTxt, cloumn, CommandKind.SqlTextWithParams);

            return list;
        }

        /// <summary>
        /// // 返回 查询 字段部分
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string GetTableField(string tableName)
        {
            StringBuilder sbFiled = new StringBuilder();
            List<SqlColumn> list = GetFillSqlColumn(tableName);
            bool isFirst = true;
            var filed = "";

            foreach (var item in list)
            {
                filed = Util.parseSqlRetain(item.Name);

                if (isFirst)
                {
                    sbFiled.Append(tableName + "." + filed);
                    isFirst = false;
                }
                else
                {
                    sbFiled.Append("," + tableName + "." + filed);
                }
            }
            return sbFiled.ToString();
        }

        /// <summary>
        /// //返回 格式化后的 sql语句
        /// </summary>
        public static string GetCommandText()
        {
            string str = "sysproperties";
            string str2 = "id";
            string str3 = "smallid";
            string sqlVersion = GetSqlVersion();
            if
            (
                (sqlVersion.IndexOf("Microsoft SQL Server 2005", StringComparison.OrdinalIgnoreCase) > -1)
                || (sqlVersion.IndexOf("Microsoft SQL Server 2008", StringComparison.OrdinalIgnoreCase) > -1)
            )
            {
                str = "sys.extended_properties";
                str2 = "minor_id";
                str3 = "minor_id";
            }
            return string.Format(_sqlStr, str, str2, str3);
        }

        /// <summary>
        /// //返回当前所用数据库的版本信息
        /// </summary>
        public static string GetSqlVersion()
        {
            string sql = "select  @@version";

            return DataHelper.GetFillScalarList<string>(sql)[0].ToString();

        }
    }
}
