using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SMSPortalCross;
using SMSPortalDBDataLibrary;
using SMSPortalWebPanel.ViewModels;

namespace SMSPortalWebPanel.Models
{


    public class AuthenticationBusinessLayer
    {
        public UserViewModel GetUserValidity(AuthenticationViewModel authenticationVM)
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {

                    if ((from x in db.Users select x).Count() == 0)
                    {
                        if (authenticationVM.UserName == MvcApplication.DefaultUserName && authenticationVM.Password == MvcApplication.DefaultPassword)
                        {
                            UserViewModel userVM = new UserViewModel
                            {
                                User_UserName = MvcApplication.DefaultUserName,
                                User_FirstName = MvcApplication.DefaultFirstName,
                                User_LastName = MvcApplication.DefaultLastName,
                                User_Status = Enums.UserLevel.DefaultOrFirstTime

                            };
                            return userVM;
                        }
                    }

                    string hashPass = SMSPortalCross.Utils.StringUtils.getMd5(authenticationVM.Password);
                    var q = (from x in db.Users where x.TFUserName == authenticationVM.UserName && x.TFPassword == hashPass select x).FirstOrDefault();

                    if (q != null)
                    {
                        UserViewModel userVM = new UserViewModel
                        {
                            User_UserName = q.TFUserName,
                            User_FirstName = q.TFName,
                            User_LastName = q.TFFamily,
                            User_IsFirstLogin = q.TFIsFirstLogin

                        };

                        if (q.TFIsFirstLogin)
                            userVM.User_Status = Enums.UserLevel.DefaultOrFirstTime;
                        else

                        switch (q.TFUserLevel)
                        {
                            case (int)Enums.UserLevel.Admin:
                                userVM.User_Status = Enums.UserLevel.Admin;
                                break;

                            case (int)Enums.UserLevel.User:
                                userVM.User_Status = Enums.UserLevel.User;
                                break;

                            case (int)Enums.UserLevel.NotAuthenticated:
                                userVM.User_Status = Enums.UserLevel.NotAuthenticated;
                                break;
                            default:
                                userVM.User_Status = Enums.UserLevel.NotAuthenticated;
                                break;

                        }

                        return userVM;
                    }
                    else
                        return new UserViewModel
                        {
                            User_Status = Enums.UserLevel.NotAuthenticated
                        };
                }
            }
            catch
            {
                return new UserViewModel
                {
                    User_Status = Enums.UserLevel.NotAuthenticated
                };
            }



        }

        public string getPassword(string userName)
        {

            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    return (from x in db.Users where x.TFUserName == userName select x.TFPassword).FirstOrDefault();
                }
            }
            catch { }
            return null;
        }

    }



}