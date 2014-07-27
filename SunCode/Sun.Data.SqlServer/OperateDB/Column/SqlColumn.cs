using System;
using System.Collections.Generic;
using System.Data;

namespace Sun.Data.SqlServer
{
    /// <summary>
    /// //Model-->表的各字段
    /// </summary>
    public class SqlColumn
    {
        private SqlDbType dbType;
        private string defaultValue;
        private string description;
        private string function;
        private int id;
        private bool isIdentity;
        private bool isnull;
        private bool isPk;
        private string name;
        private int precision;
        private int scale;
        private int size;
        private string tableName;

        internal SqlColumn() { }

        public static object SetNull(SqlColumn columnInfo)
        {
            switch (Util.GetType(columnInfo.DataType).ToString())
            {
                case "System.Int16":
                    return Null.NullShort;

                case "System.Int32":
                case "System.Int64":
                    return Null.NullInteger;

                case "System.Single":
                    return Null.NullSingle;

                case "System.Double":
                    return Null.NullDouble;

                case "System.Decimal":
                    return Null.NullDecimal;

                case "System.DateTime":
                    return Null.NullDate;

                case "System.String":
                case "System.Char":
                    return Null.NullString;

                case "System.Boolean":
                    return Null.NullBoolean;

                case "System.Guid":
                    return Null.NullGuid;
            }
            return null;
        }
        /// <summary>
        /// //Type __字段的类型 int/varchar/....
        /// </summary>
        public SqlDbType DataType
        {
            get
            {
                return this.dbType;
            }
            internal set
            {
                this.dbType = value;
            }
        }
        /// <summary>
        /// //default__字段默认值
        /// </summary>
        public string DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
            internal set
            {
                this.defaultValue = value;
            }
        }
        /// <summary>
        /// //description___字段描述
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
            internal set
            {
                this.description = value;
            }
        }

        public string Function
        {
            get
            {
                return this.function;
            }
            set
            {
                this.function = value;
            }
        }
        /// <summary>
        /// //ID
        /// </summary>
        public int ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
        /// <summary>
        /// //Identity__1/0  1表示是自增加的
        /// </summary>
        public bool IsIdentity
        {
            get
            {
                return this.isIdentity;
            }
            internal set
            {
                this.isIdentity = value;
            }
        }
        /// <summary>
        /// //主键
        /// </summary>
        public bool IsKey
        {
            get
            {
                return this.isPk;
            }
            internal set
            {
                this.isPk = value;
            }
        }
        /// <summary>
        /// //Null__是否可以为空
        /// </summary>
        public bool IsNull
        {
            get
            {
                return this.isnull;
            }
            internal set
            {
                this.isnull = value;
            }
        }
        /// <summary>
        /// //Name  __字段的名称
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
            internal set
            {
                this.name = value;
            }
        }
        /// <summary>
        /// //字段长度
        /// </summary>
        public int Precision
        {
            get
            {
                return this.precision;
            }
            internal set
            {
                this.precision = value;
            }
        }
        /// <summary>
        /// //Scale
        /// </summary>
        public int Scale
        {
            get
            {
                return this.scale;
            }
            internal set
            {
                this.scale = value;
            }
        }
        /// <summary>
        /// Length__ 长度
        /// </summary>
        public int Size
        {
            get
            {
                return this.size;
            }
            internal set
            {
                this.size = value;
            }
        }
        /// <summary>
        /// //数据库表名  如sun_users
        /// </summary>
        public string TableName
        {
            get
            {
                return this.tableName;
            }
            internal set
            {
                this.tableName = value;
            }
        }
    }
}
