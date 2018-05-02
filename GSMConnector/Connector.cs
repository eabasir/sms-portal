using SMSPortalModemLibrary.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMSPortalModemLibrary.Structure;
using SMSPortalDBDataLibrary;
using SMSPortalCross;
using System.Data.Entity;
using System.Timers;
using System.Threading;

namespace GSMConnector
{
    class Connector : OnDataRecievedListener
    {
        private NetworkManager networkManager = null;
        private Guid simId;
        private int sim_number;
        private List<QueueItem> queueItems;
        private List<USSD> ussdItems;
        private Guid queueId;
        private string message;
        private int start;
        private int finish;

        public bool isNetworkStarted = false;
        private bool isQueueFinished = true;
        private bool isUSSDFinished = true;

        public Connector(Guid simId, int port)
        {

            this.simId = simId;
            this.sim_number = 1;
            networkManager = NetworkManager.getInstance(this, port);

            try
            {
                networkManager.StartListening();
            }
            catch (Exception e)
            {
                Logger.ShowError(e.Message);
            }

        }

        public void registerUSSDs(List<USSD> lstUssdItems)
        {
            resetTimer();
            this.isUSSDFinished = false;
            this.ussdItems = lstUssdItems;
            Console.ResetColor();

            sendUSSD();


        }


        public void registerQueue(List<QueueItem> queueItems, Guid queueId)
        {
            resetTimer();
            this.isQueueFinished = false;
            Console.ResetColor();

            try
            {
                this.queueId = queueId;

                this.queueItems = queueItems;

                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    try
                    {
                        message = (from x in db.Queues where x.TFId == queueId select x.Message.TFContent).FirstOrDefault();

                        if (Encoding.BigEndianUnicode.GetBytes(message).Length > SMSTextPacket.MESSAGE_TEXT_COUNTER)
                        {
                            Logger.ShowError(string.Format("Message could not be more than {0} bytes", SMSTextPacket.MESSAGE_TEXT_COUNTER));
                            return;
                        }

                        start = 0;
                        finish = queueItems.Count() - 1;
                        AbstractPacket packetToSend = new SMSTextPacket(sim_number, message);
                        byte[] byteToSend = packetToSend.toRaw();

                        networkManager.Send(byteToSend);
                    }
                    catch (IndexOutOfRangeException e1)
                    {
                        Logger.ShowError(string.Format("Message could not be more than {0} bytes", SMSTextPacket.MESSAGE_TEXT_COUNTER));

                    }

                }
            }
            catch (Exception e)
            {
                Logger.ShowError(e.Message);
            }

        }


        private void sendUSSD() {
            try
            {
                if (ussdItems.Count > 0)
                {
                    string message = ussdItems[0].TFSend;
                    if (!string.IsNullOrEmpty(message) || message.Length > 50)
                    {
                        USSDPacket ussdPacket = new USSDPacket(sim_number, USSDPacket.USSDType.Write, message);
                        byte[] byteToSend = ussdPacket.toRaw();
                        networkManager.Send(byteToSend);
                    }
                    else
                    {
                        Logger.ShowError("Not valid message to send USSD...");
                    }
                }
            }
            catch (Exception e)
            {
                Logger.ShowError(e.Message);
            }
        }


        public void shutDownModem()
        {
            this.isNetworkStarted = false;
            networkManager.shutDown();

        }


        #region listeners



        public void onConnectionAcceted()
        {
            Logger.Show("Modem Connected!", ConsoleColor.Green);
            isNetworkStarted = networkManager.isConnected();

        }

        public void onConnectionStarted()
        {
            Logger.Show("Server Started!", ConsoleColor.Green);
            isNetworkStarted = networkManager.isConnected();
        }


