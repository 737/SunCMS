using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sun.API
{
    /// <summary>
    /// //内部类 将对象封闭成SunCMS JSON 的风格
    /// </summary>
    public class PackJSON
    {
        public PackJSON() { }

        public PackJSON(bool bState)
        {
            this.State = bState;
        }

        public PackJSON(bool bState, object oData)
        {
            this.State = bState;
            this.Data = oData;
        }

        public PackJSON(bool bState, string sMessageDetail)
        {
            this.State = bState;
            this.MessageDetail = sMessageDetail;
        }

        public PackJSON(bool bState, string sMessageDetail, object oData)
        {
            this.State = bState;
            this.Data = oData;
            this.MessageDetail = sMessageDetail;
        }

        /// <summary>
        /// //tag the true/false
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// //message detail
        /// </summary>
        public string MessageDetail { get; set; }

        /// <summary>
        /// //
        /// </summary>
        public object Data { get; set; }

    }
}
