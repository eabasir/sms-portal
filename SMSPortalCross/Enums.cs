using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSPortalCross
{
    public class Enums
    {

        public enum UserLevel
        {
            Admin = 0,
            User = 1,
            NotAuthenticated = 2,
            DefaultOrFirstTime =3
        }


        public enum Schedule
        {
            None = 0,
            Once = 1,
            Daily = 2,
            WeekLy = 3,
            Monthly = 4,
            NowANDDaily = 5,
            NowANDWeekly = 6,
            NowANDMonthly = 7

        }



        public enum SearchType
        {
            Contact = 1,
            Company = 2,
            User = 3,
            Sendbox = 4,
            Inbox = 5,
    
        }
        public static string getPersianSchedule(int n)
        {
            switch (n)
            {
                case (int)SMSPortalCross.Enums.Schedule.None:
                    return "بدون تکرار";

                case (int)SMSPortalCross.Enums.Schedule.Once:
                    return "یکبار";
                case (int)SMSPortalCross.Enums.Schedule.Daily:
                    return "روزانه";
                case (int)SMSPortalCross.Enums.Schedule.WeekLy:
                    return "هفتگی";
                case (int)SMSPortalCross.Enums.Schedule.Monthly:
                    return "ماهانه";
                case (int)SMSPortalCross.Enums.Schedule.NowANDDaily:
                    return "در لحظه و با تکرار روزانه";
                case (int)SMSPortalCross.Enums.Schedule.NowANDWeekly:
                    return "در لحظه و با تکرار هفتگی";
                case (int)SMSPortalCross.Enums.Schedule.NowANDMonthly:
                    return "در لحظه و با تکرار ماهانه";
                default:
                    return "";


            }
        }


        public static bool getDefultActiveState(int n)
        {
            switch (n)
            {
                case (int)Schedule.None:
                    return true;

                case (int)Schedule.Once:
                    return false;
                case (int)Schedule.Daily:
                    return false;
                case (int)Schedule.WeekLy:
                    return false;
                case (int)Schedule.Monthly:
                    return false;
                case (int)Schedule.NowANDDaily:
                    return true;
                case (int)Schedule.NowANDWeekly:
                    return true;
                case (int)Schedule.NowANDMonthly:
                    return true;
                default:
                    return true;


            }

        }

        public static Schedule getSendType(int type, bool isNow)
        {

            switch (type)
            {

                case 0:
                    if (isNow) return Schedule.None; else return Schedule.Once;

                case 1:
                    if (isNow) return Schedule.NowANDDaily; else return Schedule.Daily;
                case 2:
                    if (isNow) return Schedule.NowANDWeekly; else return Schedule.WeekLy;

                case 3:
                    if (isNow) return Schedule.NowANDMonthly; else return Schedule.Monthly;

                default:
                    return Schedule.None;
            }


        }


        public static bool isOnceType(int n)
        {
            if (n == (int)Schedule.None || n == (int)Schedule.Once)
                return true;
            else
                return false;

        }

        public static SearchType getSearchType(int type)
        {

            switch (type)
            {

                case 1:
                    return SearchType.Contact;
                case 2:
                    return SearchType.Company;
                case 3:
                    return SearchType.User;
                case 4:
                    return SearchType.Sendbox;
                case 5:
                    return SearchType.Inbox;
                default:
                    return SearchType.Contact;
            }


        }


        public static UserLevel getUserType(int type)
        {

            switch (type)
            {

                case 0:
                    return UserLevel.Admin;
                case 1:
                    return UserLevel.User;
                case 2:
                    return UserLevel.NotAuthenticated;
                case 3:
                    return UserLevel.DefaultOrFirstTime;
                default:
                    return UserLevel.NotAuthenticated;
             }


        }




    }
}
