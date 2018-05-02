using SMSPortalCross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSPortalWebPanel.ViewModels
{
    public class UserViewModel
    {
        public string User_UserName { get; set; }

        public string User_Password { get; set; }

        public string User_FirstName { get; set; }

        public string User_LastName { get; set; }

        public bool User_IsFirstLogin { get; set; }

        public Enums.UserLevel User_Status { get; set; }


    }
}