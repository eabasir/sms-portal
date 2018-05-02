using SMSPortalCross;
using SMSPortalDBDataLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SMSPortalWebPanel.Filters
{
    public class FirstTimeFilter : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            Enums.UserLevel userLevel = Enums.getUserType(Convert.ToInt32(httpContext.Session["Status"]));

            if (userLevel == Enums.UserLevel.DefaultOrFirstTime)
                return true;
            else
                return false;

        }


    }

}