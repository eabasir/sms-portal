using SMSPortalCross;
using SMSPortalDBDataLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Timers;

namespace GSMConnector
{
    class QueueMaker  
    {

        private System.Timers.Timer aTimer;
        private Guid Sim_Id = Guid.Empty;
        private Connector connector;
        List<QueueItem> lstQueueItems = null;
        List<USSD> lstUssdItems = null;



        public QueueMaker(Guid Sim_Id)
        {
            this.Sim_Id = Sim_Id;
            SIM sim = null;
            using (SMSPortalDBEntities db = new SMSPortalDBEntities())
            {

                sim = (from x in db.SIMs where x.TFId == Sim_Id select x).FirstOrDefault();
            }

            Logger.Show(string.Format("Working SIM: {0}", sim.TFNumber), ConsoleColor.Magenta);

            if (sim != null)
            {
                connector = new Connector(Sim_Id, sim.TFPort);


                try
                {

                    aTimer = new System.Timers.Timer(10000);
                    aTimer.Elapsed += OnTimedEvent;
                    aTimer.AutoReset = true;
                    aTimer.Enabled = true;
                    aTimer.Start();


                }
                catch (Exception e)
                {
                    Logger.Log(Messages.CONNECTOR_MAIN, e);
                }

            }
        }

       

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            try
            {
                aTimer.Stop();

                while (!connector.isNetworkStarted)
                    Thread.Sleep(1000);


                Logger.Show("Start Checking of new queue...\r\n---------------------------------------------", ConsoleColor.DarkYellow);
                
                lstQueueItems = getQueueItems();

                lstUssdItems = getUSSDItems();

                if (lstUssdItems.Count > 0)
                {
                    connector.registerUSSDs(lstUssdItems);
                    while (!connector.getIsAllFinished())
                        Thread.Sleep(1000);
                }

                if (lstQueueItems.Count > 0)
                {

                    foreach (var _queueItems in lstQueueItems.GroupBy(x => x.queue_phone.TFQueue_Id))
                    {


                        if (lstUssdItems.Count > 0)
                        {
                            connector.registerUSSDs(lstUssdItems);
                            while (!connector.getIsAllFinished())
                                Thread.Sleep(1000);
                        }


                        Logger.Show(string.Format("{0} items are selected for Queue_Id: {1}", _queueItems.Count().ToString(), _queueItems.Key.ToString()), ConsoleColor.Cyan);
                        Guid queueId = _queueItems.Key;
                        List<QueueItem> queueItems = _queueItems.Select(g => g).ToList();
                        connector.registerQueue(queueItems, queueId);
                        
                        while (!connector.getIsAllFinished())
                            Thread.Sleep(1000);
                
                    }


                }
                aTimer.Start();
                //Console.ForegroundColor = ConsoleColor.Cyan;
                //Console.WriteLine("--------------------------------------------------");
                //Console.WriteLine("Timer has been started again.");


            }
            catch (Exception ex)
            {
                Logger.Log(Messages.CONNECTOR_TIMER_EVENT, ex);
            }



        }

        private List<USSD> getUSSDItems()
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {

                    var q = (from x in db.USSDs where x.TFSim_Id == Sim_Id && !x.TFIsSent select x).ToList();
                    return q;
                  

                }
            }
            catch {

            }
            return null;
        }

        public List<QueueItem> getQueueItems()
        {
            List<QueueItem> queueItems = new List<QueueItem>();
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;
            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSPortalLowLevelConnection"].ConnectionString);

                connection.Open();

                string query = "UPDATE Queue_Phone " +
                                  "SET TFIsUnderProcess = 1 " +
                                  "OUTPUT INSERTED.TFId " +
                                  "WHERE TFId IN " +
                                  "( " +
                                      "SELECT TOP 20 TFId " +
                                      "FROM Queue_Phone WHERE TFQueue_Id in " +
                                        "( " +
                                            "SELECT TFId FROM Queue where Queue.TFUser_Id in" +
                                            "( " +
                                                "select TFUser_Id from SIM_User where TFSIM_Id = @simId" +
                                            ") " +
                                            "and TFEnable = 1" +
                                        ") " +
                                        "and TFIsUnderProcess = 0 and TFEnable = 1 and TFActive = 1 and TFError = 0" +

                                   ")";

                command = new SqlCommand(query, connection);

                command.Parameters.Add(new SqlParameter("simId", Sim_Id));
                
                reader = command.ExecuteReader();

                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {

                    while (reader.Read())
                    {
                        Guid QP_id = Guid.Parse(reader["TFId"].ToString());


                        QueueItem item = new QueueItem()
                        {
                            queue_phone = (from x in db.Queue_Phone where x.TFId == QP_id select x).FirstOrDefault(),
                        };
                        item.sendBox = (from x in db.SendBoxes where x.TFQueue_Id == item.queue_phone.TFQueue_Id select x).FirstOrDefault();
                        queueItems.Add(item);
                    }

                }



            }
            catch (Exception e)
            {
                Logger.Log(Messages.CONNECTOR_GET_QUEUE_ITEM, e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                    reader.Close();
                }
                if (command != null)
                {
                    command.Dispose();
                }
                if (connection != null)
                {
                    connection.Dispose();
                    connection.Close();
                }
            }

            return queueItems;
        }


       


        public void shutDown()
        {
            connector.shutDownModem();
        }

       
    }

}
