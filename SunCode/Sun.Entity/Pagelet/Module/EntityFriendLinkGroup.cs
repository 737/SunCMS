using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sun.Entity.Pagelet
{
    public class EntityFriendLinkGroup
    {
        [Handler(false)]
        public int? id { get; set; }

        public string subject { get; set; }
        public string description { get; set; }

    }
}
