using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sun.Entity.Pagelet;
using Sun.Zone;
using Sun.Hubble;

namespace Sun.API.Pagelet
{
    public class ApiAdvertisementGroup
    {
        private EntityInfo entInfo = Entity.EntityHelper.GetNew().FindEntity("AdvertisementGroup");

        public List<EntityAdvertisementGroup> GetList()
        {
            var filledEnt = Sun.HelperContext.GetFillEntity<EntityAdvertisementGroup>();
            SunQuery query = new SunQuery(entInfo.MappingName, CrudType.Select, entInfo.EntityType, filledEnt);

            return Sun.Hubble.DataHelper.GetFillList<EntityAdvertisementGroup>(query);
        }
        private int RestAction(CrudType crud)
        {
            var _fillEntity = Sun.HelperContext.GetFillEntity<EntityAdvertisementGroup>();
            SunQuery _query = new SunQuery(entInfo.MappingName, crud, _fillEntity.GetType(), _fillEntity);

            return  Sun.Hubble.DataHelper.SetExecuteNonQuery(_query);
        }

        [Action(Verb = "GET")]
        public string Retrieve()
        {
            List<EntityAdvertisementGroup> _list = this.GetList();

            return Toolkit.JSON.GetPackJSON(true, new { advertisementGroup = _list });
        }

        [Action(Verb = "POST")]
        public string Update()
        {
            var result = RestAction(CrudType.Update);

            if (result == 1)
            {
                return Toolkit.JSON.GetPackJSON(true, "update sucess");
            }
            return Toolkit.JSON.GetPackJSON(false, "update fail");
        }

        [Action(Verb = "POST")]
        public string Create()
        {
            var result = RestAction(CrudType.Insert);

            if (result == 1)
            {
                return Toolkit.JSON.GetPackJSON(true, "insert sucess");
            }
            return Toolkit.JSON.GetPackJSON(false, "insert fail");
        }

        [Action(Verb = "GET")]
        public string Remove()
        {
            var result = RestAction(CrudType.Delete);

            if (result == 1)
            {
                return Toolkit.JSON.GetPackJSON(true, "delete sucess");
            }
            return Toolkit.JSON.GetPackJSON(false, "delete fail");
        }

    }
}
