using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Web;

namespace Sun.diagnostics
{
    public static class log
    {
        private static StreamWriter debugWriter;
        private static TraceSwitch debugSwitch;
        private static TextWriterTraceListener _textWriterTraceListener;

        private static string tracingFileName;

        static log()
        {
            Type _meType = typeof(Sun.diagnostics.log);
            try
            {
                if (!Monitor.TryEnter(_meType))
                {
                    Monitor.Enter(_meType);
                }
                else
                {
                    bool flag = true;
                    try
                    {
                        string tracingFilePath = "~/suncms/log/";

                        tracingFileName = DateTime.Now.ToString("yyyyMMdd") + ".txt";
                        tracingFilePath = tracingFilePath + tracingFileName;
                        tracingFilePath = Sun.Toolkit.context.getMapPath(tracingFilePath);

                        string directoryPath = Path.GetDirectoryName(tracingFilePath);
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        FileInfo _fileInfo = new FileInfo(tracingFilePath);
                        debugWriter = new StreamWriter(_fileInfo.Open(FileMode.Create, FileAccess.Write, FileShare.ReadWrite));
                        _textWriterTraceListener = new TextWriterTraceListener(debugWriter);

                        Trace.Listeners.Add(_textWriterTraceListener);


                        flag = false;
                    }
                    catch
                    {
                    }
                    if (flag)
                    {
                        debugSwitch = null;
                        debugWriter = null;
                    }
                }
            }
            catch
            {
            }
            finally
            {
                Monitor.Exit(_meType);
            }
        }

        /// <summary>
        /// // 记录错误log
        /// </summary>
        public static void recordError(string message)
        {
            try
            {
                if (debugWriter != null)
                {   
                    string _fileName = DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    if (tracingFileName != _fileName)
                    {
                        tracingFileName = _fileName;
                        FileInfo _fileInfo = new FileInfo("~/suncms/log/" + tracingFileName);
                        debugWriter = new StreamWriter(_fileInfo.Open(FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
                    }

                    lock (debugWriter)
                    {
                        Exception ex = Sun.Toolkit.context.getCurrentHttpContext().Server.GetLastError();
                        string txt;
                        if (ex == null)
                        {
                            txt = "-------------------------------------------------------\r\ndatetime:  "
                                   + DateTime.Now.ToString()
                                   + "\r\nusername:  "
                                   + "null"
                                   + "\r\nip:        "
                                   + Sun.Toolkit.context.getUserIP() + "\r\nurl:       "
                                   + Sun.Toolkit.context.getCurrentHttpContext().Request.Url.ToString()
                                   + "\r\nmessage:   " + message
                                   + "\r\n-------------------------------------------------------\r\n";
                        }
                        else
                        {
                            txt = "-------------------------------------------------------\r\ndatetime: "
                                   + DateTime.Now.ToString()
                                   + "\r\nusername: "
                                   + "null"
                                   + "\r\nipaddress: "
                                   + Sun.Toolkit.context.getUserIP() + "\r\nurl:"
                                   + Sun.Toolkit.context.getCurrentHttpContext().Request.Url.ToString()
                                   + "\r\nmessage:" + message
                                   + "\r\n\r\n-------------------------\r\n"
                                   + HttpUtility.HtmlEncode(Sun.Toolkit.context.getCurrentHttpContext().Server.GetLastError().ToString())
                                   + "\r\n\r\n-------------------------------------------------------\r\n";
                        }

                        Trace.WriteLine(txt);
                        debugWriter.Flush();
                    }
                }

                // 搜索 EventLog
                //new EventLog("sun_application", ".", "Sun.Web").WriteEntry("1111111111111111111", EventLogEntryType.Error);
            }
            catch
            {
            }


        }

    }
}
