using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMSPortalModemLibrary.Network;
using SMSPortalModemLibrary.Utils;

namespace SMSPortalModemLibrary.Structure
{
    public class SMSTextPacket : AbstractPacket
    {
        public static int MESSAGE_TEXT_COUNTER = 600;

        private string message;


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



        /// <summary>
        /// This constructor is used when need to make raw packet to write message of sms 
        /// </summary>
        /// <param name="sim_number"></param>
        /// <param name="_message"></param>
        public SMSTextPacket(int sim_number, string _message)
        {
            this.SimNumber = sim_number;
            this.Message = _message;
            this.Cmd = AbstractPacket.WRITE_CMD;
        }


        /// <summary>
        /// This constructor is used when need to make Object from raw packet to read current sms text
        /// </summary>
        /// <param name="sim_number"></param>
        public SMSTextPacket(int sim_number)
        {
            this.SimNumber = sim_number;
            this.Cmd = AbstractPacket.READ_CMD;
        }


        public override AbstractPacket toObject(byte[] data)
        {
            try
            {
                base.toObject(data);

                if (Cmd == READ_CMD)
                {
                    byte[] temp = data.Skip(cIndex).Take(MESSAGE_TEXT_COUNTER).ToArray();
                    this.Message = Encoding.BigEndianUnicode.GetString(temp);
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
            try
            {

                if (this.Cmd == WRITE_CMD)
                    packet = new byte[SRC_ADD_COUNTER + DEST_ADD_COUNTER + CMD_COUNTER + ADD_VAR_COUNTER + MESSAGE_TEXT_COUNTER];
                else
                    packet = new byte[SRC_ADD_COUNTER + DEST_ADD_COUNTER + CMD_COUNTER + ADD_VAR_COUNTER];


                cIndex = SRC_ADD_COUNTER + DEST_ADD_COUNTER;

                packet[cIndex] = this.Cmd;
                cIndex += CMD_COUNTER;

                add_var = PacketUtility.IntToTwoBytes(getSMSTextAV(this.SimNumber));
                Array.Copy(add_var, 0, packet, cIndex, ADD_VAR_COUNTER);
                cIndex += ADD_VAR_COUNTER;

                if (this.Cmd == AbstractPacket.WRITE_CMD)
                {
                    byte[] temp = Encoding.BigEndianUnicode.GetBytes(this.Message);

                    if (temp.Length > MESSAGE_TEXT_COUNTER)
                        throw new IndexOutOfRangeException();

                    byte[] message_bytes = new byte[MESSAGE_TEXT_COUNTER];
                    Array.Copy(temp, 0, message_bytes, 0, temp.Length);

                    Array.Copy(message_bytes, 0, packet, cIndex, MESSAGE_TEXT_COUNTER);
                    cIndex += MESSAGE_TEXT_COUNTER;
                }

                return base.toRaw();
            }
            catch (Exception e) {
                throw;
            }
        }



        public override string ToString()
        {
            try { 
            string command = string.Empty;
            if (this.Cmd == AUTO_CMD)
                command = "Auto";
            else if (this.Cmd == WRITE_CMD)
                command = "Write";
            else if (this.Cmd == READ_CMD)
                command = "Read";


            return String.Format("SMS Text => SIM: {0}, Command: {1}, " +
                                 "Message: {2}",
                                  this.SimNumber, command,
                                  this.Message);

        }
            catch {
                return "Write SMS Text Packet Error";
            }
        }

    }
}
