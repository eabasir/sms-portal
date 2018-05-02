using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMSPortalWebPanel.ViewModels;
using SMSPortalCross;
using SMSPortalDBDataLibrary;

namespace SMSPortalWebPanel.Filters
{
    public class HeaderFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewResult v = filterContext.Result as ViewResult;
            if (v != null)
            {
                BaseViewModel bvm = v.Model as BaseViewModel;
                bvm.User_UserName = HttpContext.Current.User.Identity.Name;
                
                bvm.User_Status = Enums.getUserType(Convert.ToInt32(filterContext.HttpContext.Session["Status"]));
                bvm.User_FirstName = Convert.ToString(filterContext.HttpContext.Session["FirstName"]);
                bvm.User_LastName = Convert.ToString(filterContext.HttpContext.Session["LastName"]);

                try
                {
                    using (SMSPortalDBEntities db = new SMSPortalDBEntities()) {
                        bvm.UnreadMessageCount = (from x in db.Inboxes where x.TFIsRead == false select x).Count();
                    }
                }
                catch { }


            }
        }
    }
}