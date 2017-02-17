using System;
using System.IO;
using System.Text;
using log4net;
using System.Web;
using GMS.Core.Config;

namespace GMS.Core.Log
{
    public class Log4NetHelper
    {
        static Log4NetHelper()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;

            string path = Path.Combine(dir, "Config/log4net.config");
            
            FileInfo file = new FileInfo(path);
            SetConfig(file);
        }

        private static void SetConfig(FileInfo configFile)
        {   
            log4net.Config.XmlConfigurator.Configure(configFile);
        }

        private static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");

        private static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");

        public static void Info(string info)
        {
            lock (typeof(Log4NetHelper))
            {
                if (loginfo.IsInfoEnabled)
                {
                    loginfo.Info(info);
                }
            }
        }

        public static void Error(string info, Exception se)
        {
            lock (typeof(Log4NetHelper))
            {
                if (logerror.IsErrorEnabled)
                {
                    logerror.Error(info, se);
                }
            }
        }

        
    }
}
