using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSPortalWebPanel.ViewModels
{


    public class DashboardViewModel : BaseViewModel {

        public List<SimData> SIMDatas { get; set; }
    }

    public class SimData {
        public string Number { get; set; }

        public string Sents { get; set; }
        public string Recieveds { get; set; }
        public string Charge { get; set; }

    }

    public class ChartViewModel
    {
        public List<string> labels { get; set; }

        public List<ChartDataSet> datasets { get; set; }

        
    }

    public class ChartDataSet {
        public List<int> data { get; set; }
        public string label { get; set; }
    }
}