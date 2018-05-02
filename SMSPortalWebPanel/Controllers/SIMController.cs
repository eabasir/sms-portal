using SMSPortalWebPanel.Filters;
using SMSPortalWebPanel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMSPortalWebPanel.Models;
using SMSPortalDBDataLibrary;

namespace SMSPortalWebPanel.Controllers
{
    [PortalAuthorizatoin]
    [AdminFilter]
    public class SIMController : Controller
    {
        [HeaderFilter]
        public ActionResult Index()
        {
            SIMListViewModel simListVM = new SIMListViewModel();
            SIMBusinessLayer bll = new SIMBusinessLayer();
            List<SIMViewModel> lstSimVM = new List<SIMViewModel>();

            foreach (SIM sim in bll.GetSims())
            {
                SIMViewModel simViewModel = new SIMViewModel()
                {
                    Id = sim.TFId,
                    Number = sim.TFNumber,
                    Charge = sim.TFCharge.HasValue ? sim.TFCharge.ToString() : "-",
                    Port = sim.TFPort.ToString()

                };
                lstSimVM.Add(simViewModel);
            }
            simListVM.SIMVMs = lstSimVM;

            return View("Index", simListVM);
        }


        [HeaderFilter]
        public ActionResult AddUpdate(Guid? id)
        {
            SIMViewModel simVM = new SIMViewModel();

            if (id.HasValue)
            {
                SIM sim = new SIMBusinessLayer().getSimById((Guid)id);
                simVM.Id = sim.TFId;
                simVM.Number = sim.TFNumber;
                simVM.Charge = sim.TFCharge.HasValue ? sim.TFCharge.ToString() : "-";
                simVM.Port = sim.TFPort.ToString();


            }
            return View("AddUpdate", simVM);
        }

        [HeaderFilter]
        public ActionResult Save(SIMViewModel simVM)
        {
            
            if (ModelState.IsValid)
            {
                SIMBusinessLayer simBLL = new SIMBusinessLayer();
                bool result = false;
                string message = string.Empty;
                if (simVM.Id != Guid.Empty)
                {
                    result = simBLL.updateSim(simVM);
                    message = (result ? SMSPortalCross.Constants.OPERATION_SUCCESSFUL : "شماره سیم کارت یا پورت اتصال نمی تواند تکراری باشد");
                }
                else
                {
                    result = simBLL.addNewSim(simVM);
                    message = (result ? SMSPortalCross.Constants.OPERATION_SUCCESSFUL : "شماره سیم کارت یا پورت اتصال نمی تواند تکراری باشد");
                     
                }
                simVM._Result = message;
                if (result)
                    return RedirectToAction("Index");
                else
                    return View("AddUpdate", simVM);
            }
            else
            {
                //simVM.Port =  ModelState["Port"].Value.AttemptedValue;
                return View("AddUpdate", simVM);
            }

          
        }


        public ActionResult Delete(Guid id)
        {

            SIMBusinessLayer simBLL = new SIMBusinessLayer();
            simBLL.deleteSimById(id);
            return RedirectToAction("Index");
        }
    }
}
