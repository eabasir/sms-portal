using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace SMSPortalService
{
    public class Global : HttpApplication
    {

        public static readonly object Task_Sync_Obj = new object();

      
        string Dir_Scheduler;
        public static string File_Scheduler;

        void Application_Start(object sender, EventArgs e)
        {
            
            Dir_Scheduler = Path.Combine(Server.MapPath("~"), "Scheduler");
            File_Scheduler = Path.Combine(Dir_Scheduler, "SMSPortalScheduleProcess.exe");

            if (!Directory.Exists(Dir_Scheduler))
                Directory.CreateDirectory(Dir_Scheduler);


            new Logger(Server);

       
        }
    }
}