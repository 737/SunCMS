using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Sun.Toolkit
{
    public static class io
    {
        /// <summary>
        /// //判断文件存不存在
        /// 
        /// </summary>
        public static bool isFileExist(string url)
        {
            return System.IO.File.Exists(context.getMapPath(url.Split(new char[] { '?' })[0]));
        }

        /// <summary>
        /// // 读取文件内容
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string getTextFile(string path)
        {
            string absPath = context.getMapPath(path);
            string charset = "gb2312";
            string txt;

            Encoding encoding = Encoding.GetEncoding(charset);
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(absPath, encoding);
                txt = reader.ReadToEnd();
            }
            catch (Exception)
            {
                txt = "读取文件失败 或者  文件不存在";
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
            }
            return txt;
        }
    }
}
