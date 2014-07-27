using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Sun
{
    /// <summary>
    /// //entity -- > systemConfig.config
    /// </summary>
    [Serializable]
    public class entitySystem
    {
        public string autoKeyword { get; set; }
        public int? albumWidth { get; set; }
        public int? albumRow { get; set; }
        public int? albumCol { get; set; }
        public int? albumPageSize { get; set; }
        public int? albumStyle { get; set; }
        public int? albumNailWidth { get; set; }
        public string adminEmail { get; set; }
        public string baseHost { get; set; }
        public string beian { get; set; }
        public int? cacheFactor { get; set; }
        public string copyRight { get; set; }
        public string comName { get; set; }
        public string comTel { get; set; }
        public string comAddress { get; set; }
        public string description { get; set; }
        public string dutyAdmin { get; set; }
        public bool? delUpload { get; set; }
        public bool? delLink { get; set; }
        public string indexUrl { get; set; }
        public bool? imgLocal { get; set; }
        public string imgType { get; set; }
        public bool? imgMark { get; set; }
        public string keywords { get; set; }
        public int? listSize { get; set; }
        public string webName { get; set; }
        public int? rollClick { get; set; }
        public bool? rewrite { get; set; }
        public bool? recycled { get; set; }
        public string symbol { get; set; }
        public string saveStyle { get; set; }
        public string smtpServer { get; set; }
        public int? smtpPort { get; set; }
        public string smtpEmail { get; set; }
        public string smtpUser { get; set; }
        public string smtpCode { get; set; }
        public string softType { get; set; }
        public int? taskRate { get; set; }
        public int? taskThread { get; set; }
        public int? maxSummary { get; set; }
        public int? maxTitle { get; set; }
        public bool? memOpen { get; set; }
        public string memForbid { get; set; }
        public int? memMinID { get; set; }
        public int? memMinCode { get; set; }
        public bool? memCheckEmial { get; set; }
        public bool? memOpenReg { get; set; }
        public string mediaType { get; set; }
        public int? nailWidth { get; set; }
        public int? nailHeight { get; set; }
        public bool? nailFill { get; set; }
        public int? nailbgColor { get; set; }
        public int? userMaxFace { get; set; }
        public bool? upNailCut { get; set; }

    }
}
