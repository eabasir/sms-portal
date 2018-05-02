using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


namespace SMSPortalWebPanel
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static string DefaultUserName { get { return "admin"; } }
        public static string DefaultPassword { get { return "admin"; } }
        public static string DefaultFirstName { get { return "ادمین"; } }
        public static string DefaultLastName { get { return "-"; } }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);//Explanation done
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
           
            
        }
    }
}
