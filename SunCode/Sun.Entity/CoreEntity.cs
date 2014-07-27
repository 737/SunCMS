using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Sun.Entity.Pagelet;
using Sun;

namespace Sun.Entity
{
    [Serializable, XmlRoot("CoreEntity")]
    public class CoreEntity
    {
        private List<EntityInfo> __item;
        [XmlArray("Item")]
        public List<EntityInfo> Item
        {
            get
            {
                return this.__item;
            }
            set
            {
                this.__item = value;
            }
        }


        public static CoreEntity GetNew()
        {
            return (ConfigHelper.GetConfig(typeof(CoreEntity), "entity.config") as CoreEntity);
        }

        public EntityInfo FindEntity(string code)
        {
            if (this.Item == null)
            {
                this.Item = GetNew().Item;
            }

            foreach (EntityInfo ent in this.Item)
            {
                if (ent.Code.ToLower() == code.ToLower())
                {
                    return ent;
                }
            }
            return null;
        }
    }
}
