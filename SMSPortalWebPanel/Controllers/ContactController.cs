using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMSPortalWebPanel.Filters;
using SMSPortalWebPanel.Models;
using SMSPortalWebPanel.ViewModels;
using SMSPortalDBDataLibrary;
using Newtonsoft.Json;
using System.IO;

namespace SMSPortalWebPanel.Controllers
{
    [PortalAuthorizatoin]
    public class ContactController : Controller
    {

        [HeaderFilter]
        public ActionResult Index()
        {
            ContactListViewModel contactListVM = new ContactListViewModel();
            ContactBusinessLayer bll = new ContactBusinessLayer();
            List<ContactViewModel> lstContactVM = bll.GetContactVMs();

            contactListVM.ContactVMs = lstContactVM;

            return View("Index", contactListVM);
        }


        [HeaderFilter]
        public ActionResult AddUpdate(Guid? id)
        {
            ContactAddUpdateViewModel contactAddUpdateVM = new ContactAddUpdateViewModel();

            if (id.HasValue)
            {
                contactAddUpdateVM = new ContactBusinessLayer().getContactViewModelById((Guid)id);

            }
            else
            {
                contactAddUpdateVM.CompanyList = new ContactBusinessLayer().getSelectListItemsForCompanies();
            }


            return View("AddUpdate", contactAddUpdateVM);
        }

        [HeaderFilter]
        public ActionResult GroupAdd()
        {
            ContactAddUpdateViewModel contactAddUpdateVM = new ContactAddUpdateViewModel
            {
                CompanyList = new ContactBusinessLayer().getSelectListItemsForCompanies()
            };
            return View("GroupAdd", contactAddUpdateVM);
        }


        [HeaderFilter]
        public ActionResult Save(ContactAddUpdateViewModel contactAddUpdateVM)
        {
            ContactBusinessLayer contactBLL = new ContactBusinessLayer();

            if (ModelState.IsValid)
            {
                bool result = false;
                string message = string.Empty;
                if (contactAddUpdateVM.Id != Guid.Empty)
                {

                    result = contactBLL.updateContact(contactAddUpdateVM);
                    message = (result ? SMSPortalCross.Constants.OPERATION_SUCCESSFUL : "شماره تلفن کاربر نمی تواند تکراری باشد.");

                }
                else
                {
                    result = contactBLL.addNewContact(contactAddUpdateVM);
                    message = (result ? SMSPortalCross.Constants.OPERATION_SUCCESSFUL : "شماره تلفن کاربر نمی تواند تکراری باشد.");

                }
                if (result)
                    return RedirectToAction("Index");
                else
                {
                    contactAddUpdateVM._Result = message;
                    contactAddUpdateVM.CompanyList = contactBLL.getSelectListItemsForCompanies();
                    return View("AddUpdate", contactAddUpdateVM);
                }


            }
            else
            {
                contactAddUpdateVM.CompanyList = contactBLL.getSelectListItemsForCompanies();
                return View("AddUpdate", contactAddUpdateVM);
            }


        }

        [HttpPost]
        public ActionResult SaveGroup(string data) {

            ContactGroupAddViewModel contactGroupAdd = JsonConvert.DeserializeObject<ContactGroupAddViewModel>(data);

            bool result = new ContactBusinessLayer().groupAddContact(contactGroupAdd);

            myJsonResult jsonResult = new myJsonResult();
         
            if (result)
            {
                jsonResult.Result = "مخاطبین با موفقیت اضافه شدند.";
                return Json(JsonConvert.SerializeObject(jsonResult));
            }
            else
            {
                jsonResult.Error = "خطا در اضافه کردن مخاطبین فایل";
                return Json(JsonConvert.SerializeObject(jsonResult));

            }
        }

        public ActionResult Delete(Guid id)
        {

            ContactBusinessLayer contactBLL = new ContactBusinessLayer();
            contactBLL.deleteContactById(id);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult getContacts()
        {
            ContactBusinessLayer contactBLL = new ContactBusinessLayer();
            List<ContactViewModel> lstContactVM = contactBLL.GetContactVMs();

            return Json(JsonConvert.SerializeObject(lstContactVM));

        }

        [HeaderFilter]
        public FileResult DownloadErrors(string id)
        {
            string fName = Path.Combine(Server.MapPath("~/Uploads/"), id + ".xlsx");
            byte[] fileBytes = System.IO.File.ReadAllBytes(fName);
            var response = new FileContentResult(fileBytes, "application/octet-stream");
            response.FileDownloadName = id + ".xlsx";
            return response;
        }
    }

}
