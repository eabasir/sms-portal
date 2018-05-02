using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMSPortalDBDataLibrary;
using System.Data.Entity;
using SMSPortalWebPanel.ViewModels;
using SMSPortalCross;

namespace SMSPortalWebPanel.Models
{
    public class ReportBusinessLayer
    {

        public IEnumerable<ReportViewModel> getReport(Enums.SearchType type, string serachText, DateTime start, DateTime finish)
        {
            switch (type)
            {

                case Enums.SearchType.Contact:
                    return searchByContact(serachText, start, finish);
                case Enums.SearchType.Company:
                    return searchByCompany(serachText, start, finish);
                case Enums.SearchType.User:
                    return searchByUser(serachText, start, finish);
                case Enums.SearchType.Sendbox:
                    return searchBySendbox(serachText, start, finish);
                case Enums.SearchType.Inbox:
                    return searchByInbox(serachText, start, finish);

            }
            return null;
        }


        private IEnumerable<ReportViewModel> searchByContact(string searchText, DateTime start, DateTime finish)
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    var q1 = (from x in db.Queue_Phone
                              where
                                 (x.Phone.Contact.TFName.Contains(searchText) ||
                                 x.Phone.Contact.TFFamily.Contains(searchText) ||
                                 x.Phone.TFNumber.Contains(searchText))
                                 &&
                                ((x.Queue.TFDateTimeCreate >= start && x.Queue.TFDateTimeCreate <= finish) ||
                                (x.Queue.TFDateTimeRequest >= start && x.Queue.TFDateTimeRequest <= finish))

                              orderby x.Queue.TFDateTimeCreate descending
                              select new ReportViewModel
                              {
                                  Message = x.Queue.Message.TFContent,
                                  DateTimeFa = x.Queue.TFDateTimeCreateFA,
                                  SIM = "-",
                                  Contact = (x.Phone.Contact != null ? x.Phone.Contact.TFName + " " + x.Phone.Contact.TFFamily : "")
                                  + "، " +
                                  x.Phone.TFNumber,
                                  iSSent = true,
                                  Details = new List<string> {
                                     x.TFEnable ? "فعال" : "غیر فعال",
                                     x.TFIsUnderProcess ? "در صف ارسال" : "انتظار برای ارسال",
                                     "ساخته شده در: "+ x.Queue.TFDateTimeCreateFA,
                                     "ارسال در:" + x.Queue.TFDateTimeRequestFA,
                                     x.Queue.TFScheduleType == 0 ? "ارسال یک بار" : "ارسال زمانبندی شده"

                                 }
                              }).ToList();

                    var q2 = (from x in db.SendBox_Phone
                              where
                                 (x.Phone.Contact.TFName.Contains(searchText) ||
                                 x.Phone.Contact.TFFamily.Contains(searchText) ||
                                 x.Phone.TFNumber.Contains(searchText))
                                 &&
                                ((x.SendBox.TFDateTimeCreate >= start && x.SendBox.TFDateTimeCreate <= finish) ||
                                (x.TFDateTimeDelivery >= start && x.TFDateTimeDelivery <= finish))

                              orderby x.SendBox.TFDateTimeCreate descending
                              select new ReportViewModel
                              {
                                  Message = x.SendBox.Message.TFContent,
                                  DateTimeFa = x.SendBox.TFDateTimeCreateFA,
                                  Contact = (x.Phone.Contact != null ? x.Phone.Contact.TFName + " " + x.Phone.Contact.TFFamily : "")
                                  + "، " +
                                  x.Phone.TFNumber,
                                  iSSent = true,
                                  SIM = x.SIM.TFNumber,
                                  Details = new List<string> {
                                     "زمان ارسال به صف: " + x.TFDateTimeSendFA,
                                     "زمان ارسال در شبکه GSM " + x.TFDateTimeSendGSMFA,
                                     x.TFIsDelivered ? "دلیور شده در: " + x.TFDateTimeDeliveryFA : "دلیور نشده"

                                 }
                              }).ToList();

                    var q3 = (from x in db.Inboxes
                              where
                                x.TFSenderNumber.Contains(searchText)
                                &&
                                (x.TFDateTime >= start && x.TFDateTime <= finish)

                              orderby x.TFDateTime descending
                              select new ReportViewModel
                              {
                                  Message = x.TFMessage,
                                  DateTimeFa = x.TFDateTimeFA,
                                  Contact = x.TFSenderNumber,
                                  iSSent = false,
                                  SIM = x.SIM.TFNumber,
                                  Details = new List<string> {
                                    "زمان دریافت: " + x.TFDateTimeFA

                                 }
                              }).ToList();


