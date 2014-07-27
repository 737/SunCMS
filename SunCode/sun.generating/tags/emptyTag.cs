using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sun.generating.tags
{
    public class emptyTag : iInterpreter
    {

        public void addAttribute(string name, string value)
        {
            throw new NotImplementedException();
        }

        public string render()
        {
            throw new NotImplementedException();
        }

        public int index
        {
            get { throw new NotImplementedException(); }
        }

        public string expression
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

        public object context
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


        public template contextTemplate
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


        parser iInterpreter.context
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


        public object getCurrentData()
        {
            throw new NotImplementedException();
        }


        public object getData()
        {
            throw new NotImplementedException();
        }


        public object data
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
