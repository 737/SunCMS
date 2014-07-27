using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sun.generating.tags;

namespace sun.generating
{
    public interface iInterpreter
    {
        /// <summary>
        /// // 表达示  包括外围的标签
        /// </summary>
        string expression { get; set; }
        /// <summary>
        /// //当前 解析器使用的数据
        /// </summary>
        object data { get; set; }
        parser context { get; set; }

        void addAttribute(string name, string value);

        string render();

    }
}
