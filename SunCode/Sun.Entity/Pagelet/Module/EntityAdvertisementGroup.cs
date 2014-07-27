using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sun.Entity.Pagelet
{
    public class EntityAdvertisementGroup
    {
        [Handler(IsIgnored = true)]
        public int? id { get; set; }

        public string subject { get; set; }

        public string description { get; set; }

    }
}
