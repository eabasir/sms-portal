using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMSPortalWebPanel.ViewModels
{
    public class EditPasswordViewModel : BaseViewModel
    {
     
        [StringLength(20, MinimumLength = 5, ErrorMessage = "کلمه عبور باید حداقل 6 و حداکثر 20 کاراکتر باشد")]
        public string OldPassword { get; set; }

        [StringLength(20, MinimumLength = 5, ErrorMessage = "کلمه عبور باید حداقل 6 و حداکثر 20 کاراکتر باشد")]
        public string NewPassword { get; set; }

        [StringLength(20, MinimumLength = 5, ErrorMessage = "کلمه عبور باید حداقل 6 و حداکثر 20 کاراکتر باشد")]
        public string ReNewPassword { get; set; }

    }
}