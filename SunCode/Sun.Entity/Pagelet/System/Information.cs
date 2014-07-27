using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sun.Entity.Pagelet
{
    /// <summary>
    /// //xml -- > information.config
    /// </summary>
    [Serializable]
    public class Information
    {
        /*** site ***/
        public string BaseHost { get; set; }
        public string Beian { get; set; }
        public string CopyRight { get; set; }
        public string KeyWord { get; set; }
        public string WebName { get; set; }
        public string Description { get; set; }
        public string IndexUrl { get; set; }
        public string ComName { get; set; }
        public string ComTel { get; set; }
        public string ComAddress { get; set; }
        /*** /site ***/

        /*** core ***/
        public string AdminEmail { get; set; }
        public string Symbol { get; set; }
        public bool DelUpload { get; set; }
        public bool Rewrite { get; set; }
        public bool Recycled { get; set; }
        public string SMTPServer { get; set; }
        public string SMTPPort { get; set; }
        public string SMTPEmail { get; set; }
        public string SMTPUser { get; set; }
        public string SMTPCode { get; set; }
        /*** /core ***/

        /*** core ***/
        public string NailWidth { get; set; }
        public string NailHeight { get; set; }
        public string IMGType { get; set; }
        public string SoftType { get; set; }
        public string MediaType { get; set; }
        public string SaveStyle { get; set; }
        public string AlbumWidth { get; set; }
        public string AlbumRow { get; set; }
        public string AlbumCol { get; set; }
        public string AlbumPageSize { get; set; }
        public string AlbumStyle { get; set; }
        public string AlbumNailWidth { get; set; }
        public bool NailFill { get; set; }
        public string NailbgColor { get; set; }
        public string UserMaxFace { get; set; }
        public bool UpNailCut { get; set; }
        public bool IMGMark { get; set; }
        /*** /core ***/

        /*** member ***/
        public bool MemOpen { get; set; }
        public string MemForbid { get; set; }
        public string MemMinID { get; set; }
        public string MemMinCode { get; set; }
        public bool MemCheckEmial { get; set; }
        public bool MemOpenReg { get; set; }
        /*** /member ***/

        /*** performance ***/
        public string ListSize { get; set; }
        public string CacheFactor { get; set; }
        public string TaskRate { get; set; }
        public string TaskThread { get; set; }
        /*** /performance ***/

        /*** other ***/
        public string MaxSummary { get; set; }
        public bool IMGLocal { get; set; }
        public bool DelLink { get; set; }
        public bool AutoKeyword { get; set; }
        public string MaxTitle { get; set; }
        public string DutyAdmin { get; set; }
        public string RollClick { get; set; }
        /*** /performance ***/
    }
}
