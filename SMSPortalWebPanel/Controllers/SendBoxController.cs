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
    public class SendBoxController : Controller
    {
        [HeaderFilter]
        public ActionResult Index()
        {
            BaseViewModel bvm = new BaseViewModel();
            return View("Index", bvm);
        }


        [HttpPost]
        public ActionResult getSents()
        {
            var q = new SendBoxBusinessLayer().getSents();
            string s = JsonConvert.SerializeObject(q);
            return Json(s);
        }


        [HttpPost]
        public ActionResult getSentContacts(string id)
        {
            Guid sentBoxId = Guid.Parse(id);
            var q = new SendBoxBusinessLayer().getSentContacts(sentBoxId);
            string s = JsonConvert.SerializeObject(q);
            return Json(s);
        }

     


    }

}