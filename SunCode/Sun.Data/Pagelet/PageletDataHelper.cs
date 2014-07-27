using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sun.Hubble;

namespace Sun.Data
{
    public class PageletDataHelper
    {
        public static IList GetData(PageletQuery pageletQuery)
        {
            Entity.Pagelet.EntityInfo entityInfo = pageletQuery.GetEntityInfo();
            SunQuery query = new SunQuery(entityInfo.MappingName, CrudType.Select, entityInfo.EntityType, pageletQuery.GetFillEntity());

            return DataHelper.GetFillListEntity(query);
        }

        public static IList GetData(string MappingName, Type EntityType, object obj)
        {

            SunQuery query = new SunQuery(MappingName, CrudType.Select, EntityType, obj);

            return DataHelper.GetFillListEntity(query);
        }

        public static List<T> GetFillList<T>(string MappingName, Type EntityType, object obj) where T : class,new()
        {
            SunQuery query = new SunQuery(MappingName, CrudType.Select, EntityType, obj);
            return DataHelper.GetFillList<T>(query);
        }

        /// <summary>
        /// // 主力
        /// </summary>
        public static List<T> DbSelect<T>(string MappingName) where T : class,new()
        {
            return DbSelect<T>(MappingName, null);
        }
        public static List<T> DbSelect<T>(string MappingName, object obj) where T : class,new()
        {
            Type EntityType = typeof(T);

            SunQuery query = new SunQuery(MappingName, CrudType.Select, EntityType, obj);
            return DataHelper.GetFillList<T>(query);
        }

        public static object DbUpdate<T>(string MappingName, object obj) where T : class,new()
        {
            Type EntityType = typeof(T);

            SunQuery query = new SunQuery(MappingName, CrudType.Update, EntityType, obj);
            //return query; // DataHelper.GetFillList<T>(query);
            //return obj;//SqlColumnHelper.GetFillSqlColumn(MappingName);

            return DataHelper.SetExecuteNonQuery(query);
        }
    }
}
