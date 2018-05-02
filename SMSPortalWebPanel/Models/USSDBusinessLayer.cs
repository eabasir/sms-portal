using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMSPortalDBDataLibrary;
using System.Data.Entity;
using SMSPortalWebPanel.ViewModels;

namespace SMSPortalWebPanel.Models
{
    public class USSDBusinessLayer
    {

        public USSDListViewModel GetUSSDs(Guid simId)
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    
                    DateTime start = DateTime.Now.AddMonths(-1);

                    var q = (from x in db.USSDs
                             where x.TFDTSend >= start
                             orderby x.TFDTSend descending
                             select x).ToList();

                    if (q != null)
                    {
                        USSDListViewModel ussdListVM = new USSDListViewModel();
                        ussdListVM.UssdVMs = new List<USSDViewModel>();

                        foreach (USSD ussd in q)
                        {
                            ussdListVM.UssdVMs.Add(new USSDViewModel()
                            {
                                SendMessage = ussd.TFSend,
                                ReceivedMessage = ussd.TFReceive,
                                DTSend = ussd.TFDTSendFa,
                                ISPrefered = ussd.TFIsPrefred,
                                PreferedTitle = ussd.TFPreferdTitle,
                                IsSent = ussd.TFIsSent
                            });

                        }

                        return ussdListVM;
                    }
                    else
                        return null;


                }
            }
            catch
            {
                return null;
            }


        }

        public void sendNew(SendNewUSSD vm)
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities()) {

                    DateTime dt = DateTime.Now;
                    Guid simId = new SIMBusinessLayer().getSimByNumber(vm.SIM_Number).TFId;

                    db.USSDs.Add(new USSD()
                    {
                        TFId = Guid.NewGuid(),
                        TFSim_Id = simId,
                        TFSend = vm.SendMessage,
                        TFDTSend = dt,
                        TFDTSendFa = SMSPortalCross.Utils.Date.CompleteGregorianToPersian(dt),
                        TFIsPrefred = vm.ISPrefered,
                        TFPreferdTitle = vm.PreferedTitle

                    });

                    db.SaveChanges();
                }
            }
            catch { }
        }
    }
}
