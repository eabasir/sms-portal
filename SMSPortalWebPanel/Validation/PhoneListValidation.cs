using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SMSPortalWebPanel.Validation
{
    public class PhoneListValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            List<string> Numbers = value as List<string>;
            if (Numbers == null)
                return false;

            foreach (string number in Numbers)
            {
                if (!Regex.IsMatch(number, "^[0-9]*$"))
                    return false;

                if (!SMSPortalCross.Utils.NumberUtils.isMobileNumber(number))
                    return false;

            }
            return true;

        }
    }
}