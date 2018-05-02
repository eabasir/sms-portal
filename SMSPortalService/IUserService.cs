using SMSPortalCross;
using SMSPortalDBDataLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SMSPortalService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserService" in both code and config file together.
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        void AddToQueue(string userName, string password, string content, List<string> numbers, Enums.Schedule ScheduleType, DateTime dtSend);

        [OperationContract]
        void AddRangeToQueue(string userName, string password, string content, long start, long finish, Enums.Schedule ScheduleType, DateTime dtSend);

        [OperationContract]
        void ProccessScheduleQueue(Guid userId, Guid sendBoxId);
        

    }
}
