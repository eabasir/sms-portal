using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SMSPortalWebPanel.ViewModels
{
    public class CompanyViewModel : BaseViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "ورود نام الزامی است")]
        [MaxLength(50, ErrorMessage = "نام سازمان نمی تواند بیشتز از 20 کاراکتر باشد")]
        public string Name { get; set; }

      
        public string Address { get; set; }

        public string Number { get; set; }

       
        public List<ContactViewModel> selectedContacts { get; set; }

   
    }


    public class CompanyListViewModel : BaseViewModel
    {
        public List<CompanyViewModel> CompanyVMs { get; set; }
    }



}