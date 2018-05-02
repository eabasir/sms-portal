using SMSPortalCross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SMSPortalScheduleProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new Logger();

                Guid userId = Guid.Parse(args[0].Replace("-", ""));
                Guid sendBoxId = Guid.Parse(args[2].Replace("-", ""));


                SMSPortalService.UserServiceClient client = new SMSPortalService.UserServiceClient();
                client.ProccessScheduleQueue(userId, sendBoxId);
            }
            catch (Exception e)
            {
                Logger.Log(Messages.SCHEDULE_CONSOL, e);
            }

           


        }

       
    }
}
