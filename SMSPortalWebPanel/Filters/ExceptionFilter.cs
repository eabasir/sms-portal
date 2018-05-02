using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMSPortalWebPanel.Logger;
using System.Web.Routing;

namespace SMSPortalWebPanel.Filters
{
    public class ExceptionFilter:HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            FileLogger logger = new FileLogger();
            logger.LogException(filterContext.Exception);
            base.OnException(filterContext);

            
           
        }
    }
}