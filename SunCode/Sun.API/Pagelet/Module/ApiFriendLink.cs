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
    /// <summary>
    /// //返回系统主菜单
    /// </summary>
    public class ApiFriendLink
    {
        private static EntityInfo einfo = Entity.EntityHelper.GetNew().FindEntity("Friendlink");

        private static List<EntityFriendLink> GetEntityFriendLink()
        {
            var fillEntity = Sun.HelperContext.GetFillEntity<EntityFriendLink>();
            var flData = Sun.Data.PageletDataHelper.DbSelect<EntityFriendLink>(einfo.MappingName, fillEntity);

            return flData;
        }
        private static List<EntityFriendLinkGroup> GetEntityFriendLinkGroup()
        {
            EntityInfo groupInfo = Entity.EntityHelper.GetNew().FindEntity("FriendLinkGroup");

            var groupData = Sun.Data.PageletDataHelper.DbSelect<EntityFriendLinkGroup>(
                groupInfo.MappingName
                );

            return groupData;
        }

        [Action]
        public static string Retrieve()
        {
            List<EntityFriendLink> _listFL = GetEntityFriendLink();
            List<EntityFriendLinkGroup> _listFLGroup = GetEntityFriendLinkGroup();
            EntityFriendLinkGroup group = null;

            foreach (var item in _listFL)
            {
                group = _listFLGroup.Find(
                delegate(EntityFriendLinkGroup group1)
                {
                    return group1.id == item.groupID;
                });

                item.groupName = group == null ? null : group.subject;
            }

            return Toolkit.JSON.GetPackJSON(true, new { friendLink = _listFL, friendLinkGroup = _listFLGroup });
        }

        /// <summary>
        /// //更新数据
        /// </summary>
        [Action(Verb = "POST")]
        public static string Update()
        {
            EntityFriendLink fillEntity = Sun.HelperContext.GetFillEntity<EntityFriendLink>();

            SunQuery query = new SunQuery(einfo.MappingName, CrudType.Update, fillEntity.GetType(), fillEntity);

            var result = Hubble.DataHelper.SetExecuteNonQuery(query);

            if (result == 1)
            {
                return Toolkit.JSON.GetPackJSON(true, query.FillEntity);
            }
            return Toolkit.JSON.GetPackJSON(false, "更新失败，请检查数据", query.FillEntity);
        }

        [Action(Verb = "POST")]
        public static string Create()
        {
            EntityFriendLink fillEntity = Sun.HelperContext.GetFillEntity<EntityFriendLink>();

            SunQuery query = new SunQuery(einfo.MappingName, CrudType.Insert, fillEntity.GetType(), fillEntity);

            var result = Hubble.DataHelper.SetExecuteNonQuery(query);
            if (result == 1)
            {
                return Toolkit.JSON.GetPackJSON(true, query.FillEntity);
            }
            return Toolkit.JSON.GetPackJSON(false, "增加数据失败，请检查数据", query.FillEntity);
        }

        [Action]
        public static string Remove()
        {
            EntityFriendLink fillEntity = Sun.HelperContext.GetFillEntity<EntityFriendLink>();

            SunQuery query = new SunQuery(einfo.MappingName, CrudType.Delete, fillEntity.GetType(), fillEntity);

            var result = Hubble.DataHelper.SetExecuteNonQuery(query);
            if (result == 1)
            {
                return Toolkit.JSON.GetPackJSON(true, "delete sucess");
            }
            return Toolkit.JSON.GetPackJSON(false, "更新失败，请检查数据", query.FillEntity);
        }

    }



}
