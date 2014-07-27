using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Threading;
using Sun.ViewModel.Pagelet;
using Sun;
using Sun.UI;
using Sun.Zone;
using Sun.Toolkit;
using Sun.Entity.Pagelet;
using Sun.Data;
using Sun.Hubble;

namespace Sun.API.Pagelet
{
    public class ApiAdvertisement
    {
        private EntityInfo entInfo = Entity.EntityHelper.GetNew().FindEntity("Advertisement");
        private List<EntityAdvertisement> GetAd()
        {
            var fillEnt = Sun.HelperContext.GetFillEntity<EntityAdvertisement>();

            return Sun.Data.PageletDataHelper.DbSelect<EntityAdvertisement>(entInfo.MappingName, fillEnt);
        }


        [Action(Verb = "GET")]
        public string Retrieve()
        {
            List<EntityAdvertisement> _ad = this.GetAd();
            List<EntityAdvertisementGroup> _adGroup = (new ApiAdvertisementGroup()).GetList();

            return Toolkit.JSON.GetPackJSON(true, new { advertisement = _ad, advertisementGroup = _adGroup });
        }

        [Action(Verb = "POST")]
        public string Update()
        {
            var fillEnt = Sun.HelperContext.GetFillEntity<EntityAdvertisement>();

            SunQuery query = new SunQuery(entInfo.MappingName, CrudType.Update, fillEnt.GetType(), fillEnt);

            var result = Sun.Hubble.DataHelper.SetExecuteNonQuery(query);

            if (result == 1)
            {
                return Toolkit.JSON.GetPackJSON(true, query.FillEntity);
            }
            return Toolkit.JSON.GetPackJSON(false, "更新失败，请检查数据", query.FillEntity);
        }

        [Action(Verb = "POST")]
        public string Create()
        {
            var fillEnt = Sun.HelperContext.GetFillEntity<EntityAdvertisement>();

            SunQuery query = new SunQuery(entInfo.MappingName, CrudType.Insert, fillEnt.GetType(), fillEnt);

            var result = Sun.Hubble.DataHelper.SetExecuteNonQuery(query);

            if (result == 1)
            {
                return Toolkit.JSON.GetPackJSON(true, "insert sucess");
            }
            return Toolkit.JSON.GetPackJSON(false, "insert fail");
        }

        [Action(Verb = "GET")]
        public string Remove()
        {
            var fillEnt = Sun.HelperContext.GetFillEntity<EntityAdvertisement>();

            SunQuery query = new SunQuery(entInfo.MappingName, CrudType.Delete, fillEnt.GetType(), fillEnt);

            var result = Sun.Hubble.DataHelper.SetExecuteNonQuery(query);

            if (result == 1)
            {
                return Toolkit.JSON.GetPackJSON(true, "delete sucess");
            }
            return Toolkit.JSON.GetPackJSON(false, "delete fail");
        }

    }
}
