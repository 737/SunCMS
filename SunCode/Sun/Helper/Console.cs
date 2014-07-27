using System;
using System.Collections.Generic;
using System.Text;

namespace Sun
{
    public class Console
    {
        public static void Alert(object sText)
        {
            string alert = "<script type='text/javascript'>alert({0})\")</script>";
            alert = string.Format(alert, sText.ToString());
            StrWriter(alert.ToString());
        }

        public static void Log(object sText)
        {
            string console = "<script type='text/javascript'>console.log(\"{0}\")</script>";
            console = string.Format(console, sText.ToString());
            StrWriter(console.ToString());
        }

        public static void WriterBR(object sText)
        {
            string txt = "<br />\"{0}\"<br />";
            txt = string.Format(txt, sText.ToString());
            StrWriter(txt.ToString());
        }

        public static void Writer(object sText)
        {
            StrWriter(sText.ToString());
        }

        private static void StrWriter(string sTxt)
        {
            Util.Context.CurrentHttpContext.Response.Write(sTxt.ToString());
        }

    }
}
