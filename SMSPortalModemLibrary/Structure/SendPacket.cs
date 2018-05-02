using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMSPortalModemLibrary.Network;
using SMSPortalModemLibrary.Utils;

namespace SMSPortalModemLibrary.Structure
{
    public class SendPacket : AbstractPacket
    {
        private static int STATUS_COUNTER = 1;

        private int status;

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


        /// <summary>
        /// This constructor is used when packet need to be made as raw byte array
        /// </summary>
        /// <param name="sim_number"></param>
        /// <param name="_cmd"></param>
        /// <param name="_status"></param>
        public SendPacket(int sim_number, byte _cmd, int _status)
        {
            this.SimNumber = sim_number;
            this.Cmd = _cmd;
            this.Status = _status;
        }


        /// <summary>
        /// This constructor is used when packet need to be parsed as object
        /// </summary>
        /// <param name="sim_number"></param>
        public SendPacket(int sim_number)
        {
            this.SimNumber = sim_number;
        }

        public override AbstractPacket toObject(byte[] data)
        {
            try
            {
                base.toObject(data);
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
                packet = new byte[SRC_ADD_COUNTER + DEST_ADD_COUNTER + CMD_COUNTER + ADD_VAR_COUNTER + STATUS_COUNTER];
            else // e.g read and ...
                packet = new byte[SRC_ADD_COUNTER + DEST_ADD_COUNTER + CMD_COUNTER + ADD_VAR_COUNTER]; // TODO : should be investigated later

            cIndex = SRC_ADD_COUNTER + DEST_ADD_COUNTER;

            packet[cIndex] = this.Cmd;
            cIndex += CMD_COUNTER;

            add_var = PacketUtility.IntToTwoBytes(getSendAV(this.SimNumber));
            Array.Copy(add_var, 0, packet, cIndex, ADD_VAR_COUNTER);
            cIndex += ADD_VAR_COUNTER;

            packet[cIndex] = (byte)Status;
            cIndex += STATUS_COUNTER;

            return base.toRaw();
        }



        public override string ToString()
        {
            try {             string command = string.Empty;
            if (this.Cmd == AUTO_CMD)
                command = "Auto";
            else if (this.Cmd == WRITE_CMD)
                command = "Write";
            else if (this.Cmd == READ_CMD)
                command = "Read";


            return String.Format("Send => SIM: {0}, Command: {1}, " +
                                 "Status: {2}",
                                  this.SimNumber, command,
                                  this.Status);

        }
            catch {
                return "Write Send Packet Error";
            }
        }


    }

}
