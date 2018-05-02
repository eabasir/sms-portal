using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SMSPortalWebPanel.Models;
using SMSPortalCross;
using SMSPortalWebPanel.ViewModels;

namespace SMSPortalWebPanel.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        // GET: Authentication
        public ActionResult Login()
        {
            return View();
        }

        [CaptchaMvc.Attributes.CaptchaVerify("Captcha is not valid")]
        [HttpPost]
        public ActionResult DoLogin(AuthenticationViewModel authenticationVM)
        {
            if (ModelState.IsValid)
            {
                AuthenticationBusinessLayer bal = new AuthenticationBusinessLayer();

                UserViewModel userVM = bal.GetUserValidity(authenticationVM);
                Enums.UserLevel status = userVM.User_Status;


                if (status == Enums.UserLevel.NotAuthenticated)
                {
                    ModelState.AddModelError("CredentialError", "نام کاربری یا رمز عبور اشتباه است");
                    return View("Login");
                }

                FormsAuthentication.SetAuthCookie(authenticationVM.UserName, false);

                Session["Status"] = (int)status;
                Session["FirstName"] = userVM.User_FirstName;
                Session["LastName"] = userVM.User_LastName;

                if (status == Enums.UserLevel.DefaultOrFirstTime || userVM.User_IsFirstLogin)
                    return RedirectToAction("EditPassword", "Profile");
                else
                    return RedirectToAction("Index", "Dashboard");

            }
            else
            {
                ModelState.AddModelError("CredentialError", "عبارت امنیتی وارد شده صحیح نمی باشد");

                return View("Login");
            }
        }
    }
}