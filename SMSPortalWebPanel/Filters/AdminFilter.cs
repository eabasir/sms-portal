using SMSPortalCross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace SMSPortalWebPanel.Filters
{
    public class AdminFilter:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Convert.ToInt32(filterContext.HttpContext.Session["Status"]) != (int) Enums.UserLevel.Admin)
            {
                filterContext.Result = new ContentResult()
                {
                    Content="شما دسترسی به این عملیات را ندارید."
                };
            }
        }
    }
}