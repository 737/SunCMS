using System;
using System.Xml.Serialization;

namespace Sun.Entity.Pagelet
{
    /// <summary>
    /// //pagelet model 对象的属性信息
    /// </summary>
    [Serializable]
    public class EntityInfo
    {
        private string __mappingName;

        [XmlAttribute("code")]
        public string Code { get; set; }

        private string __assembly;
        [XmlAttribute("assembly")]
        public string Assembly
        {
            get
            {
                return this.__assembly;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.__entityType = Type.GetType(value.Trim(), false, true);
                }
                this.__assembly = value;
            }
        }

        private Type __entityType;
        [XmlIgnore]
        public Type EntityType
        {
            get
            {
                return this.__entityType;
            }
            set
            {
                this.__entityType = value;
            }
        }

        private EDataStyle __dataStyle;
        [XmlAttribute("dataStyle")]
        public EDataStyle DataStyle
        {
            get
            {
                return this.__dataStyle;
            }
            set
            {
                switch (value.ToString().Trim().ToUpper())
                {
                    case "DATABANK":
                        this.__dataStyle = EDataStyle.DATABANK;
                        break;
                    case "CONFIG":
                        this.__dataStyle = EDataStyle.CONFIG;
                        break;
                }
            }
        }

        private EBindStyle __bindStyle;
        [XmlAttribute("bindStyle")]
        public EBindStyle BindTStyle
        {
            get
            {
                return this.__bindStyle;
            }
            set
            {
                switch (value.ToString().Trim().ToUpper())
                {
                    case "LIST":
                        this.__bindStyle = EBindStyle.LIST;
                        break;
                    case "AROW":
                        this.__bindStyle = EBindStyle.AROW;
                        break;
                }
            }

        }

        [XmlAttribute("mappingName")]
        public string MappingName
        {
            get
            {
                return ("sun_" + this.__mappingName);
            }
            set
            {
                this.__mappingName = value;
            }
        }

        /// <summary>
        ///  // 实体 模块名称
        /// </summary>
        [XmlIgnore]
        public string PageletModuleName { get; set; }

        /// <summary>
        /// // 实体 名称
        /// </summary>
        [XmlIgnore]
        public string PageletName { get; set; }

        /// <summary>
        /// // 实体 行为
        /// </summary>
        [XmlIgnore]
        public Sun.EPageletAction PageletAction { get; set; }


    }

}
