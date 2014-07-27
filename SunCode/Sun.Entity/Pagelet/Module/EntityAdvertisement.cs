using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sun.Entity.Pagelet
{
    public class EntityAdvertisement
    {

        [Handler(IsIgnored = true)]
        public int? id { get; set; }

        public string subject { get; set; }

        public string siteUrl { get; set; }

        public string detail { get; set; }

        public bool? isEnable { get; set; }

        public string groupID { get; set; }

        [Handler(IsIgnored = true)]
        public string groupName { get; set; }

        public string startTime { get; set; }
    }
}