using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace Sun.Util
{
    /// <summary>
    /// //这里只是  收集常用方法，，
    /// </summary>
    class zRepo
    {
        /// <summary>
        /// 将文件转换成字符串,常用于读取网站模板
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isSpace"></param>
        /// <returns></returns>
        public static string GetTempleContent(string path)
        {
            string result = string.Empty;
            string sFileName = HttpContext.Current.Server.MapPath(path);
            if (File.Exists(sFileName))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(sFileName))
                    {
                        result = sr.ReadToEnd();
                    }
                }
                catch
                {
                    result = "读取模板文件(" + path + ")出错";
                }
            }
            else
            {
                result = "找不到模板文件：" + path;
            }
            return result;
        }

        /// <summary>
        /// 读取,添加，修改xml文件
        /// </summary>
        /// <param name="Xmlpath">Xml路径</param>
        /// <param name="Node">新的子节点名称</param>
        /// <param name="Value">新节点对应的值</param>
        /// <param name="flag">1：读取，否则为 修改或者添加</param>
        /// <returns>1：修改添加成功，为空字符串表示修改添加成功，否则是读取成功</returns>
        public static string getXML(string Xmlpath, string Node, string Value, int flag)
        {
            try
            {
                string filepath = HttpContext.Current.Server.MapPath(Xmlpath);
                XmlDocument xmlDoc = new XmlDocument();
                if (!File.Exists(filepath))
                {
                    XmlDeclaration xn = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                    XmlElement root = xmlDoc.CreateElement("rss");
                    XmlElement root1 = xmlDoc.CreateElement("item");

                    root.AppendChild(root1);
                    xmlDoc.AppendChild(xn);
                    xmlDoc.AppendChild(root);
                    xmlDoc.Save(filepath);//本地路径名字
                }
                xmlDoc.Load(filepath);//你的xml文件
                string ReStr = string.Empty;
                XmlElement xmlObj = xmlDoc.DocumentElement;

                XmlNodeList xmlList = xmlDoc.SelectSingleNode(xmlObj.Name.ToString()).ChildNodes;

                foreach (XmlNode xmlNo in xmlList)
                {
                    if (xmlNo.NodeType != XmlNodeType.Comment)//判断是不是注释类型
                    {
                        XmlElement xe = (XmlElement)xmlNo;
                        {
                            if (xe.Name == xmlObj.FirstChild.Name)
                            {
                                XmlNodeList xmlNList = xmlObj.FirstChild.ChildNodes;

                                foreach (XmlNode xmld in xmlNList)
                                {
                                    XmlElement xe1 = (XmlElement)xmld;
                                    {
                                        if (xe1.Name == Node)
                                        {
                                            if (flag == 1)//读取值
                                            {
                                                if (xmld.InnerText != null && xmld.InnerText != "")
                                                {
                                                    ReStr = xmld.InnerText;
                                                }
                                            }
                                            else//修改值
                                            {
                                                xmld.InnerText = Value;//给节点赋值
                                                xmlDoc.Save(filepath);
                                                ReStr = Value.Trim();
                                            }
                                        }
                                    }
                                }
                                if (ReStr == string.Empty)// 添加节点
                                {
                                    XmlNode newNode;
                                    newNode = xmlDoc.CreateNode("element", Node, Value);//创建节点
                                    newNode.InnerText = Value;//给节点赋值
                                    xe.AppendChild(newNode);//把节点添加到doc
                                    xmlDoc.Save(filepath);
                                    ReStr = Value.Trim();
                                }
                            }
                        }
                    }
                }
                return ReStr;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 取得文件扩展名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>扩展名</returns>
        public static string GetFileEXT(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return "";
            }
            if (filename.IndexOf(@".") == -1)
            {
                return "";
            }
            int pos = -1;
            if (!(filename.IndexOf(@"\") == -1))
            {
                pos = filename.LastIndexOf(@"\");
            }
            string[] s = filename.Substring(pos + 1).Split('.');
            return s[1];
        }

        /// <summary>
        /// 替换文本中的空格和换行
        /// </summary>
        public static string ReplaceSpace(string str)
        {
            string s = str;
            s = s.Replace(" ", "&nbsp;");
            s = s.Replace("\n", "<BR />");
            return s;
        }

        /// <summary>
        /// 去掉结尾，
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string LostDot(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            else
            {
                if (input.IndexOf(",") > -1)
                {
                    int intLast = input.LastIndexOf(",");
                    if ((intLast + 1) == input.Length)
                    {
                        return input.Remove(intLast);
                    }
                    else
                    {
                        return input;
                    }
                }
                else
                {
                    return input;
                }
            }
        }

        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <returns></returns>
        private int getRandom(int minValue, int maxValue)
        {
            Random ri = new Random(unchecked((int)DateTime.Now.Ticks));
            int k = ri.Next(minValue, maxValue);
            return k;
        }

        //想定一个三位的随机数：string ThreeRandom=this.getRandom(100,999).Tostring();
        //类似的，四位随机数：string FourRandom=this.getRandom(1000,9999).Tostring();


        /// <summary>
        /// 判断输入是否为日期类型
        /// </summary>
        /// <param name="s">待检查数据</param>
        /// <returns></returns>
        public static bool IsDate(string s)
        {
            if (s == null)
            {
                return false;
            }
            else
            {
                try
                {
                    DateTime d = DateTime.Parse(s);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// MD5加密字符串处理
        /// </summary>
        /// <param name="Half">加密是16位还是32位；如果为true为16位</param>
        public static string MD5(string Input, bool Half)
        {
            string output = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Input, "MD5").ToLower();
            if (Half) output = output.Substring(8, 16);
            return output;
        }

        public static string MD5(string Input)
        {
            return MD5(Input, true);
        }

        /// <summary>
        /// 过滤字符
        /// </summary>
        public static string Filter(string sInput)
        {
            if (sInput == null || sInput.Trim() == string.Empty)
                return null;
            string sInput1 = sInput.ToLower();
            string output = sInput;
            string pattern = @"*|and|exec|insert|select|delete|update|count|master|truncate|declare|char(|mid(|chr(|'";
            if (Regex.Match(sInput1, Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
            {
                throw new Exception("字符串中含有非法字符!");
            }
            else
            {
                output = output.Replace("'", "''");
            }
            return output;
        }


        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="strValue">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                return HttpContext.Current.Request.Cookies[strName].Value.ToString();
            }
            return "";
        }

        /// <summary>
        /// //将用户输入的字符串转换为可换行、替换Html编码、无危害数据库特殊字符、去掉首尾空白、的安全方便代码
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string ConvertStr(string inputString)
        {
            string retVal = inputString;
            retVal = retVal.Replace("&", "&amp;");
            retVal = retVal.Replace("\"", "&quot;");
            retVal = retVal.Replace("<", "&lt;");
            retVal = retVal.Replace(">", "&gt;");
            retVal = retVal.Replace(" ", "&nbsp;");
            retVal = retVal.Replace("  ", "&nbsp;&nbsp;");
            retVal = retVal.Replace("\t", "&nbsp;&nbsp;");
            retVal = retVal.Replace("\r", "<br>");
            return retVal;
        }

        private static string FetchURL(string strMessage)
        {
            string strPattern = @"(?<url>(http|ftp|mms|rstp|news|https)://(?:[\w-]+\.)+[\w-]+(?:/[\w-./?%&~=]*[^.\s|,|\)|<|!])?)";
            string strReplace = "<a href=\"${url}\" target=_blank>${url}</a>";
            string strInput = strMessage;
            string strResult;
            strResult = Regex.Replace(strInput, strPattern, strReplace);
            strPattern = @"(?<!http://)(?<url>www\.(?:[\w-]+\.)+[\w-]+(?:/[\w-./?%&~=]*[^.\s|,|\)|<|!])?)";
            strReplace = "<a href=\"http://${url}\" target=_blank>${url}</a>";
            strResult = Regex.Replace(strResult, strPattern, strReplace);
            return strResult;
        }

        public string ToUrl(string inputString)
        {
            string retVal = inputString;
            retVal = ConvertStr(retVal);
            retVal = FetchURL(retVal);
            return retVal;
        }


    
    }
}
