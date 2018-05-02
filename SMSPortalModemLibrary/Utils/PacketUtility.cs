using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMSPortalModemLibrary.Network;

namespace SMSPortalModemLibrary.Utils
{
    public class PacketUtility
    {

        public static byte[] IntToTwoBytes(int i)
        {
            byte[] res = new byte[2];
            res[0] = (byte)i;
            res[1] = (byte)(i >> 8);

            return res;

        }


        public static byte[] IntToByteArray(int i)
        {
            byte[] intBytes = BitConverter.GetBytes(i);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);

            return intBytes;
        }

        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            if (hex.Length == 2)
                hex = "00" + hex;

            else if (hex.Length < 4)
                hex = "0" + hex;

            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();



        }

        /// <summary>
        /// get packet seperator as byte array
        /// </summary>
        /// <returns>packet seperator byte array</returns>
        private static byte[] getPacketSeperator()
        {
            byte[] result= Encoding.ASCII.GetBytes(AbstractPacket.seperator_text);
            return result;
        }


      /// <summary>تبدیل 
      /// 
      /// Search for seperator indexes in data
      /// </summary>
      /// <param name="data"></param>
      /// <returns>list of found  packet start indexes => found index + pattern length </returns>
        public static List<int> search(byte[] data)
        {
            List<int> lstPattentIndex = new List<int>();

            byte[] pattern = getPacketSeperator();
            for (int i = 0; i <= data.Length - pattern.Length; i++)
            {
                if (match(data, getPacketSeperator(), i))
                {
                    lstPattentIndex.Add(i + pattern.Length);
                }
            }
            return  lstPattentIndex;
        }

        private static bool match(byte[] data, byte[] pattern,   int start)
        {
            if (pattern.Length + start > data.Length)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < pattern.Length; i++)
                {
                    if (pattern[i] != data[i + start])
                    {
                        return false;
                    }
                }
                return true;
            }
        }


        /// <summary>
        /// Finds packet seperator in a buffer and make list of seperated packets
        /// </summary>
        /// <param name="data"></param>
        /// <returns> list of seperated packets</returns>
        public static List<byte[]> getDataPackets(byte[] data)
        {

            List<byte[]> result = new List<byte[]>();

            List<byte[]> temps = new List<byte[]>();



            List<int> lstPatternIndexes = PacketUtility.search(data);

            for (int i = 0; i < lstPatternIndexes.Count; i++)
            {
                if ((lstPatternIndexes.Count - 1) >= (i + 1))
                {
                    temps.Add(data.Skip(lstPatternIndexes[i]).Take(lstPatternIndexes[i + 1] - lstPatternIndexes[i]).ToArray());
                }
                else
                {
                    temps.Add(data.Skip(lstPatternIndexes[i]).ToArray());

                }

            }

            return temps;


        }


        public static int messagePagesCounter(string message) {
            if (string.IsNullOrEmpty(message))
                return 0;

            if (message.Length <= 70)
                return 1;

            if (message.Length > 70 && message.Length <= 134)
                return 2;

            if (message.Length > 134 && message.Length <= 201)
                return 3;

            if (message.Length > 201 && message.Length <= 238)
                return 4;

            if (message.Length > 238 && message.Length <= 3000)
                return 5;


            return 0;
        }

        public static bool isRelatedId(int refId, int receviedId, int pages) {

            if (pages == 1) {
                if (receviedId == refId)
                    return true;
                else
                    return false;
            }

            for (int i = refId; i < pages; i--) {
                if (i < 0)
                    i += 255;

                if (receviedId == i)
                    return true;
 
            }

            return false;
        }



    }
}
