using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Sun.Data.SqlServer
{
    /// <summary>
    /// //从 表名 得到的每一个 Column 
    /// </summary>
    internal class SqlColumnHelper : Base
    {
        private string __columnName;
        private string __dataType;
        private string __defaultValue;
        private string __description;
        private string __id;
        private string __identity;
        private string __key;
        private string __null;
        private string __precision;
        private string __scale;
        private string __size;
        private string __tableName;
        private SqlCommand SelectCommand;
        /// <summary>
        /// //下面这句话的意思是,,根据 表名 得到此表的所有字段
        /// </summary>
        private static string sqlStr = "\r\nSELECT\r\n a.colid as 'id',a.name 'Name',\r\n COLUMNPROPERTY( a.id,a.name,'IsIdentity') 'Identity',\r\n (case when (SELECT count(*)\r\n FROM sysobjects\r\n WHERE (name in\r\n           (SELECT name\r\n          FROM sysindexes\r\n          WHERE (id = a.id) AND (indid in\r\n                    (SELECT indid\r\n                   FROM sysindexkeys\r\n                   WHERE (id = a.id) AND (colid in\r\n                             (SELECT colid\r\n                            FROM syscolumns\r\n                            WHERE (id = a.id) AND (name = a.name))))))) AND\r\n        (xtype = 'PK'))>0 then 1 else 0 end) 'PK',\r\n b.name 'Type',\r\n a.length 'length',\r\n COLUMNPROPERTY(a.id,a.name,'PRECISION') as 'PRECISION',\r\n isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0) as 'Scale',\r\n a.isnullable 'Null',\r\n isnull(e.text,'') 'default',\r\n isnull(g.[value],'') AS 'description' \r\n FROM  syscolumns  a left join systypes b\r\non  a.xtype=b.xusertype\r\ninner join sysobjects d\r\non a.id=d.id  and  d.xtype='U' and  d.name=@tablename\r\nleft join syscomments e\r\non a.cdefault=e.id\r\nleft join {0} g\r\non a.id=g.{1} AND a.colid = g.{2} \r\norder by object_name(a.id),a.colorder";
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

        /// <summary>
        /// //构造 _初始化本类中的字段
        /// </summary>
        public SqlColumnHelper()
        {
            this.__tableName = "Fields";
            this.__columnName = "Name";
            this.__key = "PK";
            this.__identity = "Identity";
            this.__dataType = "Type";
            this.__size = "Length";
            this.__precision = "precision";
            this.__scale = "Scale";
            this.__null = "Null";
            this.__defaultValue = "default";
            this.__description = "description";
            this.__id = "ID";
            base._dataAdapter.TableMappings.Add("Table", this.__tableName);
        }

        public SqlColumnHelper(SqlConnection conn): base(conn)
        {
            this.__tableName = "Fields";
            this.__columnName = "Name";
            this.__key = "PK";
            this.__identity = "Identity";
            this.__dataType = "Type";
            this.__size = "Length";
            this.__precision = "precision";
            this.__scale = "Scale";
            this.__null = "Null";
            this.__defaultValue = "default";
            this.__description = "description";
            this.__id = "ID";
            base._dataAdapter.TableMappings.Add("Table", this.__tableName);
        }        

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.SelectCommand != null))
            {
                this.SelectCommand.Dispose();
            }
            base.Dispose(disposing);
        }
        
        /// <summary>
        /// //将 表名 的所有字段存储到SqlColumnCollection中
        /// </summary>
        public SqlColumnCollection GetColumn(string tableName)
        {
            SqlCommand selectCommand = this.GetSelectCommand();
            selectCommand.Parameters["@tablename"].Value = tableName;

            base._dataAdapter.SelectCommand = selectCommand;        // base._dataAdapter 是重点

            DataTable dataTable = this.CreateTable();               //create a table
            base._dataAdapter.Fill(dataTable);

            SqlColumnCollection columnsCollection = new SqlColumnCollection();
            foreach (DataRow row in dataTable.Rows)
            {
                columnsCollection.Insert(this.FillModel(row, tableName));    
            }
            dataTable.Dispose();
            dataTable = null;
            return columnsCollection;
        }

        /// <summary>
        /// //返回 创建的datatable
        /// </summary>
        private DataTable CreateTable()
        {
            DataTable table = new DataTable(this.__tableName);
            DataColumnCollection columns = table.Columns;
            columns.Add(this.__id, typeof(int));
            columns.Add(this.__columnName, typeof(string));
            columns.Add(this.__key, typeof(bool));
            columns.Add(this.__identity, typeof(bool));
            columns.Add(this.__dataType, typeof(string));
            columns.Add(this.__size, typeof(int));
            columns.Add(this.__scale, typeof(int));
            columns.Add(this.__precision, typeof(int));
            columns.Add(this.__null, typeof(bool));
            columns.Add(this.__defaultValue, typeof(string));
            columns.Add(this.__description, typeof(string));
            return table;
        }
        /// <summary>
        /// //根据 DataRow和表名,,将SqlColumn赋值,并将其返回
        /// </summary>
        private SqlColumn FillModel(DataRow dr, string tableName)
        {
            SqlColumn column = new SqlColumn();
            column.Name = (string)dr[this.__columnName];
            column.IsKey = (bool)dr[this.__key];
            column.IsIdentity = (bool)dr[this.__identity];
            string str = (string)dr[this.__dataType];
            if (str == "numeric")
            {
                column.DataType = SqlDbType.Float;
            }
            else
            {
                column.DataType = (SqlDbType)Enum.Parse(typeof(SqlDbType), str, true);
            }
            column.Size = (int)dr[this.__size];
            column.ID = (int)dr[this.__id];
            column.DefaultValue = (string)dr[this.__defaultValue];
            column.Description = (string)dr[this.__description];
            column.IsNull = (bool)dr[this.__null];
            column.Precision = (int)dr[this.__precision];
            column.TableName = tableName;
            return column;
        }

        /// <summary>
        /// //返回 sql语句
        /// </summary>
        private SqlCommand GetSelectCommand()
        {
            if (this.SelectCommand == null)
            {
                string commandText = this.FormatCommandText();
                this.SelectCommand = new SqlCommand(commandText, base._dataConnection);
                this.SelectCommand.Parameters.Add("@tablename", SqlDbType.NVarChar);
            }
            return this.SelectCommand;
        }

        /// <summary>
        /// //返回 格式化后的 sqlcommandtext语句
        /// </summary>
        private string FormatCommandText()
        {
            string str = "sysproperties";
            string str2 = "id";
            string str3 = "smallid";
            string sqlVersion = Setting.sql_version(base._dataConnection);
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
            return string.Format(sqlStr, str, str2, str3);
        }
    }
}
