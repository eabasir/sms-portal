using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMSPortalModemLibrary.Network;
using SMSPortalModemLibrary.Utils;

namespace SMSPortalModemLibrary.Structure
{
    public class ActivationPacket : AbstractPacket
    {
        private static int START_COUNTER = 1;
        private static int FINISH_COUNTER = 1;
        private static int WORKING_INDEX_COUNTER = 1;
        private static int Unused_COUNTER = 1;
        private static int RES_COUNTER = 4;

        private int start;
        private int finish;
        private int workingIndex;

        public int Start
        {
            get
            {
                return start;
            }

            set
            {
                start = value;
            }
        }

        public int Finish
        {
            get
            {
                return finish;
            }

            set
            {
                finish = value;
            }
        }


        /// <summary>
        /// Working index is the last index which modem has tried to send.
        /// Under some specific condition such as GSM_SENDING_ERROR (see: AbstarctPacket.GSM_SENDING_ERROR_Text), it is used to
        /// determine what was the last index which modem has tried to send before error happens.
        /// If it is needed to re-active numbers (such as mentioned error), it is meaningul to increase this index by one and put it
        /// as Start index to skip error number.
        /// </summary>
        public int WorkingIndex
        {
            get
            {
                return workingIndex;
            }

            set
            {
                workingIndex = value;
            }
        }

        /// <summary>
        /// This constructor is used when packet need to be made as raw byte array
        /// </summary>
        /// <param name="sim_number"></param>
        /// <param name="_start"></param>
        /// <param name="_finish"></param>
        public ActivationPacket(int sim_number, int _start, int _finish)
        {
            this.SimNumber = sim_number;
            this.Cmd = AbstractPacket.WRITE_CMD;
            this.Start = _start;
            this.Finish = _finish;
        }


        /// <summary>
        /// This constructor is used when packet need to be parsed as object.
        /// Both Request and Write commands use  this constructor.
        /// </summary>
        /// <param name="sim_number"></param>
        public ActivationPacket(int sim_number)
        {
            this.SimNumber = sim_number;
        }



        public override AbstractPacket toObject(byte[] data)
        {
            try
            {
                base.toObject(data);

                if (Cmd == READ_CMD)
                {
                    this.Start = data.Skip(cIndex).Take(START_COUNTER).ToArray()[0];
                    cIndex += START_COUNTER;

                    this.Finish = data.Skip(cIndex).Take(FINISH_COUNTER).ToArray()[0] ;
                    cIndex += FINISH_COUNTER;

                    cIndex += Unused_COUNTER; // third byte in data is already unused, 4th byte is related to working index

                    this.WorkingIndex = data.Skip(cIndex).Take(WORKING_INDEX_COUNTER).ToArray()[0];
                    cIndex += FINISH_COUNTER;


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
                packet = new byte[SRC_ADD_COUNTER + DEST_ADD_COUNTER + CMD_COUNTER + ADD_VAR_COUNTER + START_COUNTER + FINISH_COUNTER + Unused_COUNTER + WORKING_INDEX_COUNTER + RES_COUNTER];
            else
                packet = new byte[SRC_ADD_COUNTER + DEST_ADD_COUNTER + CMD_COUNTER + ADD_VAR_COUNTER];

            cIndex = SRC_ADD_COUNTER + DEST_ADD_COUNTER;

            packet[cIndex] = this.Cmd;
            cIndex += CMD_COUNTER;

            add_var = PacketUtility.IntToTwoBytes(getActivationAV(this.SimNumber));
            Array.Copy(add_var, 0, packet, cIndex, ADD_VAR_COUNTER);
            cIndex += ADD_VAR_COUNTER;

            if (this.Cmd == AbstractPacket.WRITE_CMD)
            {

                packet[cIndex] = (byte)Start;
                cIndex += START_COUNTER;

                packet[cIndex] = (byte)Finish;
                cIndex += FINISH_COUNTER;

                cIndex += Unused_COUNTER; // third byte in data is already unused, 4th byte is related to working index

                packet[cIndex] = (byte)Start; // Working index should be equal to start value
                cIndex += WORKING_INDEX_COUNTER;

                byte[] res = { 0x00, 0x00, 0x00, 0x00 };
                Array.Copy(res, 0, packet, cIndex, RES_COUNTER);
                cIndex += RES_COUNTER;
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


                return String.Format("Activation => SIM: {0}, Command: {1}, ",
                                      this.SimNumber, command);
            }
            catch {
                return "Write Activation Packet Error";
            }
        }
    }





}
