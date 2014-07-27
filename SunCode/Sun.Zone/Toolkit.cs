using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sun.Zone
{
    public class Toolkit
    {
        public static string FormatUrl(string url)
        {
            string[] turl = url.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            turl[0] = "/" + turl[0];
            turl[turl.Length - 2] = "Api" + turl[turl.Length - 2];

            url = string.Join("/", turl);

            return url;
        }
    }
}
