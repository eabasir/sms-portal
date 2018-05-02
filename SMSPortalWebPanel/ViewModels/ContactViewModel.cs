using SMSPortalWebPanel.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SMSPortalWebPanel.ViewModels
{
    public class ContactViewModel : BaseViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "ورود نام الزامی است")]
        [MaxLength(20, ErrorMessage = "نام نمی تواند بیشتز از 20 کاراکتر باشد")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ورود نام خانوادگی الزامی است")]
        [MaxLength(30, ErrorMessage = "نام خانوادگی نمی تواند بیشتر از 30 کاراکتر باشد")]
        public string Family { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; }

        public List<string> Companies { get; set; }
        public string Address { get; set; }

        [PhoneListValidation(ErrorMessage = "شماره وارد شده معتبر نمی باشد")]
        public List<string> Numbers { get; set; }

      }


    public class ContactListViewModel : BaseViewModel
    {
        public List<ContactViewModel> ContactVMs { get; set; }
    }


    public class ContactAddUpdateViewModel : ContactViewModel
    {
        public List<SelectListItem> CompanyList { get; set; }
    }

    public class ContactGroupAddViewModel {
        public List<FileContactsViewModel> Contacts { get; set; }
        public List<string> Companies { get; set; }
    }

    public class FileContactsViewModel {
        public string Name { get; set; }
        public string Family { get; set; }
        public string Email { get; set; }
        public List<string> Numbers { get; set; }

    }


}