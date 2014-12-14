using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Sun;

namespace sun.generating.tags
{
    public class TagCreater
    {
        private Type _tagType;

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
                this._tagType = Type.GetType("sun.generating.tags." + tag, true, true);
            }
            else
            {
                Sun.tagEntity tagEnt = Sun.configTag.getTag(nameSpace);
                if (tagEnt != null)
                {
                    try
                    {
                        string typeName = string.Format("{0}.{2},{1}", tagEnt.nameSpace, tagEnt.assembly, tag);
                        this._tagType = Type.GetType(typeName, true, true);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }

        }
        public TagCreater(Type tagType)
        {
            this._tagType = tagType;
        }

        public ITagInterpreter createInterpreter(string attributes, string innerHtml)
        {
            if (this._tagType == null)
            {
                return new emptyTag();
            }


            var tagsInterpreter = this._tagType.Assembly.CreateInstance(this._tagType.FullName, true, System.Reflection.BindingFlags.CreateInstance, null, new object[] { attributes, innerHtml }, null, null);

            return (ITagInterpreter)tagsInterpreter;
        }
    }
}
