using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sun.Entity
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class HandlerAttribute : Attribute
    {
        public HandlerAttribute() { }
        public HandlerAttribute(bool isIgnored)
        {
            this.IsIgnored = isIgnored;
        }

        /// <summary>
        /// // 表示忽略此属性
        /// </summary>
        public bool IsIgnored { get; set; }
    }
}
