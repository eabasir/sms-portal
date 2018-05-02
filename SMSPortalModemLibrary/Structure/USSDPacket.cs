using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMSPortalModemLibrary.Network;
using SMSPortalModemLibrary.Utils;

namespace SMSPortalModemLibrary.Structure
{
    // This class has 3 usages. each one can have own Read and Write Requests
    // 1) Write USSD code.
    // 2) Execute USSD.
    // 3) Read USSD Answer.

    public class USSDPacket : AbstractPacket
    {
        public enum USSDType
        {
            Execute,
            Read,
            Write
        }

        private static int RES_COUNTER = 1;
        private static int MESSAGE_COUNTER = 300;

        private string requestAnswer;
        private USSDType ussdType;
        public string RequestAnswer
        {
            get
            {
                return requestAnswer;
            }

            set
            {
                requestAnswer = value;
            }
        }



        public USSDType UssdType
        {
            get
            {
                return ussdType;
            }

            set
            {
                ussdType = value;
            }
        }
        /// <summary>
        /// This constructor is used when need to make Object from raw packet.
        /// </summary>
        /// <param name="sim_number"></param>
        public USSDPacket(int sim_number)
        {
            this.SimNumber = sim_number;

        }


        /// <summary>
        /// This constructor is used when need to make raw packet.
        /// Used for Write USSD code, Read USSD answer, and Execute USSD.
        /// </summary>
        /// <param name="sim_number"></param>
        /// <param name="_requestAnswer">Code of ussd command. (Not null only when _ussdType eqauls Write )</param>
        /// <param name="_ussdType">Can have: Read, Write and Execute</param>
        public USSDPacket(int sim_number, USSDType _ussdType, string _requestAnswer)
        {
            this.SimNumber = sim_number;
            this.ussdType = _ussdType;
            this.requestAnswer = _requestAnswer;

        }



        public override AbstractPacket toObject(byte[] data)
        {
            try
            {
                base.toObject(data);

                if (Cmd == READ_CMD)
                {
                    byte[] temp = data.Skip(cIndex).Take(MESSAGE_COUNTER).ToArray();
                    this.RequestAnswer = Encoding.BigEndianUnicode.GetString(temp);
                    cIndex += MESSAGE_COUNTER;

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
            if (ussdType == USSDType.Write)
                packet = new byte[SRC_ADD_COUNTER + DEST_ADD_COUNTER + CMD_COUNTER + ADD_VAR_COUNTER + MESSAGE_COUNTER + RES_COUNTER];
            else if (ussdType == USSDType.Execute)
                packet = new byte[SRC_ADD_COUNTER + DEST_ADD_COUNTER + CMD_COUNTER + ADD_VAR_COUNTER + RES_COUNTER];
            else if (ussdType == USSDType.Read)
                packet = new byte[SRC_ADD_COUNTER + DEST_ADD_COUNTER + CMD_COUNTER + ADD_VAR_COUNTER];


            cIndex = SRC_ADD_COUNTER + DEST_ADD_COUNTER;

            if (ussdType == USSDType.Write)
                this.Cmd = AbstractPacket.WRITE_CMD;
            else if (ussdType == USSDType.Execute)
                this.Cmd = AbstractPacket.EXECUTE_CMD;
            else if (ussdType == USSDType.Read)
                this.Cmd = AbstractPacket.READ_CMD;

            packet[cIndex] = this.Cmd;
            cIndex += CMD_COUNTER;

            switch (this.ussdType)
            {
                case USSDType.Write:
                    add_var = PacketUtility.IntToTwoBytes(getUSSDWriteAV(this.SimNumber));
                    break;
                case USSDType.Read:
                    add_var = PacketUtility.IntToTwoBytes(getUSSDReadAV(this.SimNumber));
                    break;
                case USSDType.Execute:
                    add_var = PacketUtility.IntToTwoBytes(getUSSDExecuteAV(this.SimNumber));
                    break;
            }

            Array.Copy(add_var, 0, packet, cIndex, ADD_VAR_COUNTER);
            cIndex += ADD_VAR_COUNTER;

            if (this.ussdType == USSDType.Write)
            {
                byte[] temp = Encoding.UTF8.GetBytes(this.RequestAnswer);

                byte[] code_bytes = new byte[300];
                Array.Copy(temp, 0, code_bytes, 0, temp.Length);


                Array.Copy(code_bytes, 0, packet, cIndex, MESSAGE_COUNTER);
                cIndex += MESSAGE_COUNTER;

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


                return String.Format("USSD => SIM: {0}, Command: {1}, " +
                                     "USSD Type: {2}, Received Message: {3}",
                                      this.SimNumber, command,
                                      this.UssdType.ToString(), this.RequestAnswer);

            }
            catch
            {
                return "Write USSD Packet Error";
            }
        }
    }

}
