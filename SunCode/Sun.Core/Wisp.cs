using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sun.Entity.Pagelet;
using Sun;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Sun.Core
{
    /// <summary>
    /// // suncms 小精灵
    /// </summary>
    public class Wisp
    {
        private List<string> __pageletQuery;
        private EntityInfo __entityInfo;
        private Sun.EPageletAction __pageletAction;

        public Wisp()
        {
            var queryDict = this.GetQueryDict();
            if ((queryDict != null) && (queryDict["sun_page"] != null))
            {
                this.__pageletQuery = new List<string>(queryDict["sun_page"].Split('.'));
            }
        }

        /// <summary>
        /// // 返回对应的 实体的信息
        /// </summary>
        public EntityInfo GetEntityInfo()
        {
            if (this.__entityInfo == null)
            {
                this.__entityInfo = Sun.Entity.CoreEntity.GetNew().FindEntity(this.GetPageletName());
                this.__entityInfo.PageletModuleName = this.GetPageletModelName();
                this.__entityInfo.PageletName = this.GetPageletName();
                this.__entityInfo.PageletAction = this.GetPageletAction();

            }
            return this.__entityInfo;
        }
        public T GetFillEntity<T>(Type type) where T : class,new()
        {
            var queryDict = this.GetQueryDict();
            T entity = new T();
            Regex re = new Regex(@"System.\w*", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            string propName, propType;
            MatchCollection propTypeCollect;

            foreach (PropertyInfo info in type.GetProperties())
            {
                object propValue = null;

                if (info.CanWrite)
                {
                    propName = info.Name.ToLower(); //转成小写 保持一致
                    if (queryDict.ContainsKey(propName))
                    {
                        propValue = queryDict[propName];
                    }
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
                                propValue = Sun.Toolkit.Parse.ToInt(propValue);
                                break;
                            case "System.DateTime":
                                propValue = Sun.Toolkit.Parse.ToDateTime(propValue);
                                break;
                            case "System.Boolean":
                                propValue = Sun.Toolkit.Parse.ToBoolean(propValue);
                                break;
                        }

                        try
                        {
                            info.SetValue(entity, propValue, null);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }

            return entity;
        }

        public string GetPageletModelName()
        {
            if (this.__pageletQuery.Count > 0)
            {
                return this.__pageletQuery[0];
            }
            return null;
        }
        public string GetPageletName()
        {
            if (this.__pageletQuery.Count > 1)
            {
                return this.__pageletQuery[1];
            }
            return null;
        }
        public EPageletAction GetPageletAction()
        {
            if (this.__pageletQuery.Count > 2)
            {
                string str = this.__pageletQuery[2].Trim().ToUpper();

                switch (str)
                {
                    case "INSERT":
                        this.__pageletAction = EPageletAction.INSERT;
                        break;
                    case "DELETE":
                        this.__pageletAction = EPageletAction.DELETE;
                        break;
                    case "UPDATE":
                        this.__pageletAction = EPageletAction.UPDATE;
                        break;
                    case "SELECT":
                    default:
                        this.__pageletAction = EPageletAction.SELECT;
                        break;
                }
            }
            else
            {
                this.__pageletAction = EPageletAction.SELECT;
            }

            return this.__pageletAction;
        }

        public Dictionary<string, string> GetQueryDict()
        {
            return Sun.Toolkit.context.GetQueryDict();
        }
    }
}