                    return q1.Concat(q2).Concat(q3);



                }
            }
            catch { }
            return null;

        }

        private IEnumerable<ReportViewModel> searchByCompany(string searchText, DateTime start, DateTime finish)
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    Company company = (from x in db.Companies where x.TFName.Contains(searchText) select x).FirstOrDefault();
                    if (company == null)
                        return null;

                    var q1 = (from x in db.Queue_Phone
                              where
                              (from y in db.Contact_Company where y.TFCompany_Id == company.TFId select y.TFContact_Id).Contains((Guid)x.Phone.TFContact_Id)
                               &&
                                ((x.Queue.TFDateTimeCreate >= start && x.Queue.TFDateTimeCreate <= finish) ||
                                (x.Queue.TFDateTimeRequest >= start && x.Queue.TFDateTimeRequest <= finish))

                              orderby x.Queue.TFDateTimeCreate descending
                              select new ReportViewModel
                              {
                                  Message = x.Queue.Message.TFContent,
                                  DateTimeFa = x.Queue.TFDateTimeCreateFA,
                                  SIM = "-",
                                  Contact = (x.Phone.Contact != null ? x.Phone.Contact.TFName + " " + x.Phone.Contact.TFFamily : "")
                                  + "، " +
                                  x.Phone.TFNumber,
                                  iSSent = true,
                                  Details = new List<string> {
                                     x.TFEnable ? "فعال" : "غیر فعال",
                                     x.TFIsUnderProcess ? "در صف ارسال" : "انتظار برای ارسال",
                                     "ساخته شده در: "+ x.Queue.TFDateTimeCreateFA,
                                     "ارسال در:" + x.Queue.TFDateTimeRequestFA,
                                     x.Queue.TFScheduleType == 0 ? "ارسال یک بار" : "ارسال زمانبندی شده"

                                 }
                              }).ToList();

                    var q2 = (from x in db.SendBox_Phone
                              where
                                   (from y in db.Contact_Company where y.TFCompany_Id == company.TFId select y.TFContact_Id).Contains((Guid)x.Phone.TFContact_Id)
                                 &&
                                ((x.SendBox.TFDateTimeCreate >= start && x.SendBox.TFDateTimeCreate <= finish) ||
                                (x.TFDateTimeDelivery >= start && x.TFDateTimeDelivery <= finish))

                              orderby x.SendBox.TFDateTimeCreate descending
                              select new ReportViewModel
                              {
                                  Message = x.SendBox.Message.TFContent,
                                  DateTimeFa = x.SendBox.TFDateTimeCreateFA,
                                  Contact = (x.Phone.Contact != null ? x.Phone.Contact.TFName + " " + x.Phone.Contact.TFFamily : "")
                                  + "، " +
                                  x.Phone.TFNumber,
                                  iSSent = true,
                                  SIM = x.SIM.TFNumber,
                                  Details = new List<string> {
                                     "زمان ارسال به صف: " + x.TFDateTimeSendFA,
                                     "زمان ارسال در شبکه GSM " + x.TFDateTimeSendGSMFA,
                                     x.TFIsDelivered ? "دلیور شده در: " + x.TFDateTimeDeliveryFA : "دلیور نشده"

                                 }
                              }).ToList();

                    return q1.Concat(q2);



                }
            }
            catch { }
            return null;
        }
        private IEnumerable<ReportViewModel> searchByUser(string searchText, DateTime start, DateTime finish)
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {
                    User user = (from x in db.Users
                                 where
              x.TFName.Contains(searchText) ||
              x.TFFamily.Contains(searchText) ||
              x.TFUserName.Contains(searchText)

                                 select x).FirstOrDefault();
                    if (user == null)
                        return null;

                    var q1 = (from x in db.Queue_Phone
                              where
                             x.Queue.TFUser_Id == user.TFId
                              &&
                                ((x.Queue.TFDateTimeCreate >= start && x.Queue.TFDateTimeCreate <= finish) ||
                                (x.Queue.TFDateTimeRequest >= start && x.Queue.TFDateTimeRequest <= finish))

                              orderby x.Queue.TFDateTimeCreate descending
                              select new ReportViewModel
                              {
                                  Message = x.Queue.Message.TFContent,
                                  DateTimeFa = x.Queue.TFDateTimeCreateFA,
                                  SIM = "-",
                                  Contact = (x.Phone.Contact != null ? x.Phone.Contact.TFName + " " + x.Phone.Contact.TFFamily : "")
                                  + "، " +
                                  x.Phone.TFNumber,
                                  iSSent = true,
                                  Details = new List<string> {
                                     x.TFEnable ? "فعال" : "غیر فعال",
                                     x.TFIsUnderProcess ? "در صف ارسال" : "انتظار برای ارسال",
                                     "ساخته شده در: "+ x.Queue.TFDateTimeCreateFA,
                                     "ارسال در:" + x.Queue.TFDateTimeRequestFA,
                                     x.Queue.TFScheduleType == 0 ? "ارسال یک بار" : "ارسال زمانبندی شده"

                                 }
                              }).ToList();

                    var q2 = (from x in db.SendBox_Phone
                              where
                              x.SendBox.TFUser_Id == user.TFId &&
                                ((x.SendBox.TFDateTimeCreate >= start && x.SendBox.TFDateTimeCreate <= finish) ||
                                (x.TFDateTimeDelivery >= start && x.TFDateTimeDelivery <= finish))

                              orderby x.SendBox.TFDateTimeCreate descending
                              select new ReportViewModel
                              {
                                  Message = x.SendBox.Message.TFContent,
                                  DateTimeFa = x.SendBox.TFDateTimeCreateFA,
                                  Contact = (x.Phone.Contact != null ? x.Phone.Contact.TFName + " " + x.Phone.Contact.TFFamily : "")
                                  + "، " +
                                  x.Phone.TFNumber,
                                  iSSent = true,
                                  SIM = x.SIM.TFNumber,
                                  Details = new List<string> {
                                     "زمان ارسال به صف: " + x.TFDateTimeSendFA,
                                     "زمان ارسال در شبکه GSM " + x.TFDateTimeSendGSMFA,
                                     x.TFIsDelivered ? "دلیور شده در: " + x.TFDateTimeDeliveryFA : "دلیور نشده"

                                 }
                              }).ToList();

                    return q1.Concat(q2);



                }
            }
            catch { }
            return null;
        }

        private IEnumerable<ReportViewModel> searchBySendbox(string searchText, DateTime start, DateTime finish)
        {
            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {

                    var q1 = (from x in db.Queue_Phone
                              where
                             x.Queue.Message.TFContent.Contains(searchText)
                              &&
                                ((x.Queue.TFDateTimeCreate >= start && x.Queue.TFDateTimeCreate <= finish) ||
                                (x.Queue.TFDateTimeRequest >= start && x.Queue.TFDateTimeRequest <= finish))

                              orderby x.Queue.TFDateTimeCreate descending
                              select new ReportViewModel
                              {
                                  Message = x.Queue.Message.TFContent,
                                  DateTimeFa = x.Queue.TFDateTimeCreateFA,
                                  SIM = "-",
                                  Contact = (x.Phone.Contact != null ? x.Phone.Contact.TFName + " " + x.Phone.Contact.TFFamily : "")
                                  + "، " +
                                  x.Phone.TFNumber,
                                  iSSent = true,
                                  Details = new List<string> {
                                     x.TFEnable ? "فعال" : "غیر فعال",
                                     x.TFIsUnderProcess ? "در صف ارسال" : "انتظار برای ارسال",
                                     "ساخته شده در: "+ x.Queue.TFDateTimeCreateFA,
                                     "ارسال در:" + x.Queue.TFDateTimeRequestFA,
                                     x.Queue.TFScheduleType == 0 ? "ارسال یک بار" : "ارسال زمانبندی شده"

                                 }
                              }).ToList();

                    var q2 = (from x in db.SendBox_Phone
                              where
                             x.SendBox.Message.TFContent.Contains(searchText)
                             &&
                                ((x.SendBox.TFDateTimeCreate >= start && x.SendBox.TFDateTimeCreate <= finish) ||
                                (x.TFDateTimeDelivery >= start && x.TFDateTimeDelivery <= finish))

                              orderby x.SendBox.TFDateTimeCreate descending
                              select new ReportViewModel
                              {
                                  Message = x.SendBox.Message.TFContent,
                                  DateTimeFa = x.SendBox.TFDateTimeCreateFA,
                                  Contact = (x.Phone.Contact != null ? x.Phone.Contact.TFName + " " + x.Phone.Contact.TFFamily : "")
                                  + "، " +
                                  x.Phone.TFNumber,
                                  iSSent = true,
                                  SIM = x.SIM.TFNumber,
                                  Details = new List<string> {
                                     "زمان ارسال به صف: " + x.TFDateTimeSendFA,
                                     "زمان ارسال در شبکه GSM " + x.TFDateTimeSendGSMFA,
                                     x.TFIsDelivered ? "دلیور شده در: " + x.TFDateTimeDeliveryFA : "دلیور نشده"

                                 }
                              }).ToList();

                    return q1.Concat(q2);



                }
            }
            catch { }
            return null;
        }
        private IEnumerable<ReportViewModel> searchByInbox(string searchText, DateTime start, DateTime finish)
        {

            try
            {
                using (SMSPortalDBEntities db = new SMSPortalDBEntities())
                {

                    var q1 = (from x in db.Inboxes
                              where
                             x.TFMessage.Contains(searchText)
                              &&
                                (x.TFDateTime >= start && x.TFDateTime <= finish)

                              orderby x.TFDateTime descending
                              select new ReportViewModel
                              {
                                  Message = x.TFMessage,
                                  DateTimeFa = x.TFDateTimeFA,
                                  SIM = x.SIM.TFNumber,
                                  Contact = x.TFSenderNumber,
                                  iSSent = false,
                                  Details = new List<string> {
                                       "زمان دریافت: " + x.TFDateTimeFA
                                 }
                              }).ToList();



                    return q1;



                }
            }
            catch { }
            return null;
        }


    }
}
