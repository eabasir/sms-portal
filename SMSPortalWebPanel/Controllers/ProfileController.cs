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

    public class ProfileController : Controller
    {
        [PortalAuthorizatoin]
        [HeaderFilter]
        public ActionResult Index()
        {
            ProfileViewModel profileVM = new ProfileViewModel();
            profileVM.UserName = HttpContext.User.Identity.Name;


            return View("Index", profileVM);
        }

        [PortalAuthorizatoin]
        [HeaderFilter]
        public ActionResult Save(ProfileViewModel profileVM)
        {


            if (ModelState.IsValid)
            {
                string message = string.Empty;

                if (profileVM.NewPassword != profileVM.ReNewPassword)
                {
                    profileVM._Result = "رمز عبور و تکرار آن مطابقت ندارد";
                    return View("Index", profileVM);
                }

                ProfileBusinessLayer profileBll = new ProfileBusinessLayer();

                if (profileBll.updateProfile(profileVM))
                {
                    return RedirectToAction("Logout", "Authentication");
                }
                else
                {
                    profileVM._Result = "رمز عبور فعلی نادرست است.";
                    return View("Index", profileVM);
                }

            }
            else
            {

                return View("Index", profileVM);
            }


        }

        [FirstTimeFilter]
        [HeaderFilter]
        public ActionResult EditPassword()
        {

            EditPasswordViewModel editPasswordVM = new EditPasswordViewModel();
            return View("EditPassword", editPasswordVM);
        }


        [FirstTimeFilter]
        [HeaderFilter]
        public ActionResult UpdatePassword(EditPasswordViewModel editPasswordVM)
        {

            if (ModelState.IsValid)
            {
                string message = string.Empty;

                if (editPasswordVM.NewPassword == editPasswordVM.OldPassword)
                {
                    editPasswordVM._Result = "رمز عبور قبلی و جدید نمی تواند یکسان باشد.";
                    return View("EditPassword", editPasswordVM);
                }


                if (editPasswordVM.NewPassword != editPasswordVM.ReNewPassword)
                {
                    editPasswordVM._Result = "رمز عبور و تکرار آن مطابقت ندارد";
                    return View("EditPassword", editPasswordVM);
                }

                ProfileBusinessLayer profileBll = new ProfileBusinessLayer();

    
                if (profileBll.handleFirstLogin(editPasswordVM))
                    return RedirectToAction("Logout", "Authentication");
                else
                {
                    editPasswordVM._Result = "رمز عبور فعلی نادرست است.";
                    return View("EditPassword", editPasswordVM);
                }

            }
            else
            {

                return View("EditPassword", editPasswordVM);
            }
        }


    }
}