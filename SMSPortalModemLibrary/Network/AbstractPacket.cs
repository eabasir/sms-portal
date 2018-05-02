using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMSPortalModemLibrary.Utils;

namespace SMSPortalModemLibrary.Network
{
    public class AbstractPacket
    {
        public enum StructType
        {
            Registeration,
            Activation,
            Send,
            AutoId,
            Delivery,
            AutoDelivery,
            USSDWrite,
            USSDRead,
            USSDExecute,
            SMSText,
            Inbox,
            AutoInbox
        }

        public static string seperator_text = "Cortex-M3-SIM800-ME";

        public static string GSM_Sending_Error_text = "GSM-Error-Code:01";


        protected double _base = 9000000000;


        public static int BOUNDS_LENGTH = 5000;


        protected static int SRC_ADD_COUNTER = 2;
        protected static int DEST_ADD_COUNTER = 2;
        protected static int CMD_COUNTER = 1;
        protected static int ADD_VAR_COUNTER = 2;
        protected static int CRC_COUNTER = 2;

        public static byte READ_CMD = 0x52; //(R for manual read)
        public static byte WRITE_CMD = 0x57; //(W for write)
        public static byte AUTO_CMD = 0x41; //(A for auto answer)
        public static byte EXECUTE_CMD = 0x45; //(E for auto answer)

        protected int MOBILE_NUMBER_AV_START = 500;
        private int MOBILE_NUMBER_AV_END = 999;

        private int DELIVERY_AV_START = 1000;
        private int DELIVERY_AV_END = 1499;

        private int INBOX_AV_START = 1500;
        private int INBOX_AV_END = 1599;


        private int START_STOP_AV = 101;
        private int SMS_TEXT = 102;
        private int ACTIVATION_AV = 104;
        private int AUTO_ID_AV = 106;
        private int AUTO_DELIVERY_AV = 107;
        private int AUTO_INBOX_AV = 108;

        private int USSD_WRITE_AV = 109;
        private int USSD_EXECUTE_AV = 111;
        private int USSD_READ_AV = 110;


        protected int cIndex;


        public StructType type;

        // CMD => command can be one of READ_CMD, WRITE_CMD and AUTO_CMD
        private byte cmd;
        protected byte[] add_var = new byte[ADD_VAR_COUNTER];
        protected byte[] crc = new byte[CRC_COUNTER];

        protected byte[] packet;

        private int sim_number;
        protected int address;

        public byte Cmd
        {
            get
            {
                return cmd;
            }

            set
            {
                cmd = value;
            }
        }

        public int SimNumber
        {
            get
            {
                return sim_number;
            }

            set
            {
                sim_number = value;
            }
        }


        protected int getAutoInboxAV(int simNumber)
        {

            return ((simNumber - 1) * BOUNDS_LENGTH) + AUTO_INBOX_AV;
        }

        protected int getAutoDeliveryAV(int simNumber)
        {

            return ((simNumber - 1) * BOUNDS_LENGTH) + AUTO_DELIVERY_AV;
        }

        protected int getAutoIdAV(int simNumber)
        {

            return ((simNumber - 1) * BOUNDS_LENGTH) + AUTO_ID_AV;

        }

        protected int getActivationAV(int simNumber)
        {

            return ((simNumber - 1) * BOUNDS_LENGTH) + ACTIVATION_AV;

        }


        protected int getSendAV(int simNumber)
        {

            return ((simNumber - 1) * BOUNDS_LENGTH) + START_STOP_AV;

        }

        protected int getSMSTextAV(int simNumber)
        {

            return ((simNumber - 1) * BOUNDS_LENGTH) + SMS_TEXT;

        }


        protected int getUSSDWriteAV(int simNumber)
        {
            return ((simNumber - 1) * BOUNDS_LENGTH) + USSD_WRITE_AV;
        }


        protected int getUSSDExecuteAV(int simNumber)
        {
            return ((simNumber - 1) * BOUNDS_LENGTH) + USSD_EXECUTE_AV;
        }


        protected int getUSSDReadAV(int simNumber)
        {
            return ((simNumber - 1) * BOUNDS_LENGTH) + USSD_READ_AV;
        }


        protected int[] getRegistrationAVBound(int simNumber)
        {
            int[] bounds = new int[2];

            bounds[0] = ((simNumber - 1) * BOUNDS_LENGTH) + MOBILE_NUMBER_AV_START;
            bounds[1] = ((simNumber - 1) * BOUNDS_LENGTH) + MOBILE_NUMBER_AV_END;

            return bounds;

        }

        protected int[] getDeliveryAVBound(int simNumber)
        {
            int[] bounds = new int[2];

            bounds[0] = ((simNumber - 1) * BOUNDS_LENGTH) + DELIVERY_AV_START;
            bounds[1] = ((simNumber - 1) * BOUNDS_LENGTH) + DELIVERY_AV_END;

            return bounds;

        }

        protected int[] getInboxAVBound(int simNumber)
        {
            int[] bounds = new int[2];

            bounds[0] = ((simNumber - 1) * BOUNDS_LENGTH) + INBOX_AV_START;
            bounds[1] = ((simNumber - 1) * BOUNDS_LENGTH) + INBOX_AV_END;

            return bounds;

        }

        protected int getSimNumber(int av_value)
        {
            int i = 1;

            while ((av_value -= BOUNDS_LENGTH) > 0)
            {
                i++;
            }
            return i;
        }

        public virtual AbstractPacket toObject(byte[] data)
        {

            cIndex = SRC_ADD_COUNTER + DEST_ADD_COUNTER;


            this.Cmd = data.Skip(cIndex).Take(CMD_COUNTER).ToArray()[0];
            cIndex += CMD_COUNTER;



            add_var = data.Skip(cIndex).Take(ADD_VAR_COUNTER).ToArray();
            cIndex += ADD_VAR_COUNTER;
            this.address = BitConverter.ToInt16(add_var, 0);
            if (this.sim_number > 1)
                this.address -= ((this.sim_number - 1) * BOUNDS_LENGTH);

            return this;
        }

        public virtual byte[] toRaw()
        {
            addCRC();
            return packet;
        }

        protected void addCRC()
        {
            if (this.packet != null)
            {
                try
                {
                    byte[] temp = new byte[this.packet.Length + CRC_COUNTER];
                    byte[] crc = PacketUtility.HexStringToByteArray(Crc16.ComputeChecksum(this.packet).ToString("x2"));

                    Array.Copy(this.packet, 0, temp, 0, this.packet.Length);

                    Array.Copy(crc, 0, temp, this.packet.Length, CRC_COUNTER);

                    this.packet = temp;
                }
                catch (Exception e)
                {
                    throw e;
                }

            }

            else
                throw new NullReferenceException();

        }

        public override string ToString()
        {
            try
            {
                if (packet != null)
                    return string.Format("Unexpected Packet => {0}",PacketUtility.ByteArrayToHexString( packet));

                else
                    return "Null Packet";
            }
            catch
            {
                return "Unknown Packet;";
            }
        }

    }
}
