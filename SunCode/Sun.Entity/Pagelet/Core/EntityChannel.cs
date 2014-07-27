using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sun.Entity.Pagelet
{
    public class EntityChannel
    {
        private List<EntityChannel> _children = null;

        [Handler(IsIgnored = true)]
        public int? channelId { get; set; }
        public int? parentId { get; set; }
        public int? channelType { get; set; }
        public bool? isHidden { get; set; }
        public string code { get; set; }
        public string subject { get; set; }
        public string contentPath { get; set; }
        public string index { get; set; }
        public string templateIndex { get; set; }
        public string templateList { get; set; }
        public string templateBody { get; set; }
        public string bodyRule { get; set; }
        public string listRule { get; set; }
        public string seo { get; set; }
        public string keywords { get; set; }
        public string description { get; set; }
        public string body { get; set; }
        public int? sort { get; set; }

        [Handler(IsIgnored = true)]
        public int? count { get; set; }
        [Handler(IsIgnored = true)]
        public int? subCount { get; set; }

        [Handler(IsIgnored = true)]
        public int? total
        {
            get
            {
                return this.count + this.subCount;
            }
        }

        [Handler(IsIgnored = true)]
        public List<EntityChannel> children
        {
            get
            {
                return this._children;
            }
            set
            {
                this._children = value;
            }
        }

        /// <summary>
        /// // 给 children字段增加对象
        /// </summary>
        public void addChild(EntityChannel entChannel)
        {
            if (this._children == null)
            {
                this._children = new List<EntityChannel>() { };
            }
            this._children.Add(entChannel);
        }


    }
}
