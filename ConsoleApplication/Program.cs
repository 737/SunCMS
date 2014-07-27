using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using Sun;

namespace ConsoleApplication
{
    public static  class cc
    { 
        
    }

    public class Program
    {

        static void Main(string[] args)
        {

            if (Debugger.IsAttached && Debugger.IsLogging())
            {
                Program p = new Program();
                p.SunDebugger();
                Debugger.Break();
            }
        }

        public void SunDebugger()
        {
            string cc = "\r\n<script type ='text/javascript'>  \r\n function getsta(args,context){\r\n   document.getElementById('sta'+context).innerHTML=args;\r\n}\r\nfunction loadsta(i){document.getElementById('sta'+i).innerHTML='loading...'; [DoCallback]; };\r\n</script>";

            Debug.WriteLine(cc);
        }

    }
}
