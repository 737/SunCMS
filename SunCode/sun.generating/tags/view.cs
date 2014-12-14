using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sun.generating.tags
{
    public class view //: iInterpreter
    {
        public view()
        { }

        public view(string exp, string parseTxt)
        {
        }

        public string render()
        {
            return "我是被render后的内容";
        }

        private int _index;

        public int index
        {
            get
            {
                return this._index;
            }
        }

        public string expression
        {
            get;
            set;
        }

        public object context
        {
            get;
            set;
        }

        public void addAttribute(string name, string value)
        {
            throw new NotImplementedException();
        }


        public Template contextTemplate
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


      
    }
}
