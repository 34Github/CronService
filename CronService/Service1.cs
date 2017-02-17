using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Net;
using System.IO;
using CronNET;

namespace CronService
{
    public partial class Service1 : ServiceBase
    {
        private readonly CronDaemon cron_daemon = new CronDaemon();
        

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            
            var urls = GMS.Core.Config.CachedConfigContext.Current.UrlConfig.Urls;

            foreach (var urlInfo in urls)
            {
                var t = new System.Threading.ParameterizedThreadStart(ScheduleTask);
                if(urlInfo.Ready)
                    cron_daemon.AddJob(urlInfo.Schedule, t, urlInfo);
            }
             
            cron_daemon.Start();
        }

        protected override void OnStop()
        {
            cron_daemon.Start();
        }
        
        private void ScheduleTask(object obj)
        {
            var urlInfo = obj as GMS.Core.Config.UrlInfo;

            if (!string.IsNullOrWhiteSpace(urlInfo.Url))
            {
                try
                {   
                    var result = HttpGet(urlInfo.Url, string.Empty);

                    GMS.Core.Log.Log4NetHelper.Info("OK，URL:" + urlInfo.Url);
                }
                catch (Exception ex)
                {   
                    GMS.Core.Log.Log4NetHelper.Info("Error，URL:" + urlInfo.Url);
                    
                    GMS.Core.Log.Log4NetHelper.Error(urlInfo.Name + "出现故障", ex);
                }
            }
                        
            
        }

        public static string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }  
    }
}
