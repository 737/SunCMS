using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Sun.Util
{
    /// <summary>
    /// //JSON helper
    /// </summary>
    public class JSON
    {
        private static SunCMS_JSON_Style SunStyle;

        private static string PackData(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static string GetJSON(object obj)
        {
            return (PackData(obj));
        }

        public static string GetPackJSON(bool bState)
        {
            SunStyle = new SunCMS_JSON_Style(bState);
            return (PackData(SunStyle));
        }


        public static string GetPackJSON(bool bState, object oData)
        {
            SunStyle = new SunCMS_JSON_Style(bState, oData);
            return (PackData(SunStyle));
        }

        public static string GetPackJSON(bool bState, string sMessageDetail)
        {
            SunStyle = new SunCMS_JSON_Style(bState, sMessageDetail);
            return (PackData(SunStyle));
        }

        public static string GetPackJSON(bool bState, string sMessageDetail, object oData)
        {
            SunStyle = new SunCMS_JSON_Style(bState, sMessageDetail, oData);
            return (PackData(SunStyle));
        }
    }

    /// <summary>
    /// //内部类 将对象封闭成SunCMS JSON 的风格
    /// </summary>
    internal class SunCMS_JSON_Style
    {
        public SunCMS_JSON_Style() { }

        public SunCMS_JSON_Style(bool bState)
        {
            this.State = bState;
        }

        public SunCMS_JSON_Style(bool bState, object oData)
        {
            this.State = bState;
            this.Data = oData;
        }

        public SunCMS_JSON_Style(bool bState, string sMessageDetail)
        {
            this.State = bState;
            this.MessageDetail = sMessageDetail;
        }

        public SunCMS_JSON_Style(bool bState, string sMessageDetail, object oData)
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
