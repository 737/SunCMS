using System;
using System.Collections.Generic;
using System.Text;

namespace Sun.SunControl
{
    /// <summary>
    /// //controls 接口
    /// </summary>
    public interface ISunControl
    {
        //void SetParam(object obj);

        object Value { get; set; }
    }
}
