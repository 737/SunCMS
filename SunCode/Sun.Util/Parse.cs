using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Sun.Util
{
    public static class Parse
    {
        public static int ToInt(object o)
        {
            return ToInt(o, -1);
        }

        public static int ToInt(object o, int defaultValue)
        {
            if (o == null)
            {
                return defaultValue;
            }
            string input = o.ToString();
            if (((input.Length <= 0) || (input.Length > 11)) || !Regex.IsMatch(input, "^[-]?[0-9]*$"))
            {
                return defaultValue;
            }
            if (((input.Length >= 10) && ((input.Length != 10) || (input[0] != '1'))) && (((input.Length != 11) || (input[0] != '-')) || (input[1] != '1')))
            {
                return defaultValue;
            }
            return Convert.ToInt32(o);
        }

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

        public static decimal SafeParseToDecimal(string s)
        {
            decimal result = 0m;
            decimal.TryParse(s, out result);
            return result;
        }

        //---------------------------------------------------




        public static string BytesToString(byte[] bytes)
        {
            string str = string.Empty;
            if ((bytes != null) && (bytes.Length > 0))
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    str = str + bytes[i].ToString("X") + " ";
                }
            }
            return str;
        }

        public static T ChangeType<T, FT>(FT obj)
        {
            try
            {
                return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
            }
            catch
            {
                return default(T);
            }
        }

        public static byte[] HexToBytes(string str)
        {
            str = str.Trim();
            string[] strArray = Regex.Split(str, "[ ]+");
            byte[] buffer = new byte[strArray.Length];
            int index = 0;
            foreach (string str2 in strArray)
            {
                int num2 = int.Parse(str2, NumberStyles.AllowHexSpecifier);
                buffer[index] = (byte)Convert.ToInt32(num2);
                index++;
            }
            return buffer;
        }

        public static bool IsDouble(object o)
        {
            return ((o != null) && Regex.IsMatch(o.ToString(), @"^([0-9])[0-9]*(\.\w*)?$"));
        }

        public static bool IsNumeric(object o)
        {
            if (o != null)
            {
                string input = o.ToString();
                if ((((input.Length > 0) && (input.Length <= 11)) && Regex.IsMatch(input, "^[-]?[0-9]*[.]?[0-9]*$")) && (((input.Length < 10) || ((input.Length == 10) && (input[0] == '1'))) || (((input.Length == 11) && (input[0] == '-')) && (input[1] == '1'))))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsNumericArray(string[] array)
        {
            if (array == null)
            {
                return false;
            }
            if (array.Length < 1)
            {
                return false;
            }
            foreach (string str in array)
            {
                if (!IsNumeric(str))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool ToBoolean(object o, bool defaultValue)
        {
            if (o != null)
            {
                string strA = o.ToString();
                if (string.Compare(strA, "true", true) == 0)
                {
                    return true;
                }
                if (string.Compare(strA, "false", true) == 0)
                {
                    return false;
                }
                if (string.Compare(strA, "1", true) == 0)
                {
                    return true;
                }
                if (string.Compare(strA, "0", true) == 0)
                {
                    return false;
                }
            }
            return defaultValue;
        }



        public static decimal ToDecimal(object o, decimal defaultValue)
        {
            decimal num;
            if ((o != null) && decimal.TryParse(o.ToString(), out num))
            {
                return num;
            }
            return defaultValue;
        }

        public static double ToDouble(object o, double defaultValue)
        {
            double num;
            if ((o != null) && double.TryParse(o.ToString(), out num))
            {
                return num;
            }
            return defaultValue;
        }

        public static float ToFloat(object o, float defaultValue)
        {
            if (o == null)
            {
                return defaultValue;
            }
            float num = defaultValue;
            if (Regex.IsMatch(o.ToString(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
            {
                num = Convert.ToSingle(o.ToString());
            }
            return num;
        }


        

        

    }
}
