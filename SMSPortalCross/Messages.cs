using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSPortalCross
{
    public class Messages
    {
        public const int LOGGER_FILE_CREATE = 101;
        
        public const int WEBSERVICE_ADD_TO_QUEUE = 201;
        public const int WEBSERVICE_PROCESS_SCHEDULE_QUEUE = 202;

        public const int GSM_HANDLER_QUEUE_MAKER = 303;
        public const int GSM_HANDLER_ADD_QUEUE_PHONE = 304;
        public const int GSM_HANDLER_PUSH_BY_SCHEDULE = 305;



        public const int HANDLER_RUN_SERVICE = 401;


        public const int CONNECTOR_MAIN = 501;
        
        public const int CONNECTOR_TIMER_EVENT = 502;
        public const int CONNECTOR_GET_QUEUE_ITEM = 503;
        public const int CONNECTOR_SEND = 504;
        public const int CONNECTOR_ON_SEND = 505;

        
        
       
        public const int SCHEDULE_CONSOL = 601;


        

        
    }
}
