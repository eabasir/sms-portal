using SMSPortalWebPanel.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSPortalWebPanel.Controllers
{
    
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            BaseViewModel bvm = new BaseViewModel();
            return View(bvm);
        }

        [HttpPost]
        public ActionResult getData()
        {
            Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            return Json(json);
        }
    }
}