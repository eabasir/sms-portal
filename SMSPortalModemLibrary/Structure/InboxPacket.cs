using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMSPortalModemLibrary.Network;
using SMSPortalModemLibrary.Utils;

namespace SMSPortalModemLibrary.Structure
{
    public class InboxPacket : AbstractPacket
    {
        private static int START_YEAR = 2000;
        private static int NUMBER_COUNTER = 4;
        private static int MESSAGE_TEXT_COUNTER = 300;


        private int index;
        private string number;
        private DateTime dtRecieved;
        private string message;

        public int Index
        {
            get
            {
                return index;
            }

            set
            {
                index = value;
            }
        }

        public string Number
        {
            get
            {
                return number;
            }

            set
            {
                number = value;
            }
        }

        public DateTime DtRecieved
        {
            get
            {
                return dtRecieved;
            }

            set
            {
                dtRecieved = value;
            }
        }

        public string Message
        {
            get
            {
                return message;
            }

            set
            {
                message = value;
            }
        }



        // <summary>
        ///  This constructor is used when packet is made for Read request.
        /// </summary>
        /// <param name="sim_number"></param>
        public InboxPacket(int sim_number, int _index)
        {
            this.SimNumber = sim_number;
            this.Cmd = AbstractPacket.READ_CMD;
            this.Index = _index;
        }

        /// <summary>
        ///  This constructor is used when packet need to be parsed as object
        /// </summary>
        /// <param name="sim_number"></param>
        public InboxPacket(int sim_number)
        {
            this.SimNumber = sim_number;
        }


        public override AbstractPacket toObject(byte[] data)
        {
            try
            {
                base.toObject(data);


                if (Cmd == READ_CMD || Cmd == AUTO_CMD)
                {
                    this.Number = "0" + (_base + BitConverter.ToInt32(data.Skip(cIndex).Take(NUMBER_COUNTER).Reverse().ToArray(), 0)).ToString();
                    cIndex += NUMBER_COUNTER;


                    byte[] sYear = data.Skip(cIndex).Take(1).ToArray();
                    int sendYear = Convert.ToInt32(sYear[0]);
                    cIndex++;

                    byte[] sMonth = data.Skip(cIndex).Take(1).ToArray();
                    int sendMonth = Convert.ToInt32(sMonth[0]);
                    cIndex++;

                    byte[] sDay = data.Skip(cIndex).Take(1).ToArray();
                    int sendDay = Convert.ToInt32(sDay[0]);
                    cIndex++;

                    byte[] sHour = data.Skip(cIndex).Take(1).ToArray();
                    int sendHour = Convert.ToInt32(sHour[0]);
                    cIndex++;

                    byte[] sMinute = data.Skip(cIndex).Take(1).ToArray();
                    int sendMinute = Convert.ToInt32(sMinute[0]);
                    cIndex++;

                    byte[] sSecond = data.Skip(cIndex).Take(1).ToArray();
                    int sendSecond = Convert.ToInt32(sSecond[0]);
                    cIndex++;

                    this.DtRecieved = new DateTime(START_YEAR + sendYear, sendMonth, sendDay, sendHour, sendMinute, sendSecond);



                    byte[] temp = data.Skip(cIndex).Take(MESSAGE_TEXT_COUNTER).ToArray();

                    this.message = convertBufferToText(temp);
                    
                    cIndex += MESSAGE_TEXT_COUNTER;

                }

                return this;

            }
            catch (Exception e)
            {

            }
            return null;
        }


        public override byte[] toRaw()
        {

            packet = new byte[SRC_ADD_COUNTER + DEST_ADD_COUNTER + CMD_COUNTER + ADD_VAR_COUNTER];

            cIndex = SRC_ADD_COUNTER + DEST_ADD_COUNTER;

            packet[cIndex] = AbstractPacket.READ_CMD;
            cIndex += CMD_COUNTER;

            add_var = PacketUtility.IntToTwoBytes(getInboxAVBound(this.SimNumber)[0] + this.Index);
            Array.Copy(add_var, 0, packet, cIndex, ADD_VAR_COUNTER);
            cIndex += ADD_VAR_COUNTER;


            addCRC();


            return packet;


        }


        private string convertBufferToText(byte[] temp)
        {
            try
            {
                int length = 0;
                for (int i = 0; i < temp.Length - 1; i++)
                {
                    if (temp[i] == 0x00 && temp[i + 1] == 0x00)
                    {
                        length = i;
                        break;
                    }
                }

                temp = temp.Take(length).ToArray();


                bool modifyNeeded = false;
                bool isASSCI = false;

                if (temp[0] != 0x00 || temp[0] != 0x06)
                {
                    if (temp[1] == 0x06)
                    {
                        modifyNeeded = true;
                    }
                    else if (temp[1] == 0x00)
                    {
                        // is a single asscii byte
                        if (temp[2] == 0x00)
                        {
                            isASSCI = true;
                        }
                        else
                        {
                            modifyNeeded = true;
                        }

                    }
                    else {
                        if (!temp.Any(x => x == 0x00 || x == 0x06))
                            isASSCI = true;
                    }
                    
                }
                else
                {
                    isASSCI = true;
                }


                if (isASSCI)
                    return Encoding.ASCII.GetString(temp).Trim();
                else
                {
                    if (modifyNeeded)
                        temp = temp.Skip(1).ToArray();


                    List<byte> lstBytes = temp.ToList();

                    if (lstBytes.Count % 2 != 0)
                        lstBytes.Add(0x00);


                    for (int i = 0; i < lstBytes.Count; i += 2)
                    {
                        if (i < lstBytes.Count)
                        {
                            if (lstBytes[i] != 0x00 && lstBytes[i] != 0x06)
                            {
                                try
                                {
                                    lstBytes.RemoveAt(i);
                                    lstBytes.RemoveAt(i);
                                }
                                catch { }
                              
                            }
                        }
                    }

                    temp = lstBytes.ToArray();

                
                return Encoding.BigEndianUnicode.GetString(temp).Trim();

            }

            }
            catch
            {

            }
            return null;
        }


    public override string ToString()
    {
        try
        {
            string command = string.Empty;
            if (this.Cmd == AUTO_CMD)
                command = "Auto";
            else if (this.Cmd == WRITE_CMD)
                command = "Write";
            else if (this.Cmd == READ_CMD)
                command = "Read";


            return String.Format("Inbox => SIM: {0}, Command: {1}, " +
                                 "Index: {2}, Number: {3}, Message: {4}, Received Time: {5}",
                                  this.SimNumber, command,
                                  this.Index, this.Number, this.Message, this.DtRecieved);

        }
        catch
        {
            return "Write Inbox Packet Error";
        }
    }


}

}
