using System;
using System.Collections.Generic;
using System.Text;

namespace Sun.Util
{
    public static class IO
    {
        /// <summary>
        /// //判断文件存不存在
        /// </summary>
        public static bool FileExist(string url)
        {
            return System.IO.File.Exists(Util.Path.GetMapPath(url.Split(new char[] { '?' })[0]));
        }

    }
}
