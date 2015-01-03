using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sun.Toolkit
{
    public class Date
    {
        public static DateTime ToDateTime(object o)
        {
            return ToDateTime(o, DateTime.MinValue);
        }
        public static DateTime ToDateTime(object o, DateTime def)
        {
            if (o == null)
            {
                return def;
            }
            try
            {
                DateTime time;
                if (DateTime.TryParse(o.ToString(), out time))
                {
                    return time;
                }
                return def;
            }
            catch
            {
                return def;
            }
        }

        public static string formatTime(DateTime date, string style)
        {
            if (date.GetType() != typeof(DateTime))
            {
                date = DateTime.Now;
            }

            return date.ToString(style);
        }
        public static string formatTime(string date, string style)
        {
            var dateTime = ToDateTime(date);

            return formatTime(dateTime, style);
        }

    }
}
