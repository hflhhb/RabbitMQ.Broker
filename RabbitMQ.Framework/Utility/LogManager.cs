using RabbitMQ.Framework.Infra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RabbitMQ.Framework.Utility
{
    /// <summary>
    /// LogManager
    /// </summary>
    public class LogManager
    {
        private static string _logPath = string.Empty;
        private static readonly Lazy<LifeQueue> _lifeQueue = new Lazy<LifeQueue>(() => new LifeQueue(10));
        private static object _locker = new object();
        private LogManager()
        {
            //_lifeQueue = new LifeQueue();
        }
        /// <summary>
        /// 保存日志的文件夹
        /// </summary>
        public static string LogPath
        {
            get
            {
                if (_logPath == string.Empty)
                {
                    if (HttpContext.Current == null)
                    {
                        _logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Log");
                    }
                    else
                    {
                        //_logPath = @"D:\log\"; //AppDomain.CurrentDomain.BaseDirectory + @"bin\";
                        _logPath = HttpContext.Current.Server.MapPath("~/Log");
                    }
                    if (!Directory.Exists(_logPath))
                    {
                        Directory.CreateDirectory(_logPath);
                    }

                }
                return _logPath;
            }
            set { _logPath = value; }
        }

        /// <summary>
        /// 日志文件前缀
        /// </summary>
        public static string LogFielPrefix { get; set; } = "Log";

        private static void EnqueueLog(string logFile, string msg)
        {
            _lifeQueue.Value.Enqueue(() => WriteLogCore(logFile, msg));
        }

        private static void WriteLogCore(string logFile, string msg)
        {
            lock (_locker)
            {
                try
                {
                    string fileLastName = LogFielPrefix + logFile + "_" + DateTime.Now.ToString("yyyyMMdd") + ".Log";
                    string fileName = Path.Combine(LogPath, fileLastName);
                    using (StreamWriter sw = File.AppendText(fileName))
                    {
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + msg);
                        sw.Close();
                    }
                }
                catch (Exception ex)
                {
                    WriteLogCore("LogManagerError", ex.Message);
                }
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        public static void WriteLog(string logFile, string msg)
        {
            EnqueueLog(logFile, msg);
        }
        /// <summary>
        /// 写日志
        /// </summary>
        public static void Trace(string msg)
        {
            WriteLog("", msg);
        }
        /// <summary>
        /// 写日志
        /// </summary>
        public static void WriteLog(LogFile logFile, string msg)
        {
            WriteLog(logFile.ToString(), msg);
        }
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogFile
    {
        Trace,
        Warning,
        Error,
        SQL
    }
}
