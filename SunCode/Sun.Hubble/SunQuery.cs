using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sun.Hubble
{
    /// <summary>
    /// // 此类 调用了SqlTextHelper
    /// </summary>
    public class SunQuery
    {
        public SunQuery(string tableName, CrudType crud, Type entityType)
        {
            this.InitProperty(tableName, crud, entityType, null);
        }

        /// <summary>
        /// // 主要调这个
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="fillEntity">如果没有 对象，可为 null</param>
        public SunQuery(string tableName, CrudType crud, Type entityType, object fillEntity)
        {
            this.InitProperty(tableName, crud, entityType, fillEntity);
        }
        private void InitProperty(string tableName, CrudType crud, Type entityType, object fillEntity)
        {
            this.MappingName = tableName;
            this.EntityType = entityType;

            if (fillEntity == null)
            {
                this.SqlText = SqlTextHelper.GetSqlTextWithNoParam(tableName);
            }
            else
            {
                this.FillEntity = fillEntity;
                this.SqlText = SqlTextHelper.GetSqlTextWithParam(tableName, crud, fillEntity, this.PrefixParam);
            }
        }

        public string SqlText
        {
            get;
            set;
        }

        public object FillEntity
        {
            get;
            set;
        }

        public Type EntityType
        { get; set; }

        public string PrefixParam
        {
            get
            {
                string __prefixParam = string.Empty;

                if (this.FillEntity != null)
                {
                    __prefixParam = this.MappingName + "_";
                }
                return __prefixParam;
            }
        }

        /// <summary>
        /// // 所要映射的 名称 e.g.:表名、XML文件名
        /// </summary>
        public string MappingName
        { get; set; }
    }
}
