using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sun.Entity
{
    //系统后台 菜单
    public class SysMenu
    {
        private SysMenuItemCollection _children;

        public string Name { get; set; }

        public SysMenuItemCollection Children
        {
            get
            {
                if (_children == null)
                {
                    _children = new SysMenuItemCollection();
                }
                return _children;
            }
            set
            {
                this._children = value;
            }
        }
    }
}
