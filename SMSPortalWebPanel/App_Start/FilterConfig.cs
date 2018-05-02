using System.Web;
using System.Web.Mvc;
using SMSPortalWebPanel.Filters;
using System;
namespace SMSPortalWebPanel
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {

            //filters.Add(new HandleErrorAttribute()
            //    {
            //        ExceptionType = typeof(DivideByZeroException),
            //        View = "DivideError"
            //    });
            //filters.Add(new HandleErrorAttribute()
            //{
            //    ExceptionType = typeof(NotFiniteNumberException),
            //    View = "NotFiniteError"
            //});
            //filters.Add(new HandleErrorAttribute());//ExceptionFilter
            filters.Add(new ExceptionFilter());
            filters.Add(new AuthorizeAttribute());
        }
    }
}
