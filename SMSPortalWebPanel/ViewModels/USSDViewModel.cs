using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSPortalWebPanel.ViewModels
{
    public class USSDViewModel : BaseViewModel
    {
       
        public string SendMessage { get; set; }
        public string ReceivedMessage { get; set; }
        public string DTSend { get; set; }
        public bool IsSent { get; set; }
        public bool ISPrefered { get; set; }
        public string PreferedTitle { get; set; }
    }

    public class USSDListViewModel : BaseViewModel
    {
        public string SIM_Number { get; set; }

        public List<USSDViewModel> UssdVMs { get; set; }
    }

    public class SendNewUSSD {
        public string SendMessage { get; set; }
        public bool ISPrefered { get; set; }
        public string PreferedTitle { get; set; }
        public string SIM_Number { get; set; }

    }



}
