using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sun.generating
{
    public static class templateHelper
    {
        private static string defaultTempletsPath = "~/templets/default/";

        /// <summary>
        /// // 根据模板名称，返回模板的 路径
        /// </summary>
        public static string getTemplatePath(string templateName)
        {
            if (string.IsNullOrEmpty(templateName))
            {
                return templateName;
            }
            if ((templateName[0] == '/') || (templateName[0] == '~'))
            {
                return templateName;
            }
            string path = templetsPath + templateName;
            if (!Sun.Toolkit.io.isFileExist(path))
            {
                path = defaultTempletsPath + templateName;
            }
            return path;
        }

        /// <summary>
        /// // 在generate.config文件中，模板的路径
        /// </summary>
        public static string templetsPath
        {
            get
            {
                return "~/templets/default/";
            }
        }
    }
}
