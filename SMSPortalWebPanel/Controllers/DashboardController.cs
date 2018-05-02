using Newtonsoft.Json;
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
    public class DashboardController : Controller
    {
        [HeaderFilter]
        public ActionResult Index()
        {
            DashboardViewModel dashboardVM = new DashboardViewModel
            {
                SIMDatas = new DashboardBusinessLayer().getSimDatas()
            };
            return View("Index" , dashboardVM);
        }


        [HttpPost]
        public ActionResult getBarChartData() {

            var q = new DashboardBusinessLayer().getBarChartData();

            return Json(JsonConvert.SerializeObject(q));

        }

        [HttpPost]
        public ActionResult getPieChartData()
        {

            var q = new DashboardBusinessLayer().getPieChartData();

            return Json(JsonConvert.SerializeObject(q));

        }
    }
}