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
    public class QueueController : Controller
    {
        [HeaderFilter]
        public ActionResult Index()
        {
            BaseViewModel bvm = new BaseViewModel();
            return View("Index", bvm);
        }


        [HttpPost]
        public ActionResult getQueues()
        {

            var q = new QueueBusinessLayer().getQueues();
            string s = JsonConvert.SerializeObject(q);
            return Json(s);
        }


        [HttpPost]
        public ActionResult getQueueContacts(string id)
        {
            Guid queueId = Guid.Parse(id);

            var q = new QueueBusinessLayer().getQueueContacts(queueId);

            string s = JsonConvert.SerializeObject(q);
            return Json(s);
        }


        [HttpPost]
        public ActionResult changeQueueStatus(string Id, bool Status)
        {
            Guid queueId = Guid.Parse(Id);
            new QueueBusinessLayer().changeQueueStatus(queueId, Status);
            return Json("");
        }


        [HttpPost]
        public ActionResult removeQueue(string Id)
        {
            Guid queueId = Guid.Parse(Id);
            new QueueBusinessLayer().removeQueue(queueId);
            return Json("");
        }


        [HttpPost]
        public ActionResult changeQueueContactStatus(string Id, bool Status)
        {
            Guid queueContactId = Guid.Parse(Id);
            string res = new QueueBusinessLayer().changeQueueContactStatus(queueContactId, Status);
            if (res != null)
                return Json(JsonConvert.SerializeObject(new { number = res }));
            else
                return Json("");
        }


        [HttpPost]
        public ActionResult removeQueueContact(string Id)
        {
            Guid queueContactId = Guid.Parse(Id);
           string res = new QueueBusinessLayer().removeQueueContact(queueContactId);
            if (res != null)
                return Json(JsonConvert.SerializeObject(new {number = res }));
            else
                return Json("");
        }

        [HttpGet]
        public ActionResult restartQueues()
        {

            new QueueBusinessLayer().restartQueues();

            return Json("");
        }


    }

}