using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMSPortalModemLibrary.Network;
using SMSPortalModemLibrary.Utils;

namespace SMSPortalModemLibrary.Structure
{

    /// <summary>
    /// This class has no usage for Write request.
    /// </summary>
    public class DeliveryPacket : AbstractPacket
    {
        private static int START_YEAR = 2000;

        private static int NUMBER_COUNTER = 4;
        private static int ID_COUNTER = 1;

        private string number;
        private int id;
        private DateTime dtSend;
        private DateTime dtDeliver;
        private int index;

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

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public DateTime DtSend
        {
            get
            {
                return dtSend;
            }

            set
            {
                dtSend = value;
            }
        }

        public DateTime DtDeliver
        {
            get
            {
                return dtDeliver;
            }

            set
            {
                dtDeliver = value;
            }
        }

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



        /// <summary>
        /// This constructor is used to parse raw data as Object.
        /// </summary>
        /// <param name="sim_number"></param>
        public DeliveryPacket(int sim_number)
        {
            this.SimNumber = sim_number;
        }


        /// <summary>
        /// This constructor is used for Read request.
        /// </summary>
        /// <param name="sim_number"></param>
        /// <param name="_index"></param>
        /// <param name="_id"></param>
        public DeliveryPacket(int sim_number, int _index, int _id)
        {
            this.SimNumber = sim_number;
            this.Index = _index;
            this.Id = _id;
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

                    this.Id = data.Skip(cIndex).Take(ID_COUNTER).ToArray()[0];
                    cIndex += ID_COUNTER;


                    int indexBeforDtSend = cIndex;

                    try
                    {
                        byte[] sYear = data.Skip(cIndex).Take(1).ToArray();
                        int sendYear = Convert.ToInt32(BitConverter.ToString(sYear).Replace("-", string.Empty));
                        cIndex++;

                        byte[] sMonth = data.Skip(cIndex).Take(1).ToArray();
                        int sendMonth = Convert.ToInt32(BitConverter.ToString(sMonth).Replace("-", string.Empty));
                        cIndex++;

                        byte[] sDay = data.Skip(cIndex).Take(1).ToArray();
                        int sendDay = Convert.ToInt32(BitConverter.ToString(sDay).Replace("-", string.Empty));
                        cIndex++;

                        byte[] sHour = data.Skip(cIndex).Take(1).ToArray();
                        int sendHour = Convert.ToInt32(BitConverter.ToString(sHour).Replace("-", string.Empty));
                        cIndex++;

                        byte[] sMinute = data.Skip(cIndex).Take(1).ToArray();
                        int sendMinute = Convert.ToInt32(BitConverter.ToString(sMinute).Replace("-", string.Empty));
                        cIndex++;

                        byte[] sSecond = data.Skip(cIndex).Take(1).ToArray();
                        int sendSecond = Convert.ToInt32(BitConverter.ToString(sSecond).Replace("-", string.Empty));
                        cIndex++;

                        this.DtSend = new DateTime(START_YEAR + sendYear, sendMonth, sendDay, sendHour, sendMinute, sendSecond);
                    }
                    catch {
                        this.dtSend = DateTime.Now;
                        cIndex = indexBeforDtSend + 6;
                    }

                    int indexBeforDtDeliver = cIndex;
                    try
                    {
                        byte[] dYear = data.Skip(cIndex).Take(1).ToArray();
                        int deliveryYear = Convert.ToInt32(BitConverter.ToString(dYear).Replace("-", string.Empty));
                        cIndex++;

                        byte[] dMonth = data.Skip(cIndex).Take(1).ToArray();
                        int deliveryMonth = Convert.ToInt32(BitConverter.ToString(dMonth).Replace("-", string.Empty));
                        cIndex++;

                        byte[] dDay = data.Skip(cIndex).Take(1).ToArray();
                        int deliveryDay = Convert.ToInt32(BitConverter.ToString(dDay).Replace("-", string.Empty));
                        cIndex++;

                        byte[] dHour = data.Skip(cIndex).Take(1).ToArray();
                        int deliveryHour = Convert.ToInt32(BitConverter.ToString(dHour).Replace("-", string.Empty));
                        cIndex++;

                        byte[] dMinute = data.Skip(cIndex).Take(1).ToArray();
                        int deliveryMinute = Convert.ToInt32(BitConverter.ToString(dMinute).Replace("-", string.Empty));
                        cIndex++;

                        byte[] dSecond = data.Skip(cIndex).Take(1).ToArray();
                        int deliverySecond = Convert.ToInt32(BitConverter.ToString(dSecond).Replace("-", string.Empty));
                        cIndex++;

                        this.DtDeliver = new DateTime(START_YEAR + deliveryYear, deliveryMonth, deliveryDay, deliveryHour, deliveryMinute, deliverySecond);
                    }
                    catch {
                        this.DtDeliver = DateTime.Now;
                        cIndex = indexBeforDtDeliver + 6;
                    }
                }

                return this;

            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Delivery packet error => {0}", e.Message));
            }
            return null;
        }


        public override byte[] toRaw()
        {

            packet = new byte[SRC_ADD_COUNTER + DEST_ADD_COUNTER + CMD_COUNTER + ADD_VAR_COUNTER + ID_COUNTER];

            cIndex = SRC_ADD_COUNTER + DEST_ADD_COUNTER;

            packet[cIndex] = AbstractPacket.READ_CMD;
            cIndex += CMD_COUNTER;

            add_var = PacketUtility.IntToTwoBytes(getDeliveryAVBound(this.SimNumber)[0] + this.Index);
            Array.Copy(add_var, 0, packet, cIndex, ADD_VAR_COUNTER);
            cIndex += ADD_VAR_COUNTER;

            packet[cIndex] = (byte)this.Id;

            addCRC();


            return packet;


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


                return String.Format("Delivery => SIM: {0}, Command: {1}, " +
                                     "Id: {2}, Number: {3}, Send Time: {4}, Delivery Time: {5}",
                                      this.SimNumber, command,
                                      this.Id, this.Number, this.DtSend, this.DtDeliver);

            }
            catch
            {
                return "Write Deliver Packet Error";
            }
        }


    }

}
