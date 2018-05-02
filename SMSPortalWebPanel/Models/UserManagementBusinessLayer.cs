using SMSPortalCross;
using SMSPortalDBDataLibrary;
using SMSPortalWebPanel.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSPortalWebPanel.Models
{
    public class UserManagementBusinessLayer
    {

        public List<UserManagementViewModel> getAllUsers()
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    return (from x in db.Users
                            select new UserManagementViewModel()
                            {
                                Id = x.TFId,
                                Name = x.TFName,
                                Family = x.TFFamily,
                                _UserName = x.TFUserName,
                                isAdmin = x.TFUserLevel == (int)Enums.UserLevel.Admin ? true : false,
                                
                                
                            }).ToList();
                }
            }
            catch { }

            return null;
        }

        public UserManagementAddUpdateViewModel getUserManagementViewModelById(Guid id)
        {

            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
            {
                User user = db.Users.FirstOrDefault(x => x.TFId == id);


                if (user != null)
                {

                    return new UserManagementAddUpdateViewModel()
                    {
                        Id = user.TFId,
                        _UserName = user.TFUserName,
                        Name = user.TFName,
                        Family = user.TFFamily,
                        SimList = getSelectListItemsForSims(),
                        isAdmin = user.TFUserLevel == (int) Enums.UserLevel.Admin ? true : false,
                        SelectedSims = (from x in db.SIM_User where x.TFUser_Id == user.TFId select new User_Sim {
                            SimId = x.TFSIM_Id,
                            Number = x.SIM.TFNumber,
                            Max = x.TFMax
                        }).ToList()
                       
                    };


                }

                return null;
            }

        }


        public List<SelectListItem> getSelectListItemsForSims()
        {
            return new SIMBusinessLayer().GetSims().Select(x => new SelectListItem
            {
                Text = x.TFNumber,
                Value = x.TFId.ToString()
            }).ToList();


        }

        
        public bool addNewUser(UserManagementViewModel userManagementVM)
        {
            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                try
                {
                    
                    User user = new User()
                    {
                        TFId = Guid.NewGuid(),
                        TFName = userManagementVM.Name,
                        TFFamily = userManagementVM.Family,
                        TFUserName = userManagementVM._UserName,
                        TFPassword = SMSPortalCross.Utils.StringUtils.getMd5(userManagementVM.Password),
                        TFUserLevel = userManagementVM.isAdmin ? (int)Enums.UserLevel.Admin : (int)Enums.UserLevel.User,
                        TFIsFirstLogin = true
                    };

                    if (userManagementVM.SelectedSims != null)
                    {
                        foreach (User_Sim user_sim in userManagementVM.SelectedSims)
                        {
                            db.SIM_User.Add(new SIM_User
                            {
                                TFId = Guid.NewGuid(),
                                TFUser_Id = user.TFId,
                                TFSIM_Id = user_sim.SimId,
                                TFMax = user_sim.Max
                            });
                        }
                    }
                    db.Users.Add(user);
                    
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;

                }

        }

        public bool updateUser(UserManagementViewModel userManagementVM)
        {
            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
            {
                try
                {
                    User user = getUserById(userManagementVM.Id);
                    if (user != null)
                    {
                        user.TFUserName = userManagementVM._UserName;
                        user.TFPassword = userManagementVM.Password != null ? SMSPortalCross.Utils.StringUtils.getMd5(userManagementVM.Password) : user.TFPassword ;
                        user.TFName = userManagementVM.Name;
                        user.TFFamily = userManagementVM.Family;
                        user.TFUserLevel = userManagementVM.isAdmin ? (int)Enums.UserLevel.Admin : (int)Enums.UserLevel.User;

                        db.Entry(user).State = EntityState.Modified;


                        var q = (from x in db.SIM_User where x.TFUser_Id == user.TFId select x).ToList();
                        db.SIM_User.RemoveRange(q);

                        if (userManagementVM.SelectedSims != null)
                        {
                            foreach (User_Sim  user_sim in userManagementVM.SelectedSims.Distinct())
                            {
                                db.SIM_User.Add(new SIM_User
                                {
                                    TFId = Guid.NewGuid(),
                                    TFUser_Id = user.TFId,
                                    TFSIM_Id = user_sim.SimId,
                                    TFMax = user_sim.Max    
                                });

                            }
                        }
                        
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

        public void deleteUserById(Guid _id)
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    User user = db.Users.FirstOrDefault(x => x.TFId == _id);
                    db.Users.Remove(user);

                    var lstUserSims = (from x in db.SIM_User where x.TFUser_Id == _id select x).ToList();
                    db.SIM_User.RemoveRange(lstUserSims);
                    
                    db.SaveChanges();
                }
            }
            catch { }
        }


        public User getUserById(Guid id)
        {

            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
            {
                User user = db.Users.FirstOrDefault(x => x.TFId == id);
                return user;
            }

        }

        public User getUserByUserName(String username)
        {

            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
            {
                User user = db.Users.FirstOrDefault(x => x.TFUserName == username);
                return user;
            }

        }


    }
}