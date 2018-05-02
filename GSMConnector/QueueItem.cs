using SMSPortalDBDataLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSMConnector
{
    class QueueItem
    {
        public Queue_Phone queue_phone { get; set; }
        public SendBox sendBox { get; set; }
    }
}
