using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSPortalWebPanel.ViewModels
{
    public class SIMViewModel : BaseViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "ورود شماره تلفن ضروری است.")]
        public string Number { get; set; }
        public string Charge { get; set; }

        [Required(ErrorMessage = "مقدار وارد شده باید عددی بین 1025 تا 65535 باشد.")]
        [Range(1025, 65535, ErrorMessage = "مقدار وارد شده باید عددی بین 1025 تا 65535 باشد.")]
        public string Port { get; set; }
        
    }

    public class SIMListViewModel : BaseViewModel
    {
        public List<SIMViewModel> SIMVMs { get; set; }
    }

}
