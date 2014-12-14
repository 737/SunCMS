using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sun.Entity;
using Sun.Entity.Pagelet;
using Sun.Hubble;
using Sun.Zone;

namespace Sun.API.Pagelet
{
    //TODO::: public方法可以优化一下
    public class ApiChannel
    {
        private EntityInfo entInfo = EntityHelper.GetNew().FindEntity("Channel");
        // 解析栏目的级联关系
        private List<EntityChannel> parseChannel(List<EntityChannel> father, List<EntityChannel> child, int count)
        {
            var shexiade = new List<EntityChannel>(child);
            string _sqlCountTxt = "";
            int _count = 0;

            foreach (var item in father)
            {
                // 统计本栏目下有多少条文档
                _sqlCountTxt = "SELECT     COUNT(groupID) AS archivecount  FROM  sun_archive  WHERE     (groupID = {0})";
                _sqlCountTxt = string.Format(_sqlCountTxt, item.channelId);
                try
                {
                    _count = Sun.Toolkit.Parse.ToInt(Sun.Hubble.DataHelper.GetExecuteScalar(_sqlCountTxt), 0);
                }
                catch
                {
                }
                finally
                {
                    item.count = _count;
                }

                foreach (var itemChild in child)
                {
                    if (item.channelId == itemChild.parentId)
                    {
                        item.addChild(itemChild);
                        shexiade.Remove(itemChild);
                    }
                }
            }

            foreach (var item in father)
            {
                if (item.children != null && item.children.Count > 0)
                {
                    parseChannel(item.children, shexiade, count);
                }
                else
                {
                    item.children = new List<EntityChannel>() { };
                }
            }

            return father;
        }
        // 统计 本栏目 以级 子栏目的 文档条数
        private void parseChannelArchiveCount(EntityChannel entChannel, EntityChannel upChannel)
        {
            if (entChannel.children.Count > 0)
            {
                foreach (var item in entChannel.children)
                {
                    this.parseChannelArchiveCount(item, entChannel);
                }
            }
            else
            {
                entChannel.subCount = 0;
            }
            if (upChannel != null)
            {
                upChannel.subCount = upChannel.subCount == null ? 0 : upChannel.subCount;
                upChannel.subCount = upChannel.subCount + entChannel.count + entChannel.subCount;
            }
        }
        // 对栏目的操作
        private int restAction(CrudType crud)
        {
            var _fillEnt = Sun.HelperContext.GetFillEntity<EntityChannel>();
            SunQuery query = new SunQuery(entInfo.MappingName, crud, _fillEnt.GetType(), _fillEnt);

            return Sun.Hubble.DataHelper.SetExecuteNonQuery(query);
        }

        // 返回 处理后的栏目  就是将栏目 合成 栏目管理 的格式。
        private List<EntityChannel> getDealedChannels()
        {
            List<EntityChannel> _allChannelList = this.getAllChannels();
            List<EntityChannel> father = new List<EntityChannel>() { };
            List<EntityChannel> child = new List<EntityChannel>() { };

            foreach (var item in _allChannelList)
            {
                if (item.parentId == -1)
                {
                    father.Add(item);
                }
                else
                {
                    child.Add(item);
                }
            }

            father = this.parseChannel(father, child, child.Count);

            //统计 本栏目 以级 子栏目的 文档条数
            foreach (var item in father)
            {
                this.parseChannelArchiveCount(item, null);
            }

            return father;
        }

        // 根据id返回对应栏目 分级
        private EntityChannel _filterChannelById = null;
        private EntityChannel filterChannelById(List<EntityChannel> father, int? id)
        {
            if (_filterChannelById != null)
            {
                return _filterChannelById;
            }
            else
            {
                foreach (var item in father)
                {
                    if (item.channelId == id)
                    {
                        _filterChannelById = item;
                        break;
                    }
                    else if (item.children.Count > 0)
                    {
                        this.filterChannelById(item.children, id);
                    }
                }
            }
            return _filterChannelById;
        }

        /// <summary>
        /// // 返回所有栏目 未分级
        /// </summary>
        public List<EntityChannel> getAllChannels()
        {
            SunQuery query = new SunQuery(entInfo.MappingName, CrudType.Select, entInfo.EntityType, null);

            return DataHelper.GetFillList<EntityChannel>(query);
        }
        /// <summary>
        /// // 返回此栏目以级子栏目集合 未分级
        /// </summary>
        private int __sum = 0;
        public List<EntityChannel> __getChannelsWithChildrenById(EntityChannel root, List<EntityChannel> channels)
        {
            if (__sum == 0)
            {
                channels.Add(root);
            }
            else
            {
                __sum++;
            }
            if (root.children.Count > 0)
            {
                foreach (var item in root.children)
                {
                    __getChannelsWithChildrenById(item, channels);
                }
            }
            return channels;
        }
        public List<EntityChannel> getChannelsWithChildrenById(int? channelId)
        {
            List<EntityChannel> channels = new List<EntityChannel>();
            EntityChannel cn = this.getChannelWithChildrenById(channelId);

            return this.__getChannelsWithChildrenById(cn, channels);
        }

        /// <summary>
        /// // 返回此栏目以级子栏目     分级
        /// </summary>
        public EntityChannel getChannelWithChildrenById(int? id)
        {
            List<EntityChannel> cns = this.getDealedChannels();
            id = id == null ? -1 : id;

            return this.filterChannelById(cns, id);
        }


        [Action(Verb = "GET")]
        public string retrieve()
        {
            return Toolkit.JSON.GetPackJSON(true, new { channel = this.getDealedChannels() });
        }

        [Action(Verb = "POST")]
        public string update()
        {
            var result = this.restAction(CrudType.Update);

            if (result == 1)
            {
                return Toolkit.JSON.GetPackJSON(true, "update sucess");
            }
            return Toolkit.JSON.GetPackJSON(false, "update fail");
        }

        [Action(Verb = "POST")]
        public string Create()
        {
            var result = this.restAction(CrudType.Insert);

            if (result == 1)
            {
                return Toolkit.JSON.GetPackJSON(true, "insert sucess");
            }
            return Toolkit.JSON.GetPackJSON(false, "insert fail");
        }

        [Action(Verb = "GET")]
        public string Remove()
        {
            var result = this.restAction(CrudType.Delete);

            if (result == 1)
            {
                return Toolkit.JSON.GetPackJSON(true, "delete sucess");
            }
            return Toolkit.JSON.GetPackJSON(false, "delete fail");
        }
    }
}
