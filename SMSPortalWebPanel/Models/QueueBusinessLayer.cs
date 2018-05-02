using SMSPortalDBDataLibrary;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SMSPortalWebPanel.Models
{
    public class QueueBusinessLayer
    {

        public object getQueues()
        {

            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {

                    return (from x in db.Queues
                            where (from qp in db.Queue_Phone select qp.TFQueue_Id).Contains(x.TFId)
                            orderby x.TFDateTimeRequest descending
                            select new
                            {
                                Id = x.TFId,
                                Message = x.Message.TFContent,
                                Counter = (from y in db.Queue_Phone where y.TFQueue_Id == x.TFId select y).Count(),
                                User = x.User.TFUserName,
                                DTCreate = x.TFDateTimeCreateFA,
                                DTRequest = x.TFDateTimeRequestFA,
                                Type = x.TFScheduleType,
                                Status = x.TFEnable
                            }).ToList();
                }

            }
            catch { }
            return null;

        }

        public object getQueueContacts(Guid id)
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {

                    return (from x in db.Queue_Phone
                            where x.TFQueue_Id == id

                            select new
                            {
                                Id = x.TFId,
                                Name = x.Phone.Contact.TFName,
                                Family = x.Phone.Contact.TFFamily,
                                Number = x.Phone.TFNumber,
                                Proccess = x.TFIsUnderProcess,
                                Status = x.TFEnable
                            }).ToList();
                }

            }
            catch { }
            return null;
        }

        public void changeQueueStatus(Guid id, bool status)
        {

            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    var q = (from x in db.Queues where x.TFId == id select x).FirstOrDefault();
                    q.TFEnable = status;
                    db.Entry(q).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
            catch { }
        }

        public void restartQueues()
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    var q = (from x in db.Queue_Phone where x.TFIsUnderProcess == true select x).ToList();

                    foreach (var qp in q)
                    {
                        qp.TFIsUnderProcess = false;
                        db.Entry(qp).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }

            }
            catch { }

        }

        /// <summary>
        /// Queue itself should not be deleted because all data is accessed by queueId.
        /// Instead, all queue_phones related to queue id should be deleted.
        /// </summary>
        /// <param name="id"></param>
        public void removeQueue(Guid id)
        {

            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    var qp = (from x in db.Queue_Phone where x.TFQueue_Id == id select x).ToList();
                    db.Queue_Phone.RemoveRange(qp);
                    db.SaveChanges();

                    SMSPortalCross.Utils.Scheduler sceduler = new SMSPortalCross.Utils.Scheduler();
                    sceduler.removeSchedule(id);
                 
                }

            }
            catch (Exception e)
            {
            }
        }


        public string changeQueueContactStatus(Guid id, bool status)
        {

            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    var q = (from x in db.Queue_Phone where x.TFId == id select x).FirstOrDefault();
                    q.TFEnable = status;
                    db.Entry(q).State = EntityState.Modified;

                    db.SaveChanges();
                    return q.Phone.TFNumber.ToString();
                }

            }
            catch { }
            return null;
        }


        public string removeQueueContact(Guid id)
        {

            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    var q = (from x in db.Queue_Phone where x.TFId == id select x).FirstOrDefault();
                    string number = q.Phone.TFNumber.ToString();
                    db.Queue_Phone.Remove(q);
                    db.SaveChanges();
                    return number;
                }

            }
            catch { }
            return null;
        }


    }
}