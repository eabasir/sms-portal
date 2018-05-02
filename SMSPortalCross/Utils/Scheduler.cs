using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.TaskScheduler;


namespace SMSPortalCross.Utils
{
    public class Scheduler
    {
        public void makeNewSchedule(Guid queueId, Guid userId, Guid sendboxId, string path, SMSPortalCross.Enums.Schedule _scheduleType, DateTime _dtSend)
        {
            using (TaskService ts = new TaskService())
            {

                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "Queue:" + queueId.ToString();

                if (_scheduleType == SMSPortalCross.Enums.Schedule.Once)
                {
                    td.Triggers.Add(new TimeTrigger() { StartBoundary = _dtSend });
                }
                else if (_scheduleType == SMSPortalCross.Enums.Schedule.Daily || _scheduleType == SMSPortalCross.Enums.Schedule.NowANDDaily)
                {
                    DailyTrigger daily = new DailyTrigger() { StartBoundary = _dtSend };
                    daily.DaysInterval = 1;
                    td.Triggers.Add(daily);
                }
                else if (_scheduleType == SMSPortalCross.Enums.Schedule.WeekLy || _scheduleType == SMSPortalCross.Enums.Schedule.NowANDWeekly)
                {
                    WeeklyTrigger weekly = new WeeklyTrigger() { StartBoundary = _dtSend };
                    weekly.WeeksInterval = 1;
                    td.Triggers.Add(weekly);
                }
                else if (_scheduleType == SMSPortalCross.Enums.Schedule.Monthly || _scheduleType == SMSPortalCross.Enums.Schedule.NowANDMonthly)
                {
                    MonthlyTrigger monthly = new MonthlyTrigger() { StartBoundary = _dtSend };
                    td.Triggers.Add(monthly);
                }


                td.Actions.Add(new ExecAction(path, "-" + userId.ToString() + " -" + queueId.ToString() + " -" + sendboxId.ToString(), null));

                TaskFolder tf = ts.GetFolder("SMSPortal");

                tf.RegisterTaskDefinition("Queue_" + queueId.ToString(), td);
            }
        }

        public void removeSchedule(Guid queueId)
        {
            try
            {
                using (TaskService ts = new TaskService())
                {
                    TaskFolder tf = ts.GetFolder("SMSPortal");
                    tf.DeleteTask("Queue_" + queueId.ToString());
                }
            }
            catch { }
        }
    }
}