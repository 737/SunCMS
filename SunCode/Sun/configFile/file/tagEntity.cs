using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Sun
{
    public class tagEntity
    {
        public tagEntity()
        {
            this.assembly = "";
            this.nameSpace = "";
            this.tagPrefix = "";
        }

        public tagEntity(XmlNode node)
        {
            try
            {
                this.assembly = node.Attributes["assembly"].Value;
                this.nameSpace = node.Attributes["nameSpace"].Value;
                this.tagPrefix = node.Attributes["tagPrefix"].Value;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string assembly
        { get; set; }

        public string nameSpace
        { get; set; }

        public string tagPrefix
        { get; set; }
    }
}
