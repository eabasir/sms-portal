using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace SMSPortalWebPanel.Logger
{
    public class FileLogger
    {
        string root = HostingEnvironment.ApplicationPhysicalPath;
        string dir_log;
        string file_log;

        public void LogException(Exception e)
        {

            dir_log = Path.Combine(root, "Log");

            if (!Directory.Exists(dir_log))
            {
                Directory.CreateDirectory(dir_log);
            }

            file_log = Path.Combine(dir_log, DateTime.Now.ToString("dd-MM-yyyy mm hh ss") + ".txt");
            if (!File.Exists(file_log))
            {
                using (var fa = File.Create(file_log))
                {
                    fa.Close();
                }
            }

            File.WriteAllLines(file_log, 
                new string[] 
                {
                    "Message:"+e.Message,
                    "Stacktrace:"+e.StackTrace
                });


        }
    }
}