        public void onMessageTextWrite()
        {

            try
            {
                resetTimer();

                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    Guid qp_id = queueItems[0].queue_phone.TFId;

                    string number = (from x in db.Queue_Phone
                                     where x.TFId == qp_id
                                     select x.Phone.TFNumber).FirstOrDefault();

                    AbstractPacket packetToSend = new RegisterationPacket(sim_number, number, 0);
                    Logger.Show(packetToSend.ToString(), ConsoleColor.White);
                    byte[] byteToSend = packetToSend.toRaw();
                    networkManager.Send(byteToSend);
                }
            }
            catch (Exception e)
            {
                Logger.ShowError(e.Message);

            }

        }

        public void onMessageTextRead(SMSTextPacket packet)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Itterate on gridview numbers based on registeration answer of modem and register new number
        /// After all numbers are registered, activation of numbers will be done
        /// </summary>
        /// <param name="registeredPacket">Registerd packet (answer of modem)</param>
        public void onWriteNumberRegister(RegisterationPacket registeredPacket)
        {
            try
            {
                resetTimer();

                int cIndex = registeredPacket.Index + 1;
                byte[] byteToSend = null;
                AbstractPacket packetToSend = null;
                if (cIndex < queueItems.Count)
                {
                    using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                    {
                        Guid qp_id = queueItems[cIndex].queue_phone.TFId;

                        string number = (from x in db.Queue_Phone
                                         where x.TFId == qp_id
                                         select x.Phone.TFNumber).FirstOrDefault();
                        packetToSend = new RegisterationPacket(sim_number, number, cIndex);
                        Logger.Show(packetToSend.ToString(), ConsoleColor.White);
                        byteToSend = packetToSend.toRaw();
                        networkManager.Send(byteToSend);
                    }

                }
                else
                {
                    Logger.Show("All numbers are registered", ConsoleColor.White);

                    packetToSend = new ActivationPacket(sim_number, start, finish);
                    byteToSend = packetToSend.toRaw();
                    networkManager.Send(byteToSend);
                }

            }

            catch (Exception e)
            {
                Logger.ShowError(e.Message);

            }

        }

        public void onReadNumberRegister(RegisterationPacket packet)
        {
        }

        public void onWriteActivate()
        {
            try
            {
                resetTimer();
                Logger.Show("All numbers are activated", ConsoleColor.White);
                Thread.Sleep(1000);
                SendPacket sendPacket = new SendPacket(sim_number, AbstractPacket.WRITE_CMD, 1);
                byte[] byteToSend = sendPacket.toRaw();
                networkManager.Send(byteToSend);
            }
            catch (Exception e)
            {
                Logger.ShowError(e.Message);
            }


        }


        /// <summary>
        /// This listener is called when GSM error occures and activation status is read.
        /// for reactive numbers after error, the start index must be set as equal as last working index +1 (to skipp error number)
        /// </summary>
        /// <param name="packet"></param>
        public void onReadActivate(ActivationPacket packet)
        {
            try
            {
                resetTimer();


                if (packet.WorkingIndex + 1 <= packet.Finish)
                {
                    Logger.Show(string.Format("Re-Active numbers => Last index: {0} , Current Index: {1}", packet.WorkingIndex, packet.WorkingIndex + 1), ConsoleColor.Green);


                    byte[] byteToSend = null;
                    AbstractPacket packetToSend = null;

                    packetToSend = new ActivationPacket(sim_number, packet.WorkingIndex + 1, finish);
                    byteToSend = packetToSend.toRaw();
                    networkManager.Send(byteToSend);
                }
                else
                    finishWorks();

            }
            catch (Exception e)
            {
                Logger.ShowError(e.Message);
            }
        }

        public void onSend()
        {
            resetTimer();

            Logger.Show("Start sending messages", ConsoleColor.White);

        }

        public void onAutoId(RegisterationPacket packet)
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    var qp = (from x in db.Queue_Phone
                              where x.Phone.TFNumber == packet.Number && x.TFQueue_Id == queueId
                              select x).FirstOrDefault();
                    // if queue or queue phone is deleted by user during send process 'q' can be null.
                    // So, it must be checked wheter the value of q is null or not.
                    if (qp != null)
                    {

                        var sendBox = (from x in db.SendBoxes where x.TFQueue_Id == queueId select x).FirstOrDefault();


                        db.SendBox_Phone.Add(new SendBox_Phone
                        {
                            TFId = Guid.NewGuid(),
                            TFPhone_Id = qp.TFPhone_Id,
                            TFSendBox_Id = sendBox.TFId,
                            TFSim_Id = simId,
                            TFGSM_Id = packet.Id,
                            TFDateTimeSend = DateTime.Now,
                            TFDateTimeSendFA = SMSPortalCross.Utils.Date.CompleteGregorianToPersian(DateTime.Now),
                            TFIsDelivered = false

                        });

                        db.SaveChanges();

                        int scheduleType = (from x in db.Queues where x.TFId == queueId select x.TFScheduleType).FirstOrDefault();

                        if (Enums.isOnceType(scheduleType))
                        {
                            db.Queue_Phone.Remove(qp);

                            Logger.Show(string.Format("Queue_Phone with QP_Id: {0} has been removed", qp.TFId), ConsoleColor.Yellow);

                        }
                        else
                        {
                            qp.TFActive = false;
                            db.Entry(qp).State = EntityState.Modified;
                            Logger.Show(string.Format("Queue_Phone with QP_Id: {0} has been deactived", qp.TFId), ConsoleColor.Yellow);

                        }


                        var _qp = queueItems.FirstOrDefault(x => x.queue_phone.TFId == qp.TFId);

                        if (_qp != null)
                            queueItems.Remove(_qp);
                        else
                        {
                            Logger.ShowError(string.Format("Invalid Queue_Phone Id: {0}", qp.TFId));
                            Logger.Log(Messages.CONNECTOR_ON_SEND, new Exception("Invalid Queue_Phone Id"));

                        }

                    }
                    db.SaveChanges();

                    if (queueItems.Count != 0)
                        resetTimer();
                    else
                        finishWorks();

                }
            }
            catch (Exception e)
            {
                Logger.Log(Messages.CONNECTOR_ON_SEND, e);
                resetTimer();
            }

        }



        public void onAutoDelivery(DeliveryPacket packet)
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {

                    var sp = (from x in db.SendBox_Phone
                              where x.Phone.TFNumber == packet.Number && !x.TFIsDelivered && x.TFGSM_Id == packet.Id
                              select x).FirstOrDefault();

                    //if (sp == null)
                    //    sp = (from x in db.SendBox_Phone
                    //          where x.TFGSM_Id == packet.Id || x.TFGSM_Id == packet.Id - 1
                    //          select x).FirstOrDefault();

                    if (sp != null)
                    {
                        if (!sp.TFIsDelivered)
                        {
                            sp.TFIsDelivered = true;
                            sp.TFDateTimeSendGSM = packet.DtSend;
                            sp.TFDateTimeSendGSMFA = SMSPortalCross.Utils.Date.CompleteGregorianToPersian(packet.DtSend);
                            sp.TFDateTimeDelivery = packet.DtDeliver;
                            sp.TFDateTimeDeliveryFA = SMSPortalCross.Utils.Date.CompleteGregorianToPersian(packet.DtDeliver);
                            db.Entry(sp).State = EntityState.Modified;

                            db.SaveChanges();

                            Logger.Show(string.Format("Sendbox_Phone with SP_Id: {0} is delivered", sp.TFId), ConsoleColor.Yellow);
                        }
                        else
                            Logger.Show(string.Format("SP_Id: {0} is already delivered", sp.TFId), ConsoleColor.Yellow);

                    }


                }


            }
            catch (Exception e)
            {
                Logger.ShowError(e.Message);
            }
        }

        public void onAutoInbox(InboxPacket packet)
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities()) {

                    string dtFa = "";
                    try {
                        dtFa = SMSPortalCross.Utils.Date.CompleteGregorianToPersian(packet.DtRecieved);
                    }
                    catch { }

                    db.Inboxes.Add(new Inbox()
                    {
                       TFId = Guid.NewGuid(),
                       TFSim_Id = this.simId,
                       TFMessage = packet.Message,
                       TFSenderNumber = packet.Number,
                       TFDateTime = packet.DtRecieved,
                       TFDateTimeFA = dtFa,
                       TFIsRead = false

                    });

                    db.SaveChanges();
                }
            }
            catch {
            }
        }

        public void onReadInbox(InboxPacket packet)
        {

            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {

                    string dtFa = "";
                    try
                    {
                        dtFa = SMSPortalCross.Utils.Date.CompleteGregorianToPersian(packet.DtRecieved);
                    }
                    catch { }

                    db.Inboxes.Add(new Inbox()
                    {
                        TFId = Guid.NewGuid(),
                        TFSim_Id = this.simId,
                        TFMessage = packet.Message,
                        TFSenderNumber = packet.Number,
                        TFDateTime = packet.DtRecieved,
                        TFDateTimeFA = dtFa,
                        TFIsRead = false

                    });

                    db.SaveChanges();
                }
            }
            catch
            {
            }
        }


        public void onDelivery(DeliveryPacket packet)
        {

        }

        public void onGSMSendingError()
        {
            try
            {
                resetTimer();

                Logger.ShowError("Error Occured... try re-active numbers by skipping current number");

                try
                {
                    using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                    {

                    }
                }
                catch { }

                Thread.Sleep(5000);
                AbstractPacket packetToSend = new ActivationPacket(sim_number);
                packetToSend.Cmd = AbstractPacket.READ_CMD;
                byte[] byteToSend = packetToSend.toRaw();
                networkManager.Send(byteToSend);
            }
            catch (Exception e)
            {
                Logger.ShowError(e.Message);
            }
        }

        public void onUSSDWrite()
        {
            resetTimer();

            USSDPacket ussdPacket = new USSDPacket(sim_number, USSDPacket.USSDType.Execute, null);
            byte[] byteToSend = ussdPacket.toRaw();
            networkManager.Send(byteToSend);
        }

        public void onUSSDExecute()
        {
            resetTimer();
            USSDPacket ussdPacket = new USSDPacket(sim_number, USSDPacket.USSDType.Read, null);
            byte[] byteToSend = ussdPacket.toRaw();
            networkManager.Send(byteToSend);
        }

        public void onUSSDRead(USSDPacket packet)
        {
            try
            {
                resetTimer();

                using (SMSPortalDBEntities db = new SMSPortalDBEntities()) {

                    Guid id = ussdItems[0].TFId;

                    var ussd = (from x in db.USSDs where x.TFId == id select x).FirstOrDefault();

                    ussd.TFIsSent = true;
                    ussd.TFReceive = packet.RequestAnswer;
                    db.Entry(ussd).State = EntityState.Modified;

                    db.SaveChanges();

                }

                try
                {
                    ussdItems.RemoveAt(0);
                }
                catch (Exception e) {
                    Logger.ShowError(e.Message);
                }


                if (ussdItems.Count > 0)
                   sendUSSD();
                else
                    finishWorks();


            }
            catch (Exception e)
            {
                Logger.ShowError(e.Message);
            }

        }



        #endregion
        private System.Timers.Timer checkFinishTimer;


        private void resetTimer()
        {
            if (checkFinishTimer != null)
            {
                checkFinishTimer.Stop();
                checkFinishTimer.Close();
            }
            checkFinishTimer = new System.Timers.Timer(60000);

            checkFinishTimer.Elapsed += OnFinishedEvent;
            checkFinishTimer.AutoReset = true;
            checkFinishTimer.Enabled = true;
            checkFinishTimer.Start();
        }


        private void OnFinishedEvent(Object source, ElapsedEventArgs e)
        {
            try
            {

                Logger.Show("item is finished by ellapse of ensuring time", ConsoleColor.DarkCyan);
                finishWorks();

            }
            catch (Exception ex)
            {
                Logger.Log(Messages.CONNECTOR_TIMER_EVENT, ex);
            }

        }

        public void finishWorks()
        {
            if (checkFinishTimer != null)
                checkFinishTimer.Stop();



            if (queueItems != null && queueItems.Count > 0)
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    foreach (QueueItem item in queueItems)
                    {

                        var qp = (from x in db.Queue_Phone where x.TFId == item.queue_phone.TFId select x).FirstOrDefault();
                        qp.TFIsUnderProcess = false;
                        db.Entry(qp).State = EntityState.Modified;

                    }

                    db.SaveChanges();
                }
            }

            this.isQueueFinished = true;
            this.isUSSDFinished = true;


        }

        public bool getIsAllFinished()
        {

            return this.isUSSDFinished && this.isQueueFinished;
        }
    }
}
