using SMSPortalDBDataLibrary;
using SMSPortalWebPanel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSPortalWebPanel.Models
{
    public class DashboardBusinessLayer
    {

        public List<SimData> getSimDatas() {

            try {
                
                using (SMSPortalDBEntities db = new SMSPortalDBEntities()) {

                    List<SimData> lstSimData = new List<SimData>();

                    var sims = (from x in db.SIMs select x).ToList();

                    foreach (var sim in sims) {
                        SimData data = new SimData
                        {
                            Number = sim.TFNumber,
                            Charge = sim.TFCharge != null ? sim.TFCharge.ToString() : "-" ,
                            Sents = (from x in db.SendBox_Phone where x.TFSim_Id == sim.TFId select x).Count().ToString() ,
                            Recieveds = (from x in db.Inboxes where x.TFSim_Id == sim.TFId select x).Count().ToString(),
                            
                        };
                        lstSimData.Add(data);

                    }
                    return lstSimData;
                }
            }
            catch(Exception e) {

            }
            return null;
        }

        public object getBarChartData()
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    List<string> labels = new List<string>();
                    List<int> sendBoxCounts = new List<int>();
                    List<int> undeliveredCounts = new List<int>();
                    List<int> inBoxCounts = new List<int>();

                    for (int i = -6; i <= 0; i++)
                    {
                        DateTime dt1 = DateTime.Today.AddDays(i);
                        DateTime dt2 = DateTime.Today.AddDays(i + 1);

                        labels.Add(SMSPortalCross.Utils.Date.GregorianToPersian(dt1));

                        int sendBoxCount = (from x in db.SendBox_Phone
                                            where x.TFDateTimeSend >= dt1 && x.TFDateTimeSend < dt2
                                            select x).Count();

                        int undeliveredCount = (from x in db.SendBox_Phone
                                            where x.TFDateTimeSend >= dt1 && x.TFDateTimeSend < dt2 && !x.TFIsDelivered
                                            select x).Count();

                        int inboxCount = (from x in db.Inboxes
                                          where x.TFDateTime >= dt1 && x.TFDateTime < dt2
                                          select x).Count();

                        sendBoxCounts.Add(sendBoxCount);
                        undeliveredCounts.Add(undeliveredCount);
                        inBoxCounts.Add(inboxCount);

                    }

                    ChartViewModel chartViewModel = new ChartViewModel()
                    {

                        labels = labels
                    };
                    var datasets = new List<ChartDataSet>();
                    datasets.Add(new ChartDataSet
                    {
                        data = sendBoxCounts,
                        label = "پیام های ارسال شده"
                    });
                    datasets.Add(new ChartDataSet
                    {
                        data = undeliveredCounts,
                        label = "پیام های دلیور نشده"
                    });
                    datasets.Add(new ChartDataSet
                    {
                        data = inBoxCounts,
                        label = "پیام های دریافت شده"
                    });

                    chartViewModel.datasets = new List<ChartDataSet>();
                    chartViewModel.datasets = datasets;

                    return chartViewModel;

                }
            }
            catch (Exception e) {
                string s = "";
            }

            return null;
        }

        public object getPieChartData()
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    List<string> labels = new List<string> {
                        "پیام های ارسال شده",
                        "پیام های دریافت شده",
                        "پیام های دلیور نشده"
                        };

                    ChartViewModel chartViewModel = new ChartViewModel()
                    {
                        labels = labels,
                        datasets = new List<ChartDataSet> {
                            new ChartDataSet {
                                data = new List<int> {
                                    (from x in db.SendBox_Phone select x).Count(),
                                    (from x in db.Inboxes select x).Count(),
                                    (from x in db.SendBox_Phone where !x.TFIsDelivered select x).Count()

                                },
                                label = "کل پیام ها"
                            }
                          
                        }
                        
                    };


                    return chartViewModel;

                }
            }
            catch { }

            return null;
        }
    }
}