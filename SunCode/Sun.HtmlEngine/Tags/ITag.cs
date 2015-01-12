using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sun.HtmlEngine.Tags
{
    public interface ITag
    {
        // 整个TAG的HTML
        string expresstion { set; get; }

        // TAG的属性
        Attributes attributes { set; get; }

        // 当前 解析器使用的数据
        object data { get; set; }

        string render();
    }
}
