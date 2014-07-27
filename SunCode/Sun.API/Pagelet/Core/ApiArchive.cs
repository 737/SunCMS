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
    public class ApiArchive
    {
        private EntityInfo entInfo = Entity.EntityHelper.GetNew().FindEntity("Archive");

        private List<EntityArchive> GetList()
        {
            var fillEntity = Sun.HelperContext.GetFillEntity<EntityArchive>();
            var flData = Sun.Data.PageletDataHelper.DbSelect<EntityArchive>(entInfo.MappingName, fillEntity);

            return flData;
        }
        private int RestAction(CrudType crud)
        {
            var _fillEntity = Sun.HelperContext.GetFillEntity<EntityArchive>();
            SunQuery _query = new SunQuery(entInfo.MappingName, crud, _fillEntity.GetType(), _fillEntity);

            return Sun.Hubble.DataHelper.SetExecuteNonQuery(_query);
        }
        private EntityArchive setTagsAndDescription(EntityArchive _fillEntity)
        {
            //取得tags
            if (_fillEntity.keyword.Trim().Length < 1)
            {
                _fillEntity.keyword = API.Util.ApiPanGu.getStrTags(_fillEntity.body, 7);
            }
            if (_fillEntity.tag.Trim().Length < 1)
            {
                _fillEntity.tag = API.Util.ApiPanGu.getStrTags(_fillEntity.body, 7);
            }
            //取得 description
            if ((_fillEntity.description.Trim().Length < 1) && (_fillEntity.body.Length > 1))
            {
                var _text = Sun.Util.String.ClearHtml(_fillEntity.body).Trim();
                _text = Sun.Util.String.ClearSpace(_text);

                _fillEntity.description = Sun.Util.String.SubString(_text, 100, "");
            }

            return _fillEntity;
        }

        public List<EntityArchive> getArchiveWithSubChannels(List<EntityArchive> _archiveList, List<EntityChannel> allChannels, EntityChannel channel)
        {
            var fillEntity = new EntityArchive() { groupID = channel.channelId };

            _archiveList.AddRange(Sun.Data.PageletDataHelper.DbSelect<EntityArchive>(entInfo.MappingName, fillEntity));

            //给archive的groupName 赋值
            foreach (var archiveItem in _archiveList)
            {
                foreach (var channelItem in allChannels)
                {
                    if (archiveItem.groupID == channelItem.channelId)
                    {
                        archiveItem.groupName = channelItem.subject;
                    }
                }
            }

            foreach (var item in channel.children)
            {
                this.getArchiveWithSubChannels(_archiveList, allChannels, item);
            }

            return _archiveList;
        }

        public EntityArchive getArchive()
        {
            var fillEntity = Sun.HelperContext.GetFillEntity<EntityArchive>();

            var list = Sun.Data.PageletDataHelper.DbSelect<EntityArchive>(entInfo.MappingName, fillEntity);

            return list[0];
        }
        public EntityArchive getArchiveById(int id)
        {
            if (id  < 0)
            {
                return null;
            }
            var ent = new EntityArchive() { id = id };
            var list = Sun.Data.PageletDataHelper.DbSelect<EntityArchive>(entInfo.MappingName, ent);

            if (list.Count < 1)
            {
                return null;
            }
            return list[0];
        }

        /// <summary>
        /// // for tag
        public List<EntityArchive> getListByGroupId(int gid)
        {
            return this.getListByGroupId(gid, false);
        }
        /// <summary>
        /// // 返回的内容是否包含此子栏目的内容
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="isWithSubList"></param>
        /// <returns></returns>
        public List<EntityArchive> getListByGroupId(int gid, bool isWithSubList)
        {
            var fillEntity = new EntityArchive() { groupID = gid };
            var flData = new List<EntityArchive>();
            if (isWithSubList)
            {
                ApiChannel apiChannel = new ApiChannel();

                EntityChannel currentChannel = apiChannel.getChannelWithChildrenById(gid);
                List<EntityChannel> allChannels = apiChannel.getAllChannels();
                flData = this.getArchiveWithSubChannels(flData, allChannels, currentChannel);
            }
            else
            {
                flData = Sun.Data.PageletDataHelper.DbSelect<EntityArchive>(entInfo.MappingName, fillEntity);
            }


            return flData;
        }

        [Action(Verb = "GET")]
        public string Retrieve()
        {
            ApiChannel apiChannel = new ApiChannel();
            var fillEntity = Sun.HelperContext.GetFillEntity<EntityArchive>();

            object result;

            if (fillEntity.groupID != null)
            {
                EntityChannel channel = apiChannel.getChannelWithChildrenById(fillEntity.groupID);

                List<EntityChannel> _channelList = apiChannel.getAllChannels();
                List<EntityArchive> _archiveList = new List<EntityArchive>() { };

                result = this.getArchiveWithSubChannels(_archiveList, _channelList, channel);
            }
            else
            {
                result = Sun.Data.PageletDataHelper.DbSelect<EntityArchive>(entInfo.MappingName, fillEntity);
            }

            return Toolkit.JSON.GetPackJSON(true, new { archive = result });
        }

        /// <summary>
        /// //更新数据
        /// </summary>
        [Action(Verb = "POST")]
        public string Update()
        {
            var _fillEntity = Sun.HelperContext.GetFillEntity<EntityArchive>();

            _fillEntity = this.setTagsAndDescription(_fillEntity);

            SunQuery _query = new SunQuery(entInfo.MappingName, CrudType.Update, _fillEntity.GetType(), _fillEntity);

            var result = Sun.Hubble.DataHelper.SetExecuteNonQuery(_query);

            if (result == 1)
            {
                return Toolkit.JSON.GetPackJSON(true, "update sucess");
            }
            return Toolkit.JSON.GetPackJSON(false, "update fail");
        }

        [Action(Verb = "POST")]
        public string Create()
        {
            var _fillEntity = Sun.HelperContext.GetFillEntity<EntityArchive>();

            _fillEntity = this.setTagsAndDescription(_fillEntity);

            SunQuery _query = new SunQuery(entInfo.MappingName, CrudType.Insert, _fillEntity.GetType(), _fillEntity);

            var result = Sun.Hubble.DataHelper.SetExecuteNonQuery(_query);

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