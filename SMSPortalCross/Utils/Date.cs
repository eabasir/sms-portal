using System;
using System.Linq;


namespace SMSPortalCross.Utils
{
    public class Date
    {
        public static string GetPersianDateString()
        {
            return String.Format(Persia.Calendar.ConvertToPersian(DateTime.Now).ToString());
        }
        public static string GetPersianDateTimeString()
        {

            return string.Format("{0} {1}", GetPersianDateString(), GetPersianTimeString());
        }
        public static string GetPersianTimeString()
        {
            return String.Format("{0:00}:{1:00}:{2:00}:{3:000}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
        }


        public static DateTime PersianToGregorian(string pdate)
        {
            string[] parts = pdate.Split(new Char[] { '/' });
          
            int year = Convert.ToInt32(StringUtils.ToEnglishNumber(parts[0]));
            int month = Convert.ToInt32(StringUtils.ToEnglishNumber(parts[1]));
            int day = Convert.ToInt32(StringUtils.ToEnglishNumber(parts[2]));

            try
            {
                return Persia.Calendar.ConvertToGregorian(year, month, day, Persia.DateType.Persian);
            }
            catch
            {
                return Persia.Calendar.ConvertToGregorian(day, month, year, Persia.DateType.Persian);
            }
        }

        public static DateTime? CompletePersianToGregorian(string pdate)
        {
            DateTime dt = DateTime.Now;

            try
            {
                string[] parts = pdate.Split(' ');

                parts = parts.Where(x => x != string.Empty).ToArray();

                string[] dateParts = parts[0].Split('/');

                int year = Convert.ToInt32(StringUtils.ToEnglishNumber(dateParts[0]));
                int month = Convert.ToInt32(StringUtils.ToEnglishNumber(dateParts[1]));
                int day = Convert.ToInt32(StringUtils.ToEnglishNumber(dateParts[2]));
                try
                {
                    dt = Persia.Calendar.ConvertToGregorian(year, month, day, Persia.DateType.Persian);
                }
                catch
                {
                    dt = Persia.Calendar.ConvertToGregorian(day, month, year, Persia.DateType.Persian);
                }

                string[] timeParts = parts[1].Split(':');

                int hour = Convert.ToInt32(StringUtils.ToEnglishNumber(timeParts[0]));
                int minute = Convert.ToInt32(StringUtils.ToEnglishNumber(timeParts[1]));
                int second = Convert.ToInt32(StringUtils.ToEnglishNumber(timeParts[2]));

                TimeSpan ts = new TimeSpan(hour, minute, second);

                dt = dt + ts;
                return dt;
            }
            catch (Exception e)
            {

            }
            return null;
        }

        public static string GregorianToPersian(DateTime date)
        {
            return Persia.Calendar.ConvertToPersian(date).ToString();
        }

        public static string CompleteGregorianToPersian(DateTime date)
        {

            string time = date.ToString("HH:mm");
            return Persia.Calendar.ConvertToPersian(date).ToString() + " " + time;
        }





    }

}
