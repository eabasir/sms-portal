using SMSPortalModemLibrary.Network;
using SMSPortalModemLibrary.Structure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace UIServer
{


    public partial class Form1 : Form, OnDataRecievedListener
    {


        string log = string.Empty;

        public class GridSendData
        {
            public string number;
            public bool sent = false;
            public int idRecievded = -1;
            public bool delivered = false;
            public DateTime dtSend;
            public DateTime dtDeliver;
        }

        public class GridInboxData
        {
            public string number;
            public DateTime dtRecieved;
            public string Message;
        }

        List<GridSendData> lstGridSendData = new List<GridSendData>();
        List<GridInboxData> lstGridInboxData = new List<GridInboxData>();


        NetworkManager networkManager;

        public Form1()
        {
            InitializeComponent();
            networkManager = NetworkManager.getInstance(this, Convert.ToInt32(txtPort.Text));
            lblStatus.Text = "Waiting for a connection...";
            grdInfo.AutoGenerateColumns = true;

            
            for (int i = 0; i < 1; i++)
            {
                lstGridSendData.Add(new GridSendData()
                {
                    number = "09125975886",
                });

            }
            grdInfo.DataSource = GetResultsTableForSend(lstGridSendData);
            
        }

        public void fillListBySendTable() {

            lstGridSendData.Clear();
            DataTable table = grdInfo.DataSource as DataTable;

            for (int i = 0; i < table.Rows.Count; i++) {
                lstGridSendData.Add(new GridSendData
                {
                    number = table.Rows[i][0].ToString()
                });

            }
            
        }


        public DataTable GetResultsTableForSend(List<GridSendData> lstGridData)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Number".ToString());
            table.Columns.Add("Sent".ToString());
            table.Columns.Add("GSM_ID".ToString());
            table.Columns.Add("Delivered".ToString());
            table.Columns.Add("Send_Time".ToString());
            table.Columns.Add("Delivery_Time".ToString());

            foreach (GridSendData data in lstGridData)
            {
                DataRow dr = table.NewRow();
                dr["Number"] = data.number;
                dr["Sent"] = data.sent ? "Yes" : "No";
                dr["GSM_ID"] = data.idRecievded.ToString();
                dr["Delivered"] = data.delivered ? "Yes" : "No"; ;
                dr["Send_Time"] = data.dtSend.ToString("yyyy-MM-dd-HH:mm:ss");
                dr["Delivery_Time"] = data.dtDeliver.ToString("yyyy-MM-dd-HH:mm:ss");

                table.Rows.Add(dr);
            }
            return table;
        }
        public DataTable GetResultsTableForInbox(List<GridInboxData> lstGridData)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Number".ToString());
            table.Columns.Add("Recieved_Time".ToString());
            table.Columns.Add("Message".ToString());

            foreach (GridInboxData data in lstGridData)
            {
                DataRow dr = table.NewRow();
                dr["Number"] = data.number;
                dr["Recieved_Time"] = data.dtRecieved.ToString("yyyy-MM-dd-HH:mm:ss");
                dr["Message"] = data.Message;

                table.Rows.Add(dr);
            }
            return table;
        }



        #region Events


        private void btnStart_Click(object sender, EventArgs e)
        {
            networkManager.StartListening();
        }
        

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                fillListBySendTable();
                int length = Encoding.BigEndianUnicode.GetBytes(txtMessage.Text).Length;
                if (length > SMSTextPacket.MESSAGE_TEXT_COUNTER)
                {
                    MessageBox.Show("متن پیام نمی تواند بیشتر از 600 کاراکتر باشد");
                    return;
                }

                lblCharCount.Text = "تعداد کاراکتر: "+ length;

                txtFinishIndex.Text = (lstGridSendData.Count - 1).ToString();
                AbstractPacket packetToSend = new SMSTextPacket(Convert.ToInt32(txtSimNumber.Text), txtMessage.Text);
                byte[] byteToSend = packetToSend.toRaw();
                networkManager.Send(byteToSend);
            }
            catch (IndexOutOfRangeException e1) {
                MessageBox.Show("متن پیام نمی تواند بیشتر از 600 کاراکتر باشد");

            }
        }


        private void btnActivate_Click(object sender, EventArgs e)
        {
            AbstractPacket packetToSend = null;
            byte[] byteToSend = null;

            packetToSend = new ActivationPacket(Convert.ToInt32(txtSimNumber.Text), Convert.ToInt32(txtStartIndex.Text), Convert.ToInt32(txtFinishIndex.Text));
            byteToSend = packetToSend.toRaw();
            networkManager.Send(byteToSend);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendPacket sendPacket = new SendPacket(Convert.ToInt32(txtSimNumber.Text), AbstractPacket.WRITE_CMD, 1);
            byte[] byteToSend = sendPacket.toRaw();
            networkManager.Send(byteToSend);
        }

        private void btnReadNumbers_Click(object sender, EventArgs e)
        {
            log = string.Empty;
            AbstractPacket packetToSend = new RegisterationPacket(Convert.ToInt32(txtSimNumber.Text), 0);
            byte[] byteToSend = packetToSend.toRaw();
            networkManager.Send(byteToSend);

        }

        private void btnReadAtivation_Click(object sender, EventArgs e)
        {

            log = string.Empty;
            AbstractPacket packetToSend = new ActivationPacket(Convert.ToInt32(txtSimNumber.Text));
            packetToSend.Cmd = AbstractPacket.READ_CMD;
            byte[] byteToSend = packetToSend.toRaw();
            networkManager.Send(byteToSend);
        }


        private void btnReadCurrentMessage_Click(object sender, EventArgs e)
        {
            AbstractPacket packetToSend = new SMSTextPacket(Convert.ToInt32(txtSimNumber.Text));
            byte[] byteToSend = packetToSend.toRaw();
            networkManager.Send(byteToSend);


        }

        private void btnSendUSSD_Click(object sender, EventArgs e)
        {
            txtUSSDAnswer.Text = string.Empty;
            USSDPacket ussdPacket = new USSDPacket(Convert.ToInt32(txtSimNumber.Text), USSDPacket.USSDType.Write, txtUSSDRequest.Text);
            byte[] byteToSend = ussdPacket.toRaw();
            networkManager.Send(byteToSend);

        }

        private void btnUSSDRead_Click(object sender, EventArgs e)
        {
            txtUSSDAnswer.Text = string.Empty;
            USSDPacket ussdPacket = new USSDPacket(Convert.ToInt32(txtSimNumber.Text), USSDPacket.USSDType.Read, null);
            byte[] byteToSend = ussdPacket.toRaw();
            networkManager.Send(byteToSend);
        }


        private void btnReadInbox_Click(object sender, EventArgs e)
        {
            InboxPacket inboxPacket = new InboxPacket(Convert.ToInt32(txtSimNumber.Text), 0);
            byte[] byteToSend = inboxPacket.toRaw();
            networkManager.Send(byteToSend);
        }

     

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            networkManager.shutDown();
        }





        #region Network Interfaces

        public void onConnectionAcceted()
        {
            lblStatus.Invoke((Action)(() => lblStatus.Text = "Modem Connected!"));
        }

        public void onConnectionStarted()
        {
            lblStatus.Invoke((Action)(() => lblStatus.Text = "Server Started!"));
        }


        /// <summary>
        /// Itterate on gridview numbers based on registeration answer of modem and register new number
        /// After all numbers are registered, activation of numbers will be done
        /// This method can be fired by two ways. Send Button and Read Written Numbers button.
        /// </summary>
        /// <param name="registeredPacket">Registerd packet (answer of modem)</param>
        public void onWriteNumberRegister(RegisterationPacket registeredPacket)
        {
            int cIndex = registeredPacket.Index + 1;
            byte[] byteToSend = null;
            AbstractPacket packetToSend = null;
            if (cIndex < lstGridSendData.Count)
            {
                packetToSend = new RegisterationPacket(Convert.ToInt32(txtSimNumber.Text), lstGridSendData[cIndex].number, cIndex);
                byteToSend = packetToSend.toRaw();
                networkManager.Send(byteToSend);
            }
            else
            {
                MessageBox.Show("All numbers are registered");
            }



        }

        public void onReadNumberRegister(RegisterationPacket registeredPacket)
        {

            int cIndex = registeredPacket.Index + 1;
            byte[] byteToSend = null;
            AbstractPacket packetToSend = null;

            log += registeredPacket.Number + Environment.NewLine;


            if (cIndex < lstGridSendData.Count)
            {
                packetToSend = new RegisterationPacket(Convert.ToInt32(txtSimNumber.Text), cIndex);
                byteToSend = packetToSend.toRaw();
                networkManager.Send(byteToSend);
            }
            else
            {
                MessageBox.Show(log);
            }

        }



        public void onWriteActivate()
        {
            MessageBox.Show("All numbers are activated");
        }


        public void onReadActivate(ActivationPacket packet)
        {
            txtStartIndex.Invoke((Action)(() => txtStartIndex.Text = packet.Start.ToString()));
            txtFinishIndex.Invoke((Action)(() => txtFinishIndex.Text = packet.Finish.ToString()));
            txtWorkingIndex.Invoke((Action)(() => txtWorkingIndex.Text = packet.WorkingIndex.ToString()));

        }



        public void onSend()
        {
            foreach (GridSendData data in lstGridSendData)
            {
                data.sent = true;
            }
            grdInfo.Invoke((Action)(() => grdInfo.DataSource = GetResultsTableForSend(lstGridSendData)));
        }

        public void onAutoId(RegisterationPacket packet)
        {
            foreach (GridSendData data in lstGridSendData)
            {
                if (data.number == packet.Number && data.idRecievded == -1)
                {
                    data.idRecievded = packet.Id;
                    break;
                }

            }

            grdInfo.Invoke((Action)(() => grdInfo.DataSource = GetResultsTableForSend(lstGridSendData)));

        }


        public void onDelivery(DeliveryPacket packet)
        {

        }

        
        public void onAutoDelivery(DeliveryPacket packet)
        {
            int pages = SMSPortalModemLibrary.Utils.PacketUtility.messagePagesCounter(txtMessage.Text);

            foreach (GridSendData data in lstGridSendData)
            {

                bool isRelatedId = SMSPortalModemLibrary.Utils.PacketUtility.isRelatedId(data.idRecievded, packet.Id, pages);
                
                if (packet.Number == data.number && isRelatedId)
                {
                    data.delivered = true;
                    data.dtSend = packet.DtSend;
                    data.dtDeliver = packet.DtDeliver;
                    break;
                }

            }

            grdInfo.Invoke((Action)(() => grdInfo.DataSource = GetResultsTableForSend(lstGridSendData)));
        }

        public void onUSSDWrite()
        {
            USSDPacket ussdPacket = new USSDPacket(Convert.ToInt32(txtSimNumber.Text), USSDPacket.USSDType.Execute, null);
            byte[] byteToSend = ussdPacket.toRaw();
            networkManager.Send(byteToSend);

        }

        public void onUSSDExecute()
        {
            USSDPacket ussdPacket = new USSDPacket(Convert.ToInt32(txtSimNumber.Text), USSDPacket.USSDType.Read, null);
            byte[] byteToSend = ussdPacket.toRaw();
            networkManager.Send(byteToSend);
        }

        public void onUSSDRead(USSDPacket packet)
        {

            txtUSSDAnswer.Invoke((Action)(() => txtUSSDAnswer.Text = packet.RequestAnswer));
        }

        public void onMessageTextWrite()
        {
            AbstractPacket packetToSend = new RegisterationPacket(Convert.ToInt32(txtSimNumber.Text), lstGridSendData[0].number, 0);
            byte[] byteToSend = packetToSend.toRaw();
            networkManager.Send(byteToSend);

        }

        public void onMessageTextRead(SMSTextPacket packet)
        {
            txtMessage.Invoke((Action)(() => txtMessage.Text = packet.Message));
        }

        public void onAutoInbox(InboxPacket packet)
        {
            lstGridInboxData.Add(new GridInboxData
            {
                number = packet.Number,
                Message = packet.Message,
                dtRecieved = packet.DtRecieved
            });
            grdInbox.Invoke((Action)(() => grdInbox.DataSource = GetResultsTableForInbox(lstGridInboxData)));
        }

        public void onReadInbox(InboxPacket packet)
        {

            lstGridInboxData.Add(new GridInboxData
            {
                number = packet.Number,
                Message = packet.Message,
                dtRecieved = packet.DtRecieved
            });

            grdInbox.Invoke((Action)(() => grdInbox.DataSource = GetResultsTableForInbox(lstGridInboxData)));

        }

        public void onGSMSendingError()
        {
            MessageBox.Show("Error Occured... try re-active numbers");

        }





        #endregion

        #endregion


    }


}
