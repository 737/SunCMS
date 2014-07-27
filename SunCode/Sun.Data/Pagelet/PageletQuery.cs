using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Sun.Entity.Pagelet;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Sun.Data
{
    /// <summary>
    /// //请求信息
    /// </summary>
    public class PageletQuery
    {
        public object _fillEntity;
        private EntityInfo __entityInfo;

        public PageletQuery(NameValueCollection querystring)
        {
            this.__queryInfo = querystring;
        }

        /// <summary>
        /// // 返回对应的 实体的信息
        /// </summary>
        public EntityInfo GetEntityInfo()
        {
            if (this.__entityInfo == null)
            {
                this.__entityInfo = Sun.Entity.CoreEntity.GetNew().FindEntity(this.GetPageletName()[1]);
            }
            return this.__entityInfo;
        }

        /// <summary>
        /// // 返回对应的 实体对象
        /// </summary>
        public object GetFillEntity()
        {
            if (this._fillEntity == null)
            {
                Type enitityType = this.GetEntityInfo().EntityType;
                object entityObj = Activator.CreateInstance(enitityType);
                Regex re = new Regex(@"System.\w*", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);

                string propName, propType;
                object propValue;
                MatchCollection propTypeCollect;

                foreach (PropertyInfo info in enitityType.GetProperties())
                {
                    if (info.CanWrite)
                    {
                        propName = info.Name;
                        propValue = this.GetQueryInfo()[propName];

                        if (propValue != null)
                        {
                            propType = info.PropertyType.FullName;
                            propTypeCollect = re.Matches(propType);
                            propType = propTypeCollect.Count > 1 ? propTypeCollect[1].ToString() : propTypeCollect[0].ToString();

                            switch (propType)
                            {
                                case "System.Int":
                                case "System.Int16":
                                case "System.Int32":
                                    propValue = Sun.Util.Parse.ToInt(propValue.ToString());
                                    break;
                                case "System.DateTime":
                                    propValue = Sun.Util.Parse.ToDateTime(propValue.ToString());
                                    break;
                            }

                            try
                            {
                                info.SetValue(entityObj, propValue, null);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                }

                this._fillEntity = entityObj;
            }

            return this._fillEntity;
        }

        /// <summary>
        /// // 页面行为
        /// </summary>
        public PageletAction GetPageletAction()
        {
            PageletAction action = PageletAction.Select;
            if (this.GetPageletName().Count == 3)
            {
                switch (this.GetPageletName()[2].ToLower().Trim())
                {
                    case "edit":
                        action = PageletAction.Edit;
                        break;
                    case "delete":
                        action = PageletAction.Delete;
                        break;
                    case "update":
                        action = PageletAction.Update;
                        break;
                }
            }
            return action;
        }

        private List<string> __pageletName = null;
        public List<string> GetPageletName()
        {
            if (this.__pageletName == null)
            {
                this.__pageletName = new List<string>(this.GetQueryInfo()["sun_page"].Split('.'));
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
