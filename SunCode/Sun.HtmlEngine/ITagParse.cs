using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sun.HtmlEngine
{
    public interface ITagParse
    {
        string expresstion { set; get; }

        string render();
    }
}
