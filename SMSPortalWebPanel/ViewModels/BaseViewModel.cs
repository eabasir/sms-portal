using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMSPortalWebPanel.ViewModels
{
    public class BaseViewModel : UserViewModel
    {
        public int UnreadMessageCount { get; set; }
        public string _Result { get; set; }

    }
}