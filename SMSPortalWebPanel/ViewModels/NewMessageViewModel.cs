using SMSPortalWebPanel.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SMSPortalWebPanel.ViewModels
{
    public class NewMessageViewModel : BaseViewModel
    {
        
        [Required(ErrorMessage = "ورود متن پیام الزامی است")]
        [MaxLength(300, ErrorMessage = "متن یام نمی تواند بیشتر از 300 کاراکتر باشد")]
        public string Message { get; set; }

        public string StrDTSend { get; set; } 
        
        public int RepeatType { get; set; }

        [PhoneListValidation(ErrorMessage = "شماره های وارد شده معتبر نمی باشد")]
        public List<string> Numbers { get; set; }     

        public string StartRange { get; set; }
        public string FinishRange { get; set; }


    }



}