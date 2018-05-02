using SMSPortalCross;
using SMSPortalDBDataLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SMSPortalService
{
    public class UserService : IUserService
    {
        public void AddToQueue(string userName, string password, string content, List<string> numbers, Enums.Schedule ScheduleType, DateTime dtSend)
        {
            try
            {
                User user = null;
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {

                    user = (from x in db.Users where x.TFUserName == userName && x.TFPassword == password select x).FirstOrDefault();

                }
                if (user != null)
                { 
                    Task.Run(() =>
                    {

                    
                            numbers = numbers.Distinct().ToList();
                            if (numbers != null)
                            {
                                List<Phone> lstPhone = NumberHandler(numbers);
                                Message message = MessageHandler(content, ScheduleType, dtSend);

                                GSMHandler handler = new GSMHandler();

                                handler.QueueMaker(user, message, lstPhone, ScheduleType, dtSend);

                            
                        }
                    });

                }
                else
                {

                }

            }
            catch (Exception e)
            {
                Logger.Log(Messages.WEBSERVICE_ADD_TO_QUEUE, e);
            }





        }

        public void AddRangeToQueue(string userName, string password, string content, long start, long finish, Enums.Schedule ScheduleType, DateTime dtSend) {
            
            try
            {
                User user = null;
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {

                    user = (from x in db.Users where x.TFUserName == userName && x.TFPassword == password select x).FirstOrDefault();

                }
                if (user != null)
                {
                    Task.Run(() =>
                    {
                        
                        if (finish - start > 0)
                        {
                            Message message = MessageHandler(content, ScheduleType, dtSend);

                            if (finish - start <= SMSPortalCross.Constants.MAX_NUMBER_TO_INSERT)
                            {
                                List<string> temp = new List<string>();

                                for (long i = start; i <= finish; i++)
                                {
                                    temp.Add("0" + i.ToString());
                                }
                                List<Phone> lstPhone = NumberHandler(temp);
                                GSMHandler handler = new GSMHandler();
                                handler.QueueMaker(user, message, lstPhone, ScheduleType, dtSend);


                            }
                            else
                            {
                                int quotient = Convert.ToInt32((finish - start) / SMSPortalCross.Constants.MAX_NUMBER_TO_INSERT);

                                for (int i = 0; i <= quotient; i++)
                                {
                                    List<string> temp = new List<string>();

                                    long newStart = start + (i * SMSPortalCross.Constants.MAX_NUMBER_TO_INSERT);

                                    long newFinish = newStart + SMSPortalCross.Constants.MAX_NUMBER_TO_INSERT;
                                    if (newFinish > finish)
                                        newFinish = finish;

                                    for (long j = newStart; j < newFinish; j++)
                                    {
                                        temp.Add("0" + j.ToString());
                                    }
                                    List<Phone> lstPhone = NumberHandler(temp);
                                    GSMHandler handler = new GSMHandler();
                                    handler.QueueMaker(user, message, lstPhone, ScheduleType, dtSend);

                                }
                            }
                        }
 
                         
                    });

                }
                else
                {

                }

            }
            catch (Exception e)
            {
                Logger.Log(Messages.WEBSERVICE_ADD_TO_QUEUE, e);
            }

        }

        public void ProccessScheduleQueue(Guid userId, Guid sendBoxId)
        {

            try
            {
                User user = null;
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    user = (from x in db.Users where x.TFId == userId select x).FirstOrDefault();
                }
                if (user != null)
                {
                    Task.Run(() =>
                    {
                        lock (Global.Task_Sync_Obj)
                        {
                            GSMHandler handler = new GSMHandler();
                            handler.PushBySchedule(user, sendBoxId);
                        }
                    });
                }
                else
                {
                }

            }
            catch (Exception e)
            {
                Logger.Log(Messages.WEBSERVICE_ADD_TO_QUEUE, e);
            }
        }

        Message MessageHandler(string content, Enums.Schedule ScheduleType, DateTime dtSend)
        {
            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
            {
                
                Message message = null;
                if (ScheduleType == Enums.Schedule.None)
                {
                    message = (from x in db.Messages
                               where x.TFContent == content
                               select x).FirstOrDefault();
                }
                else
                {
                    message = (from x in db.Messages
                               where x.TFContent == content
                               && x.TFDateTimeCreate == dtSend
                               select x).FirstOrDefault();
                }

                if (message != null)
                    return message;
                else
                {
                    message = new Message()
                    {
                        TFId = Guid.NewGuid(),
                        TFContent = content,
                        TFDateTimeCreate = DateTime.Now
                    };
                    db.Messages.Add(message);
                    db.SaveChanges();
                    return message;
                }


            }





        }

        List<Phone> NumberHandler(List<string> numbers)
        {
            List<Phone> lstPhone = null;

            numbers = numbers.Where(x => x != string.Empty).ToList();
            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
            {
                db.Configuration.AutoDetectChangesEnabled = false;
                List<string> newNumbers = numbers.Where(x => !db.Phones
                                                .Select(y => y.TFNumber)
                                                .Contains(x)
                                                ).ToList();

                if (newNumbers != null && newNumbers.Count > 0)
                {
                    foreach (string number in newNumbers)
                    {
                        db.Phones.Add(new Phone()
                        {
                            TFId = Guid.NewGuid(),
                            TFNumber = number
                        });
                    }
                    db.SaveChanges();
                }
                lstPhone = (from x in db.Phones select x).Where(x => numbers.Contains(x.TFNumber)).ToList();


            }
            return lstPhone;
        }
    }
}
