using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Sun
{
    /// <summary>
    /// //xml -- > systemConfig.config
    /// </summary>
    [Serializable]
    public class configSystem
    {
        public static entitySystem getConfig()
        {
            return ConfigHelper.getConfig<entitySystem>("systemConfig.config");
        }

    }
}
