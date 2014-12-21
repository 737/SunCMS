using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Sun;

namespace Sun.HtmlEngine.Tags
{
    public class TagCreater
    {
        private Type tagType;

        public TagCreater(string tagKey)
        {
            string[] strArray = tagKey.Split(new char[] { ':' });
            string nameSpace, tag;

            if (strArray.Length != 2)
            {
                throw new Exception("语法错误" + tagKey);
            }
            nameSpace = strArray[0].ToLower();
            tag = strArray[1].ToLower();

            if (nameSpace == "sg")
            {
                this.tagType = Type.GetType("sun.generating.tags." + tag, true, true);
            }
            else
            {
                Sun.tagEntity tagEnt = Sun.configTag.getTag(nameSpace);
                if (tagEnt != null)
                {
                    try
                    {
                        string typeName = string.Format("{0}.{2},{1}", tagEnt.nameSpace, tagEnt.assembly, tag);
                        this.tagType = Type.GetType(typeName, true, true);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }

        }


        public ITag createParser(string attributes, string innerHtml)
        {
            if (this.tagType == null)
            {
                return null;
            }

            var tagsInterpreter = this.tagType.Assembly.CreateInstance(this.tagType.FullName, true, System.Reflection.BindingFlags.CreateInstance, null, new object[] { attributes, innerHtml }, null, null);

            return (ITag)tagsInterpreter;
        }
    }
}
