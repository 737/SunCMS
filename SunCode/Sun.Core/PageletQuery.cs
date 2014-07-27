using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Sun.Entity.Pagelet;

namespace Sun.Core
{
    /// <summary>
    /// //请求信息
    /// </summary>
    public class PageletQuery
    {
        public PageletQuery(NameValueCollection querystring)
        {
            this.__queryInfo = querystring;
        }

        private EntityInfo __entityInfo;
        public EntityInfo GetEntityInfo()
        {
            if (this.__entityInfo == null)
            {
                this.__entityInfo = Sun.Entity.CoreEntity.GetNew().FindNode(this.GetPageletName()[1]);
            }
            return this.__entityInfo;
        }

        private List<string> __pageletName = null;
        public List<string> GetPageletName()
        {
            if (this.__pageletName == null)
            {
                this.__pageletName = new List<string>(this.GetQueryInfo()["page"].Split('.'));
            }
            return this.__pageletName;
        }

        private NameValueCollection __queryInfo;
        public NameValueCollection GetQueryInfo()
        {
            if (this.__queryInfo != null)
            {
                return this.__queryInfo;
            }
            return null;
        }
    }
}
