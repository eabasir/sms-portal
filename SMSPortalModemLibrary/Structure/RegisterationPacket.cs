using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMSPortalModemLibrary.Network;
using SMSPortalModemLibrary.Utils;

namespace SMSPortalModemLibrary.Structure
{
    public class RegisterationPacket : AbstractPacket
    {
        private static int NUMBER_COUNTER = 4;
        private static int ID_COUNTER = 1;
        private static int STATUS_COUNTER = 1;

        private string number;
        private int index;

        private int id;
        private int status;

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

        public int Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
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
        /// This constructor is used when packet need to be made for Write request.
        /// </summary>
        /// <param name="sim_number"></param>
        /// <param name="_number"></param>
        /// <param name="_index"></param>
        public RegisterationPacket(int sim_number, string _number, int _index)
        {
            this.SimNumber = sim_number;
            this.Cmd = AbstractPacket.WRITE_CMD;
            this.Number = _number;
            this.Index = _index;
        }

        // <summary>
        ///  This constructor is used when packet is made for Read request.
        /// </summary>
        /// <param name="sim_number"></param>
        public RegisterationPacket(int sim_number, int _index)
        {
            this.SimNumber = sim_number;
            this.Cmd = AbstractPacket.READ_CMD;
            this.Index = _index;
        }

        /// <summary>
        ///  This constructor is used when packet need to be parsed as object
        /// </summary>
        /// <param name="sim_number"></param>
        public RegisterationPacket(int sim_number)
        {
            this.SimNumber = sim_number;
        }



        public override AbstractPacket toObject(byte[] data)
        {
            try
            {
                base.toObject(data);

                Index = this.address - MOBILE_NUMBER_AV_START;

                if (Cmd == READ_CMD || Cmd == AUTO_CMD)
                {
                    this.Number = "0" + (_base + BitConverter.ToInt32(data.Skip(cIndex).Take(NUMBER_COUNTER).Reverse().ToArray(), 0)).ToString();

                    cIndex += NUMBER_COUNTER;

                    this.Id = data.Skip(cIndex).Take(ID_COUNTER).ToArray()[0];
                    cIndex += ID_COUNTER;

                    this.Status = data.Skip(cIndex).Take(STATUS_COUNTER).ToArray()[0];
                    cIndex += STATUS_COUNTER;

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
            if (this.Cmd == WRITE_CMD)
                packet = new byte[SRC_ADD_COUNTER + DEST_ADD_COUNTER + CMD_COUNTER + ADD_VAR_COUNTER + NUMBER_COUNTER + ID_COUNTER + STATUS_COUNTER];
            else
                packet = new byte[SRC_ADD_COUNTER + DEST_ADD_COUNTER + CMD_COUNTER + ADD_VAR_COUNTER];

            cIndex = SRC_ADD_COUNTER + DEST_ADD_COUNTER;


            packet[cIndex] = this.Cmd;
            cIndex += CMD_COUNTER;


            add_var = PacketUtility.IntToTwoBytes(getRegistrationAVBound(this.SimNumber)[0] + Index);
            Array.Copy(add_var, 0, packet, cIndex, ADD_VAR_COUNTER);
            cIndex += ADD_VAR_COUNTER;



            if (this.Cmd == WRITE_CMD)
            {
                number = number.Substring(2);
                int num = Convert.ToInt32(number);
                byte[] arr_number = PacketUtility.IntToByteArray(num);
                Array.Copy(arr_number, 0, packet, cIndex, NUMBER_COUNTER);
                cIndex += NUMBER_COUNTER;

                byte[] arr_id = { 0x00 };
                Array.Copy(arr_id, 0, packet, cIndex, ID_COUNTER);
                cIndex += ID_COUNTER;

                byte[] arr_status = { 0x00 };
                Array.Copy(arr_status, 0, packet, cIndex, STATUS_COUNTER);
                cIndex += STATUS_COUNTER;

            }

            return base.toRaw();



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


                return String.Format("Registeration => SIM: {0}, Command: {1}, " +
                                     "Id: {2}, Number: {3}, Status: {4} ",
                                      this.SimNumber, command,
                                      this.Id, this.Number, this.Status);

            }
            catch
            {
                return "Write Registration Packet Error";
            }
        }

    }

}
