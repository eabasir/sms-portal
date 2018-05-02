using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSPortalWebPanel.ViewModels
{
    public class ReportViewModel 
    {
      
        public string Message { get; set; }
        public bool iSSent { get; set; }
        public string SIM { get; set; }
        public string Contact { get; set; }
        public string DateTimeFa { get; set; }

        public List<string> Details { get; set; }
        
    }

    public class SearchViewModel {
        public string Type { get; set; }
        public string Text { get; set; }
        public string DTStart { get; set; }
        public string DTFinish { get; set; }

    }



}
