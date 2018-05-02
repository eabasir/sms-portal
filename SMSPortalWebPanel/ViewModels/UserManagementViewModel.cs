using SMSPortalCross;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSPortalWebPanel.ViewModels
{
    public class UserManagementViewModel : BaseViewModel
    {

        public Guid Id { get; set; }

        [StringLength(20, MinimumLength = 5, ErrorMessage = "نام کاربری باید حداقل 5 و حداکثر 20 کاراکتر باشد")]
        public string _UserName { get; set; }

        public string Password { get; set; }

        public string Re_Password { get; set; }

        [Required(ErrorMessage = "ورود نام الزامی است")]
        [MaxLength(20, ErrorMessage = "نام نمی تواند بیشتز از 20 کاراکتر باشد")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ورود نام خانوادگی الزامی است")]
        [MaxLength(30, ErrorMessage = "نام خانوادگی نمی تواند بیشتر از 30 کاراکتر باشد")]
        public string Family { get; set; }

        public bool isAdmin { get; set; }

        public List<User_Sim> SelectedSims { get; set; }

    }
    
    public class User_Sim
    {
        public Guid SimId { get; set; }

        [Required(ErrorMessage = "ورود شماره تلفن ضروری است.")]
        public string Number { get; set; }

        public int? Max { get; set; }

    }

    public class UserManagementListViewModel : BaseViewModel {

        public List<UserManagementViewModel> UserManagementVms { get; set; }
    }


    public class UserManagementAddUpdateViewModel : UserManagementViewModel
    {
        public List<SelectListItem> SimList { get; set; }
    }
}