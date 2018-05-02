using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SMSPortalCross.Utils
{
    public class NumberUtils
    {

        public static string getFormattedNumber(string number)
        {

            Regex digitsOnly = new Regex(@"[^\d]");
            number = digitsOnly.Replace(number, "");

            if (number.StartsWith("98"))
                number = "0" + number.Substring(2);

            if (number.StartsWith("098"))
                number = "0" + number.Substring(3);


            if (number.StartsWith("0098"))
                number = "0" + number.Substring(4);

            if (number.Length == 10 && number.StartsWith("9")) //e.g: 9125975886
                number = "0" + number;


            return number;
        }



        public static bool isMobileNumber(string number)
        {

            if (string.IsNullOrEmpty(number))
                return false;


            number = getFormattedNumber(number);
            if (!number.StartsWith("09") || number.Length != 11)
                return false;

            return true;

        }



    }
}
