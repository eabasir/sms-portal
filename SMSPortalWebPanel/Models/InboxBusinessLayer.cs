using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMSPortalDBDataLibrary;
using System.Data.Entity;
using SMSPortalWebPanel.ViewModels;
using System.Text.RegularExpressions;

namespace SMSPortalWebPanel.Models
{
    public class InboxBusinessLayer
    {

        public InboxListViewModel GetInbox()
        {
            try
            {

                InboxListViewModel inboxListVM = new InboxListViewModel();
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    var q = (from x in db.Inboxes
                             select new InboxViewModel
                             {
                                 Id = x.TFId,
                                 Message = x.TFMessage,
                                 SenderNumber = x.TFSenderNumber,
                                 SimNumber = x.SIM.TFNumber,
                                 DTRecieved = x.TFDateTimeFA,
                                 IsRead = x.TFIsRead

                             }).ToList();

                    foreach (InboxViewModel inboxVM in q) {

                        inboxVM.Message = inboxVM.Message.TrimEnd('\0');
                    }

                    var unReads = (from x in db.Inboxes where x.TFIsRead == false select x).ToList();

                    foreach (Inbox inbox in unReads)
                    {
                        inbox.TFIsRead = true;
                        db.Entry(inbox).State = EntityState.Modified;
                    }

                    db.SaveChanges();

                    inboxListVM.InboxVMs = q;

                }

                return inboxListVM;
            }
            catch { }
            return null;


        }
    }
}