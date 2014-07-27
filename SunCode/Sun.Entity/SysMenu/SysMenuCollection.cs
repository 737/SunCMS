using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sun.Entity
{
    //系统后台 菜单
    public class SysMenuCollection : CollectionBase
    {
        public void AddItem(SysMenu oChild)
        {
            if (!this.IsContain(oChild))
            {
                base.List.Add(oChild);
            }
        }

        public bool IsContain(SysMenu oChild)
        {
            return base.List.Contains(oChild);
        }
    }
}
