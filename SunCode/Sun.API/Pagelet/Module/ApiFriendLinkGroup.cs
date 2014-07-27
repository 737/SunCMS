using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sun.ViewModel.Pagelet;
using Sun.Entity.Pagelet;
using Sun.Zone;
using Sun.Hubble;

namespace Sun.API.Pagelet
{
    public class ApiFriendLinkGroup
    {
        private EntityInfo entityInfo = Entity.EntityHelper.GetNew().FindEntity("FriendLinkGroup");

        private EntityFriendLinkGroup GetFillEntityByQuery()
        {
            return Sun.HelperContext.GetFillEntity<EntityFriendLinkGroup>();
        }
        private int RestAction(CrudType crud)
        {
            var _fillEntity = GetFillEntityByQuery();
            SunQuery _query = new SunQuery(entityInfo.MappingName, crud, _fillEntity.GetType(), _fillEntity);
            int _result = Sun.Hubble.DataHelper.SetExecuteNonQuery(_query);

            return _result;
        }

        [Action(Verb = "GET")]
        public string Retrieve()
        {
            string jsonData = "";


            if (entityInfo != null)
            {
                var groupData = Sun.Data.PageletDataHelper.DbSelect<EntityFriendLinkGroup>(
                    entityInfo.MappingName,
                    GetFillEntityByQuery()
                );

                jsonData = Toolkit.JSON.GetPackJSON(true, new { friendLinkGroup = groupData });
            }
            else
            {
                jsonData = Toolkit.JSON.GetPackJSON(false, "没有找到对应的 Entity 信息");
            }

            return jsonData;
        }

        [Action(Verb = "POST")]
        public string Update()
        {
            var _result = RestAction(CrudType.Update);

            if (_result == 1)
            {
                return Toolkit.JSON.GetPackJSON(true, "update sucess");
            }
            return Toolkit.JSON.GetPackJSON(false, "更新失败，请检查数据");
        }

        [Action(Verb = "POST")]
        public string Create()
        {
            var _result = RestAction(CrudType.Insert);

            if (_result == 1)
            {
                return Toolkit.JSON.GetPackJSON(true, "add sucess");
            }
            return Toolkit.JSON.GetPackJSON(false, "insert fail");
        }

        [Action(Verb = "GET")]
        public string Remove()
        {
            var _result = RestAction(CrudType.Delete);

            if (_result == 1)
            {
                return Toolkit.JSON.GetPackJSON(true, "delete sucess");
            }
            return Toolkit.JSON.GetPackJSON(false, "delete fail");
        }



    }
}
