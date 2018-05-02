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
    public class UserManagementController : Controller
    {
        [HeaderFilter]
        public ActionResult Index()
        {
            UserManagementListViewModel userManagementListVM = new UserManagementListViewModel();
            userManagementListVM.UserManagementVms = new UserManagementBusinessLayer().getAllUsers();

            return View("Index" , userManagementListVM);
        }

        [HeaderFilter]
        public ActionResult AddUpdate(Guid? id)
        {
            UserManagementAddUpdateViewModel userManagementAddUpdateVM = new UserManagementAddUpdateViewModel();
            
            if (id.HasValue)
            {
                userManagementAddUpdateVM = new UserManagementBusinessLayer().getUserManagementViewModelById((Guid)id);

            }
            else
            {
                userManagementAddUpdateVM.SimList = new UserManagementBusinessLayer().getSelectListItemsForSims();
            }


            return View("AddUpdate", userManagementAddUpdateVM);
        }

        [HeaderFilter]
        public ActionResult Save(UserManagementAddUpdateViewModel userManagementAddUpdateVM )
        {
            UserManagementBusinessLayer userManagementBLL = new UserManagementBusinessLayer();

            if (ModelState.IsValid)
            {
                
                bool result = false;
                string message = string.Empty;

                string pass = userManagementAddUpdateVM.Password;
                string re_pass = userManagementAddUpdateVM.Re_Password;

                if (pass != re_pass)
                {
                    userManagementAddUpdateVM._Result = "رمز عبور و تکرار آن مطابقت ندارد.";
                    userManagementAddUpdateVM.SimList = userManagementBLL.getSelectListItemsForSims();
                    return View("AddUpdate", userManagementAddUpdateVM);
                }

                if (userManagementAddUpdateVM.Id != Guid.Empty)
                {

                    result = userManagementBLL.updateUser(userManagementAddUpdateVM);
                    message = (result ? SMSPortalCross.Constants.OPERATION_SUCCESSFUL : "خطا در ثبت اطلاعات کاربر.");

                }
                else
                {
                    if (userManagementBLL.getUserByUserName(userManagementAddUpdateVM._UserName) == null)
                    {

                        result = userManagementBLL.addNewUser(userManagementAddUpdateVM);
                        message = (result ? SMSPortalCross.Constants.OPERATION_SUCCESSFUL : "خطا در ثبت اطلاعات کاربر.");
                    }
                    else
                    {
                        message = "این نام کاربری قبلا در سامانه ثبت شده است";
                    }

                }
                if (result)
                    return RedirectToAction("Index");
                else
                {
                    userManagementAddUpdateVM._Result = message;
                    userManagementAddUpdateVM.SimList = userManagementBLL.getSelectListItemsForSims();
                    return View("AddUpdate", userManagementAddUpdateVM);
                }


            }
            else
            {
                userManagementAddUpdateVM.SimList = userManagementBLL.getSelectListItemsForSims();
                return View("AddUpdate", userManagementAddUpdateVM);
            }


        }


        public ActionResult Delete(Guid id)
        {

            UserManagementBusinessLayer userManagementBLL = new UserManagementBusinessLayer();
            userManagementBLL.deleteUserById(id);
            return RedirectToAction("Index");
        }
    }


   
}