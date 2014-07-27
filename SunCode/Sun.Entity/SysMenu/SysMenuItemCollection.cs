using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sun.Entity
{
    public class SysMenuItemCollection : CollectionBase
    {
        public void AddItem(SysMenuItem oItem)
        {
            if (!this.IsContain(oItem))
            {
                base.List.Add(oItem);
            }
        }

        public bool IsContain(SysMenuItem oChild)
        {
            return base.List.Contains(oChild);
        }
    }
}
