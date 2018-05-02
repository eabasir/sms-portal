using SMSPortalDBDataLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSPortalWebPanel.Models
{
    public class SendBoxBusinessLayer
    {

         public object getSents()
        {

            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {

                    return (from x in db.SendBoxes
                            orderby x.TFDateTimeCreate descending
                            select new
                            {
                                Id = x.TFId,
                                Message = x.Message.TFContent,
                                Counter = (from y in db.SendBox_Phone where y.TFSendBox_Id == x.TFId select y).Count(),
                                User = x.Queue.User.TFUserName,
                                DTCreate = x.TFDateTimeCreateFA,
                                Type = x.Queue.TFScheduleType,
                            }).ToList();
                }

            }
            catch { }
            return null;
        }


        public object getSentContacts(Guid id)
        {

            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {

                    return (from x in db.SendBox_Phone
                            where x.TFSendBox_Id == id
                            orderby x.TFDateTimeSend descending
                            select new
                            {
                                Id = x.TFId,
                                Name = x.Phone.Contact.TFName,
                                Family = x.Phone.Contact.TFFamily,
                                Number = x.Phone.TFNumber,
                                SIM = x.SIM.TFNumber,
                                DTSend = x.TFDateTimeSendGSMFA,
                                Delevered = x.TFIsDelivered,
                                DTDeliver = x.TFDateTimeDeliveryFA

                            }).ToList();
                }

            }
            catch { }
            return null;
        }

      

    }
}