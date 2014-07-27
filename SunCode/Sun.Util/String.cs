using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Web;
using System.Threading;

namespace Sun.Util
{
    /// <summary>
    /// //SunCMS 字符串助手
    /// </summary>
    public static class String
    {
        /// <summary>
        /// //首字母大写
        /// </summary>
        /// <param name="text">字符窜</param>
        /// <returns></returns>
        public static string ToTitleCase(string text)
        {
            if (text != null)
            {
                text = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text);
            }
            return text;
        }

        /// <summary>
        /// //MD5加密
        /// </summary>
        public static string MD5(string text)
        {
            byte[] bytes = Encoding.Default.GetBytes(text);
            bytes = new MD5CryptoServiceProvider().ComputeHash(bytes);
            string str2 = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str2 = str2 + bytes[i].ToString("x").PadLeft(2, '0');
            }
            return str2;
        }

        /// <summary>
        /// //返回清除html代码的串，（注，将所有的html代码替换为了&nbsp;）
        /// </summary>        
        public static string ClearHtml(string htmlText)
        {
            return Regex.Replace(Regex.Replace(htmlText, "<[^>]+>", ""), "&nbsp;", "", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        /// <summary>
        /// //返回清除所有空格
        /// </summary>  
        public static string ClearSpace(string text)
        { 
            return Regex.Replace(text, @"\s*","", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase| RegexOptions.Multiline);
        }

        /// <summary>
        /// //裁剪字符窜
        /// </summary>
        /// <param name="validate">字符窜</param>        
        /// <param name="length">裁剪长度</param>
        /// <param name="addEndText">末尾追加内容</param>
        /// <returns></returns>
        public static string SubString(string validate, int length, string addEndText)
        {
            return SubString(validate, 0, length, addEndText);
        }

        /// <summary>
        /// //裁剪字符窜
        /// </summary>
        /// <param name="validate">字符窜</param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="length">裁剪长度</param>
        /// <param name="addEndText">末尾追加内容</param>
        /// <returns></returns>
        public static string SubString(string validate, int startIndex, int length, string addEndText)
        {
            if (length < 0)
            {
                return validate;
            }
            validate = TrimEndBreak(validate);
            string str = validate;
            byte[] bytes = Encoding.UTF8.GetBytes(validate);
            foreach (char ch in Encoding.UTF8.GetChars(bytes))
            {
                if (((ch > 'ࠀ') && (ch < '一')) || ((ch > 0xac00) && (ch < 0xd7a3)))
                {
                    if (startIndex >= validate.Length)
                    {
                        return "";
                    }
                    return validate.Substring(startIndex, ((length + startIndex) > validate.Length) ? (validate.Length - startIndex) : length);
                }
            }
            if (length < 0)
            {
                return str;
            }
            byte[] sourceArray = Encoding.Default.GetBytes(validate);
            if (sourceArray.Length <= startIndex)
            {
                return str;
            }
            int num = sourceArray.Length;
            if (sourceArray.Length > (startIndex + length))
            {
                num = length + startIndex;
            }
            else
            {
                length = sourceArray.Length - startIndex;
                addEndText = "";
            }
            int num2 = length;
            int[] numArray = new int[length];
            byte[] destinationArray = null;
            int num3 = 0;
            for (int i = startIndex; i < num; i++)
            {
                if (sourceArray[i] > 0x7f)
                {
                    num3++;
                    if (num3 == 3)
                    {
                        num3 = 1;
                    }
                }
                else
                {
                    num3 = 0;
                }
                numArray[i] = num3;
            }
            if ((sourceArray[num - 1] > 0x7f) && (numArray[length - 1] == 1))
            {
                num2 = length + 1;
            }
            destinationArray = new byte[num2];
            Array.Copy(sourceArray, startIndex, destinationArray, 0, num2);
            return (Encoding.Default.GetString(destinationArray) + addEndText);
        }

        /// <summary>
        /// //去除尾部的空格
        /// </summary>
        public static string TrimEndBreak(string s)
        {
            for (int i = s.Length - 1; i >= 0; i--)
            {
                char ch = s[i];
                if (!ch.Equals(" "))
                {
                    char ch2 = s[i];
                    if (!ch2.Equals("\r"))
                    {
                        char ch3 = s[i];
                        if (!ch3.Equals("\n"))
                        {
                            continue;
                        }
                    }
                }
                s.Remove(i, 1);
            }
            return s;
        }

        public static string ToString(object o)
        {
            if (o != null)
            {
                return o.ToString();
            }
            return "";
        }


        /// <summary>
        /// //filter the script code
        /// </summary>
        public static string ClearScript(string text)
        {
            text = Regex.Replace(text, "<script((.|\n)*?)</script>", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            return Regex.Replace(text, "\"javascript:", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        }

        public static bool IsAbsoluteUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            if (url.ToLower().IndexOf("http://") != 0)
            {
                return false;
            }
            return true;
        }

        public static bool InArray(string s, string[] array, bool IgnoreCase)
        {
            if (((array != null) && (array.Length != 0)) && !string.IsNullOrEmpty(s))
            {
                foreach (string str in array)
                {
                    if (IgnoreCase)
                    {
                        if (str.ToLower() == s.ToLower())
                        {
                            return true;
                        }
                    }
                    else if (str == s)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static string[] MatchImg(string htmlText)
        {
            Regex regex = new Regex("<img\\s[^>]*?\\ssrc=(?<q>['\"\"]?)(?<src>[^\\[^>]*?[gif|jpg|jpeg|bmp|bmp])\\1\\s[^>].*?>", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            List<string> list = new List<string>();
            for (Match match = regex.Match(htmlText); match.Success; match = match.NextMatch())
            {
                string url = match.Groups["src"].Value;
                list.Add(UrlEncode(url));
            }
            return list.ToArray();
        }

        public static string UrlEncode(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }
            string[] strArray = url.Split(new char[] { '/', '\\' });
            for (int i = 0; i < strArray.Length; i++)
            {
                string str = strArray[i];
                if (!string.IsNullOrEmpty(str))
                {
                    strArray[i] = HttpUtility.UrlEncode(str);
                }
            }
            return string.Join("/", strArray);
        }

        public static string UnicodeSubString(string validate, int length, string replace)
        {
            if (length < 0)
            {
                return validate;
            }
            validate = TrimEndBreak(validate);
            string str = string.Empty;
            int byteCount = Encoding.Default.GetByteCount(validate);
            int num2 = validate.Length;
            int num3 = 0;
            int num4 = 0;
            if (byteCount <= length)
            {
                return validate;
            }
            for (int i = 0; i < num2; i++)
            {
                if (Convert.ToInt32(validate.ToCharArray()[i]) > 0xff)
                {
                    num3 += 2;
                }
                else
                {
                    num3++;
                }
                if (num3 > length)
                {
                    num4 = i;
                    break;
                }
                if (num3 == length)
                {
                    num4 = i + 1;
                    break;
                }
            }
            if (num4 >= 0)
            {
                str = validate.Substring(0, num4) + replace;
            }
            return str;
        }

    }
}
