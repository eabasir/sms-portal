using Newtonsoft.Json;
using SMSPortalCross;
using SMSPortalWebPanel.Filters;
using SMSPortalWebPanel.Models;
using SMSPortalWebPanel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSPortalWebPanel.Controllers
{
    [PortalAuthorizatoin]
    public class ReportController : Controller
    {
        [HeaderFilter]
        public ActionResult Index()
        {
            BaseViewModel bvm = new BaseViewModel();
            return View("index", bvm);
        }

        [HttpPost]
        public ActionResult getReport(string data)
        {
            try
            {
                SearchViewModel vm = JsonConvert.DeserializeObject<SearchViewModel>(data);

                
                if (vm.DTStart == vm.DTFinish && !(string.IsNullOrEmpty(vm.DTStart) && string.IsNullOrEmpty(vm.DTFinish)))
                    return null;

                DateTime dtStart = !string.IsNullOrEmpty(vm.DTStart) ? SMSPortalCross.Utils.Date.PersianToGregorian(vm.DTStart) : DateTime.Parse("2016-01-01");
                DateTime dtFinish = !string.IsNullOrEmpty(vm.DTFinish) ? SMSPortalCross.Utils.Date.PersianToGregorian(vm.DTFinish) : DateTime.Parse("2216-01-01");

                Enums.SearchType type = Enums.getSearchType(Convert.ToInt32(vm.Type));

                var q = new ReportBusinessLayer().getReport(type, vm.Text, dtStart, dtFinish).Take(500);

                return Json(JsonConvert.SerializeObject(q));

            }
            catch { };

            return null;

        }
    }



}