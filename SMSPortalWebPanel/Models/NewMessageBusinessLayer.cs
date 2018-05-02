using SMSPortalDBDataLibrary;
using SMSPortalWebPanel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSPortalWebPanel.Models
{
    public class NewMessageBusinessLayer
    {

        /// <summary>
        /// Gets all numbers and other data from both queue_phone and sent_phone tables
        /// </summary>
        /// <param name="sendBoxId"></param>
        /// <returns></returns>
        public NewMessageViewModel copySent(Guid sendBoxId) {

            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities()) {

                    var queueId = (from x in db.SendBoxes where x.TFId == sendBoxId select x.TFQueue_Id).FirstOrDefault();

                    var contactsInQueue = (from x in db.Queue_Phone where x.TFQueue_Id == queueId select x.Phone.TFNumber).ToList();
                    var contactsInSent = (from x in db.SendBox_Phone where x.SendBox.TFQueue_Id == queueId select x.Phone.TFNumber).ToList();

                    var numbers = contactsInQueue.Concat(contactsInSent).ToList();

                    return (from x in db.SendBoxes
                            where x.TFQueue_Id == queueId
                            select new NewMessageViewModel
                            {
                                Message = x.Message.TFContent,
                                Numbers = numbers,
                                RepeatType = x.Queue.TFScheduleType,
                                StrDTSend = x.Queue.TFDateTimeRequestFA

                            }).FirstOrDefault();

                }
            }
            catch {
            }

            return null;
        }


        public NewMessageViewModel CopySentPhone(Guid sentContactId)
        {

            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    
                    
                    return (from x in db.SendBox_Phone
                            where x.TFId == sentContactId
                            select new NewMessageViewModel
                            {
                                Message = x.SendBox.Message.TFContent,
                                Numbers = new List<string>() { x.Phone.TFNumber },
                                RepeatType = x.SendBox.Queue.TFScheduleType,
                                StrDTSend = x.SendBox.Queue.TFDateTimeRequestFA

                            }).FirstOrDefault();

                }
            }
            catch
            {
            }

            return null;
        }


        public NewMessageViewModel SendMessageToContact(Guid contactId)
        {

            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {

                    
                    return new NewMessageViewModel() {

                        Numbers = (from x in db.Phones where x.TFContact_Id == contactId select x.TFNumber).ToList()
                    };

                }
            }
            catch
            {
            }

            return null;
        }



    }
}