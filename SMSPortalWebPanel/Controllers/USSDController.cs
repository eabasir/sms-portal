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
    [AdminFilter]
    public class USSDController : Controller
    {
        [HeaderFilter]
        public ActionResult Index(Guid id)
        {

            USSDListViewModel ussdListVM = new USSDListViewModel()
            {
                SIM_Number = new SIMBusinessLayer().getSimById(id).TFNumber
            };
            return View("index", ussdListVM);
        }



        [HttpPost]
        public ActionResult getUSSDList(String number)
        {
            try
            {
                Guid simId = new SIMBusinessLayer().getSimByNumber(number).TFId;
                string s = JsonConvert.SerializeObject(new USSDBusinessLayer().GetUSSDs(simId));
                return Json(s);
            }
            catch
            {
                return null;
            }

        }


        [HttpPost]
        public ActionResult sendNewUSSD(String data)
        {
            try
            {
                SendNewUSSD vm = JsonConvert.DeserializeObject<SendNewUSSD>(data);

                new USSDBusinessLayer().sendNew(vm);

                return Json("{'result': 'true'}");
            }
            catch
            {
                return null;
            }
        }






    }
}