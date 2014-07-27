using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sun.Hubble
{
    public class SqlColumn
    {
        public Int16? ID { get; set; }
        public string Name { get; set; }
        public Int32? Identity { get; set; }
        public Int32? PK { get; set; }
        public string Type { get; set; }
        public Int16? Length { get; set; }
        public Int32? Precision { get; set; }
        public Int32? Scale { get; set; }
        public Int32? Null { get; set; }
        public string Default { get; set; }
        public string Description { get; set; }
        public string TableName { get; set; }
    }
}
