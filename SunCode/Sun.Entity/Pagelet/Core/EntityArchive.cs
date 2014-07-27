using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sun.Entity.Pagelet
{
    public class EntityArchive
    {
        [Handler(IsIgnored = true)]
        public int? id { get; set; }
        public string subject { get; set; }
        public string shortSubject { get; set; }
        public string tag { get; set; }
        public bool? isRebuild { get; set; }
        public int? authority { get; set; }
        public int? clicked { get; set; }
        public string source { get; set; }
        public string author { get; set; }
        public string time { get; set; }
        public int? groupID { get; set; }

        [Handler(IsIgnored = true)]
        public string groupName { get; set; }

        public int? sort { get; set; }
        public string color { get; set; }
        public string keyword { get; set; }
        public string description { get; set; }
        public string body { get; set; }
    }
}