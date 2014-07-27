using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sun.Entity.Pagelet
{
    public class EntityFriendLink
    {
        [Handler(IsIgnored = true)]
        public int? id { get; set; }

        public string subject { get; set; }

        public string siteUrl { get; set; }

        public int? sortNum { get; set; }

        public string logoUrl { get; set; }

        public string description { get; set; }

        public int? groupID { get; set; }

        public bool? isEnable { get; set; }

        //extend
        [Handler(IsIgnored = true)]
        public string groupName { get; set; }
    }
}
