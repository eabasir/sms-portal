using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SMSPortalCross.Utils
{
   public class StringUtils
    {

      public static  string getMd5(string input)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(input);

            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            string encoded = BitConverter.ToString(hash)
              .Replace("-", string.Empty)
              .ToLower();
            return encoded;
        }



        private static readonly string[] pn = { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };
        private static readonly string[] en = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        public static string ToPersianNumber( string strNum)
        {
            string chash = strNum;
            for (int i = 0; i < 10; i++)
                chash = chash.Replace(en[i], pn[i]);
            return chash;
        }
        public static string ToEnglishNumber( string strNum)
        {
            string chash = strNum;
            for (int i = 0; i < 10; i++)
                chash = chash.Replace(pn[i], en[i]);
            return chash;
        }
    }
}
