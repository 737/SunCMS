using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Sun
{
    /// <summary>
    /// //Url重写规则的item
    /// </summary>
    [Serializable]
    public class rewriterRule
    {
        private string _lookFor;
        private string _sendTo;

        [XmlAttribute("lookFor")]
        public string lookFor
        {
            get
            {
                return this._lookFor;
            }
            set
            {
                this._lookFor = value;
            }
        }

        [XmlAttribute("sendTo")]
        public string sendTo
        {
            get
            {
                return this._sendTo;
            }
            set
            {
                this._sendTo = value;
            }
        }

    }
}
