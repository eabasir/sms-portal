using SMSPortalWebPanel.Filters;
using SMSPortalWebPanel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMSPortalWebPanel.Models;
using SMSPortalDBDataLibrary;
using Newtonsoft.Json;

namespace SMSPortalWebPanel.Controllers
{
    [PortalAuthorizatoin]
    public class CompanyController : Controller
    {
        [HeaderFilter]
        public ActionResult Index()
        {
            CompanyListViewModel CompanyListVM = new CompanyListViewModel();
            CompanyBusinessLayer bll = new CompanyBusinessLayer();
            List<CompanyViewModel> lstCompanyVM = new List<CompanyViewModel>();

            foreach (Company company in bll.getCompanies())
            {
                CompanyViewModel CompanyViewModel = new CompanyViewModel()
                {
                    Id = company.TFId,
                    Name = company.TFName,
                    Number = company.TFPhone,
                    Address = company.TFAddress
                 
                };
                lstCompanyVM.Add(CompanyViewModel);
            }
            CompanyListVM.CompanyVMs = lstCompanyVM;

            return View("Index", CompanyListVM);
        }


        [HeaderFilter]
        public ActionResult AddUpdate(Guid? id)
        {
            CompanyViewModel companyVM = new CompanyViewModel();

            
            if (id.HasValue)
            {
                CompanyBusinessLayer companyBLL = new CompanyBusinessLayer();

                Company company = companyBLL.getCompanyById((Guid)id);
                companyVM.Id = company.TFId;
                companyVM.Name = company.TFName;
                companyVM.Number = !string.IsNullOrEmpty(company.TFPhone) ? company.TFPhone : "-";
                companyVM.Address = !string.IsNullOrEmpty(company.TFAddress) ? company.TFAddress : "-";
                companyVM.selectedContacts = companyBLL.getCompanyContactVms((Guid)id);
           
            }
            return View("AddUpdate", companyVM);
        }

        [HeaderFilter]
        public ActionResult Save(CompanyViewModel CompanyVM , List<Guid> newContactId)
        {
            
            if (ModelState.IsValid)
            {
                CompanyBusinessLayer companyBLL = new CompanyBusinessLayer();
                bool result = false;
                string message = string.Empty;
                if (CompanyVM.Id != Guid.Empty)
                {
                    result = companyBLL.updateCompany(CompanyVM , newContactId);
                    message = (result ? SMSPortalCross.Constants.OPERATION_SUCCESSFUL : "نام سازمان نمی تواند تکراری باشد.");
                }
                else
                {
                    result = companyBLL.addNewCompany(CompanyVM , newContactId);
                    message = (result ? SMSPortalCross.Constants.OPERATION_SUCCESSFUL : "نام سازمان نمی تواند تکراری باشد.");
                     
                }
                CompanyVM._Result = message;
                CompanyVM.selectedContacts = companyBLL.getCompanyContactVms(CompanyVM.Id);
                if (result)
                    return RedirectToAction("Index");
                else
                    return View("AddUpdate", CompanyVM);
            }
            else
            {
                return View("AddUpdate", CompanyVM);
            }
            
        }


        public ActionResult Delete(Guid id)
        {

            CompanyBusinessLayer companyBLL = new CompanyBusinessLayer();
            companyBLL.deleteCompanyById(id);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult getCompanies()
        {
            CompanyBusinessLayer companyBLL = new CompanyBusinessLayer();
            List<CompanyViewModel> lstCompanyVM = companyBLL.GetCompanyVMs();

            return Json(JsonConvert.SerializeObject(lstCompanyVM));

        }

        [HttpPost]
        public ActionResult getCompanyContacts(string id)
        {
            Guid _id = Guid.Parse(id);
            CompanyBusinessLayer companyBLL = new CompanyBusinessLayer();
            var lstContacts = companyBLL.getCompanyContacts(_id);
            return Json(JsonConvert.SerializeObject(lstContacts));

        }

        [HttpPost]
        public ActionResult getOutsideContacts(Guid? companyId )
        {
            CompanyBusinessLayer companyBLL = new CompanyBusinessLayer();
            List<ContactViewModel> lstContactVM = companyBLL.GetOutsideContactVMs(companyId);
            return Json(JsonConvert.SerializeObject(lstContactVM));

        }


    }
}