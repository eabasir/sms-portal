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
    public class PortalAuthorizatoin : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            Enums.UserLevel userLevel = Enums.getUserType(Convert.ToInt32(httpContext.Session["Status"]));

            try
            {
                // check if user is not removed by admin on other sessions
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    if (db.Users.Select(x => x.TFUserName == httpContext.User.Identity.Name).Count() == 0)
                        return false;
                }
            }
            catch {
                return false;
            }
            
            if (userLevel == Enums.UserLevel.NotAuthenticated || userLevel == Enums.UserLevel.DefaultOrFirstTime)
                return false;
            else
                return true;

        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            Enums.UserLevel userLevel = Enums.getUserType(Convert.ToInt32(filterContext.HttpContext.Session["Status"]));
            if (userLevel == Enums.UserLevel.DefaultOrFirstTime)
            {
                filterContext.Result = new RedirectToRouteResult(
                              new RouteValueDictionary
                              {
                                       { "action", "EditPassword" },
                                       { "controller", "Profile" }
                              });
            }
            else
                filterContext.Result = new RedirectToRouteResult(
                                  new RouteValueDictionary
                                  {
                                       { "action", "Login" },
                                       { "controller", "Authentication" }
                                  });
        }


    }

}