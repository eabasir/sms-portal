using SMSPortalDBDataLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using SMSPortalCross;

namespace SMSPortalService
{
    public class GSMHandler
    {
        public void QueueMaker(User _user, Message _message, List<Phone> lstPhone, Enums.Schedule _ScheduleType, DateTime _dtSend)
        {
            try
            {

                Guid queue_id = Guid.NewGuid();

                Queue queue = new Queue()
                {
                    TFId = queue_id,
                    TFMessage_Id = _message.TFId,
                    TFScheduleType = (int)_ScheduleType,
                    TFUser_Id = _user.TFId,
                    TFDateTimeCreate = DateTime.Now,
                    TFDateTimeCreateFA = SMSPortalCross.Utils.Date.CompleteGregorianToPersian(DateTime.Now),
                    TFDateTimeRequest = _dtSend,
                    TFDateTimeRequestFA = SMSPortalCross.Utils.Date.CompleteGregorianToPersian(_dtSend), 
                    TFEnable = true
             
                };

                SendBox sendBox = new SendBox()
                {
                    TFId = Guid.NewGuid(),
                    TFMessage_Id = _message.TFId,
                    TFUser_Id = _user.TFId,
                    TFQueue_Id = queue_id,
                    TFDateTimeCreate = DateTime.Now,
                    TFDateTimeCreateFA = SMSPortalCross.Utils.Date.CompleteGregorianToPersian(DateTime.Now),

                };
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {

                    db.Queues.Add(queue);
                    db.SendBoxes.Add(sendBox);
                    db.SaveChanges();

                }
                AddQueuePhone(_user, queue, sendBox, lstPhone, _ScheduleType, _dtSend);


            }
            catch (Exception e)
            {
                Logger.Log(Messages.GSM_HANDLER_QUEUE_MAKER, e);
            }



        }


        public void AddQueuePhone(User _user, Queue _queue, SendBox _sendBox, List<Phone> lstPhone, Enums.Schedule _scheduleType, DateTime _dtSend)
        {
            try
            {

                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    db.Configuration.AutoDetectChangesEnabled = false;

                    string message = (from x in db.Messages where x.TFId == _queue.TFMessage_Id select x.TFContent).FirstOrDefault();
                    foreach (Phone phone in lstPhone)
                    {

                        Guid qp_Id = Guid.NewGuid();
                        Queue_Phone qp = new Queue_Phone()
                        {
                            TFId = qp_Id,
                            TFPhone_Id = phone.TFId,
                            TFQueue_Id = _queue.TFId,
                            TFIsUnderProcess = false,
                            TFActive = Enums.getDefultActiveState((int)_scheduleType),
                            TFEnable = true,
                            TFError = false


                        };
                        db.Queue_Phone.Add(qp);
                       
                    }

                    db.SaveChanges();


                    if (_scheduleType != Enums.Schedule.None)
                    {
                        SMSPortalCross.Utils.Scheduler scheduler = new SMSPortalCross.Utils.Scheduler();
                        scheduler.makeNewSchedule(_queue.TFId, _user.TFId, _sendBox.TFId, Global.File_Scheduler, _scheduleType, _dtSend);

                    }

                }

            }
            catch (Exception e)
            {
                Logger.Log(Messages.GSM_HANDLER_ADD_QUEUE_PHONE, e);
            }
        }


        public void PushBySchedule(User user, Guid _sendBoxId)
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    SendBox sendBox = (from x in db.SendBoxes where x.TFId == _sendBoxId select x).FirstOrDefault();
                    Queue queue = (from x in db.Queues where x.TFId == sendBox.TFQueue_Id select x).FirstOrDefault();

                    var lstQP = (from x in db.Queue_Phone where x.TFQueue_Id == queue.TFId && x.TFActive == false select x).ToList();

                    foreach (var q in lstQP)
                    {
                        q.TFActive = true;

                        int scheduleType = (from x in db.Queues where x.TFId == queue.TFId select x.TFScheduleType).FirstOrDefault();

                        if (Enums.isOnceType(scheduleType))
                        {
                            try
                            {
                                SMSPortalCross.Utils.Scheduler scheduler = new SMSPortalCross.Utils.Scheduler();
                                scheduler.removeSchedule(queue.TFId);
                            }
                            catch (Exception e)
                            {
                                Logger.Log(Messages.GSM_HANDLER_PUSH_BY_SCHEDULE, e);
                            }
                        }

                    }

                    db.SaveChanges();

                }
            }


            catch (Exception e)
            {
                Logger.Log(Messages.GSM_HANDLER_PUSH_BY_SCHEDULE, e);
            }
        }
    }
}