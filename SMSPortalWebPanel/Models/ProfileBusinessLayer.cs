using SMSPortalCross;
using SMSPortalDBDataLibrary;
using SMSPortalWebPanel.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SMSPortalWebPanel.Models
{
    public class ProfileBusinessLayer
    {
        public bool updateProfile(ProfileViewModel profileVM)
        {
            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
            {
                try
                {

                    var q = (from x in db.Users where x.TFUserName == profileVM.User_UserName select x).FirstOrDefault();

                    if (q.TFPassword != SMSPortalCross.Utils.StringUtils.getMd5(profileVM.OldPassword))
                        return false;

                    q.TFName = profileVM.FirstName;
                    q.TFFamily = profileVM.LastName;
                    q.TFUserName = profileVM.UserName;
                    q.TFPassword = SMSPortalCross.Utils.StringUtils.getMd5(profileVM.NewPassword);

                    db.Entry(q).State = EntityState.Modified;

                    db.SaveChanges();
                    return true;

                }
                catch (Exception e)
                {
                    return false;
                }

            }
        }

        public bool handleFirstLogin(EditPasswordViewModel editpasswordVM)
        {

            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {

                    if (db.Users.Count() == 0)
                        return makeFirstAdminUser(editpasswordVM);
                    else
                        return updatePasswordForFirstTime(editpasswordVM);

                }
            }
            catch
            {
            }
            return false;

        }


        private bool updatePasswordForFirstTime(EditPasswordViewModel editpasswordVM) {

            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
            {
                try
                {
                    var q = (from x in db.Users where x.TFUserName == editpasswordVM.User_UserName select x).FirstOrDefault();
                    
                    if (q.TFPassword != SMSPortalCross.Utils.StringUtils.getMd5(editpasswordVM.OldPassword))
                        return false;

                    q.TFPassword = SMSPortalCross.Utils.StringUtils.getMd5(editpasswordVM.NewPassword);
                    q.TFIsFirstLogin = false;

                    db.Entry(q).State = EntityState.Modified;

                    db.SaveChanges();
                    return true;

                }
                catch (Exception e)
                {
                    return false;
                }

            }

        }
      
        private bool makeFirstAdminUser(EditPasswordViewModel editpasswordVM)
        {

            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
            {
                try
                {
                    if (editpasswordVM.OldPassword == MvcApplication.DefaultPassword)
                    {

                        User firstAdminUser = new User
                        {

                            TFUserName = MvcApplication.DefaultUserName,
                            TFPassword = SMSPortalCross.Utils.StringUtils.getMd5(editpasswordVM.NewPassword),
                            TFName = MvcApplication.DefaultFirstName,
                            TFFamily = MvcApplication.DefaultLastName,
                            TFId = Guid.NewGuid(),
                            TFUserLevel = (int)Enums.UserLevel.Admin,
                            TFIsFirstLogin = false
                            

                        };

                        db.Users.Add(firstAdminUser);
                        db.SaveChanges();
                        return true;
                    }
                    else
                        return false;

                }
                catch (Exception e)
                {
                    return false;
                }

            }

        }

    }
}