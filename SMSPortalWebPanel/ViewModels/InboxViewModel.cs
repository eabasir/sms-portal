using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSPortalWebPanel.ViewModels
{
    public class InboxViewModel
    {
        public Guid Id { get; set; }

        public string SenderNumber { get; set; }
        public string SimNumber { get; set; }
        public string Message { get; set; }
        public string DTRecieved { get; set; }
        public bool IsRead { get; set; }

    }

    public class InboxListViewModel : BaseViewModel
    {
        public List<InboxViewModel> InboxVMs { get; set; }
    }

}
