using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMSPortalWebPanel.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {

        [Required(ErrorMessage = "ورود نام کاربری الزامی است")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "نام کاربری باید حداقل 5 و حداکثر 20 کاراکتر باشد")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "ورود نام الزامی است")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "ورود نام خانوادگی الزامی است")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "ورود رمز عبور قبلی الزامی است")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "کلمه عبور باید حداقل 6 و حداکثر 20 کاراکتر باشد")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "ورود رمز عبور فعلی الزامی است")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "کلمه عبور باید حداقل 6 و حداکثر 20 کاراکتر باشد")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "ورود تکرار رمز عبور الزامی است")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "کلمه عبور باید حداقل 6 و حداکثر 20 کاراکتر باشد")]
        public string ReNewPassword { get; set; }

    }
}