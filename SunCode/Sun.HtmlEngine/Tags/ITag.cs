using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sun.HtmlEngine.Tags
{
    public interface ITag
    {
        string expresstion { set; get; }

        string render();
    }
}
