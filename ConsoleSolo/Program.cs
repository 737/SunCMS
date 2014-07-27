using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Sun;
using Sun.Data.SqlServer;

namespace ConsoleSolo
{
    public static class cc
    {

    }

    public class Program
    {

        static void Main(string[] args)
        {

            //if (Debugger.IsAttached && Debugger.IsLogging())
            //{
            //    Program p = new Program();
            //    p.SunDebugger();
            //    Debugger.Break();
            //}


            if (Debugger.IsAttached && Debugger.IsLogging())
            {
                string txt = "null";

                txt = Setting.ConnectionString;

                Debug.WriteLine("----------  start -------------");
                Debug.WriteLine(txt);
                Debug.WriteLine("----------- end ------------");
                Debugger.Break();
            }
        }


    }
}
