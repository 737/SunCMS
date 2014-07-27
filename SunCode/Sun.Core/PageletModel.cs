using Sun.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.UI;
using Sun.Entity.Pagelet;

namespace Sun.Core
{
    public class PageletModel
    {
        public PageletModel(PageletQuery pageletQuery)
        {
            this.SunPageletQuery = pageletQuery;
        }

        //返回config文件类型对象
        /// <summary>
        /// //返回config文件类型对象
        /// </summary>
        /// <returns></returns>
        private object GetConfigData()
        {
            return Sun.ConfigHelper.GetConfig(this.EntityInfo.EntityType, this.EntityInfo.MappingName);
        }
        private IList GetAllData()
        {
            IList obj = null;

            switch (this.EntityInfo.DataStyle)
            {
                case EDataStyle.CONFIG:
                    //obj = this.GetConfigData();
                    break;
                case EDataStyle.DATABANK:
                    obj = PageletDataHelper.GetData(this.SunPageletQuery);
                    break;
                default:
                    obj = null;
                    break;
            }
            return obj;
        }

        public IList GetData()
        {
            //保证 此方法和本类中的 属性Entity 数据一致
            if (this.__entity != null)
            {
                //return this.__entity;
            }

            IList obj = null;
            //有些页面有没有实体对象
            if (this.EntityInfo != null)
            {
                //没有这两个数据，将entityInfo传到 取数层的时候，会引发异常
                if ((this.EntityInfo.EntityType != null) && (!string.IsNullOrEmpty(this.EntityInfo.MappingName)))
                {
                    obj = this.GetAllData();
                }
            }
            return obj;
        }
        public void SetEntity(object entity)
        {
            if (entity != null)
            {
                this.Entity = entity;
            }
        }

        private object __entity;
        /// <summary>
        /// //只做 preview测试使用
        /// </summary>
        public object Entity
        {
            get
            {
                if (this.__entity == null)
                {
                    this.__entity = this.GetData();
                }
                return this.__entity;
            }
            set
            {
                if (this.__entity != value)
                {
                    this.__entity = value;
                }
            }
        }
        public EntityInfo EntityInfo
        {
            get
            {
                return this.SunPageletQuery.GetEntityInfo();
            }
        }

        public PageletQuery SunPageletQuery { get; set; }
    }
}
