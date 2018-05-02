using Newtonsoft.Json;
using SMSPortalWebPanel.Filters;
using SMSPortalWebPanel.Models;
using SMSPortalWebPanel.ViewModels;
using System;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using static SMSPortalCross.Enums;

namespace SMSPortalWebPanel.Controllers
{

    [PortalAuthorizatoin]
    public class NewMessageController : Controller
    {


        [HeaderFilter]
        public ActionResult Index()
        {
            BaseViewModel bvm = new NewMessageViewModel();
            return View("Index", bvm);
        }


        [HttpPost]
        public ActionResult SendNewMessage(string data)
        {

            NewMessageViewModel vm = JsonConvert.DeserializeObject<NewMessageViewModel>(data);
            myJsonResult result = new myJsonResult();
            if (string.IsNullOrEmpty(vm.Message) || vm.Message.Length > 300)
            {
                result.Error = "ورود متن پیام الزامی است و حداکثر باید 300 کاراکتر باشد.";
                return Json(JsonConvert.SerializeObject(result));
            }
            vm.Numbers.Remove("");
            if ((vm.Numbers == null || vm.Numbers.Count == 0) && (string.IsNullOrEmpty(vm.StartRange) || string.IsNullOrEmpty(vm.FinishRange)))
            {
                result.Error = "شماره های وارد شده معتبر نمی باشد";
                return Json(JsonConvert.SerializeObject(result));
            }


            long _start = 0;
            long _finish = 0;

            if (!string.IsNullOrEmpty(vm.StartRange) && !string.IsNullOrEmpty(vm.FinishRange))
            {

                vm.StartRange = SMSPortalCross.Utils.NumberUtils.getFormattedNumber(vm.StartRange);
                vm.FinishRange = SMSPortalCross.Utils.NumberUtils.getFormattedNumber(vm.FinishRange);

                if (!SMSPortalCross.Utils.NumberUtils.isMobileNumber(vm.StartRange)
                    || !SMSPortalCross.Utils.NumberUtils.isMobileNumber(vm.FinishRange))
                {
                    result.Error = "شماره های ابتدا و انتهای بازه صحیح نمی باشند";
                    return Json(JsonConvert.SerializeObject(result));
                }


                _start = Convert.ToInt64(vm.StartRange);
                _finish = Convert.ToInt64(vm.FinishRange);

                if (_finish - _start < 0)
                {
                    result.Error = "شماره ابتدایی بازه بزرگتر از شماره انتهایی است";
                    return Json(JsonConvert.SerializeObject(result));
                }
            }

            bool isNow = string.IsNullOrEmpty(vm.StrDTSend) ? true : false;

            DateTime dtSend;
            if (!isNow)
            {
                DateTime? dt = SMSPortalCross.Utils.Date.CompletePersianToGregorian(vm.StrDTSend);
                if (dt != null)
                {

                    dtSend = (DateTime)dt;
                    if (dtSend < DateTime.Now)
                    {
                        result.Error = "تاریخ وارد شده از تاریخ فعلی گذشته است";
                        return Json(JsonConvert.SerializeObject(result));
                    }


                }
                else
                {
                    result.Error = "تاریخ وارد شده صحیح نمی باشد";
                    return Json(JsonConvert.SerializeObject(result));
                }

            }
            else
                dtSend = DateTime.Now;

            SMSPortalService.UserServiceClient client = new SMSPortalService.UserServiceClient();

            Schedule schedule = getSendType(vm.RepeatType, isNow);

            string userName = HttpContext.User.Identity.Name;
            string password = new AuthenticationBusinessLayer().getPassword(userName);


            int total = 0;

            if (vm.Numbers.Count <= SMSPortalCross.Constants.MAX_NUMBER_TO_INSERT)
            {
                client.AddToQueue(userName, password, vm.Message, vm.Numbers.ToArray(), schedule, dtSend);
                total = vm.Numbers.Count;

            }
            else
            {

                if (vm.Numbers.Count > 0)
                {
                    int quotient = vm.Numbers.Count / SMSPortalCross.Constants.MAX_NUMBER_TO_INSERT;

                    for (int i = 0; i <= quotient; i++)
                    {
                        List<string> temp = vm.Numbers.Skip(i * SMSPortalCross.Constants.MAX_NUMBER_TO_INSERT).Take(SMSPortalCross.Constants.MAX_NUMBER_TO_INSERT).ToList();
                        client.AddToQueue(userName, password, vm.Message, vm.Numbers.ToArray(), schedule, dtSend);

                    }
                    total += vm.Numbers.Count;
                }
                
            }
            if (_start > 0 && _finish > 0)
            {
                client.AddRangeToQueue(userName, password, vm.Message, _start, _finish, schedule, dtSend);
                total += Convert.ToInt32(_finish - _start);
            }

            result.Result = "تعداد " + total + " با موفقیت در صف ارسال قرار گرفت";
            return Json(JsonConvert.SerializeObject(result));


        }

        private Schedule getSendType(int type, bool isNow)
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

        /// <summary>
        /// gets all numbers (whether is under process or is already sent) for aspecific sendBoxId 
        /// </summary>
        /// <param name="sendBoxId"></param>
        /// <returns></returns>
        [HeaderFilter]
        public ActionResult CopySent(Guid id)
        {
            NewMessageViewModel newMessageVM = new NewMessageBusinessLayer().copySent(id);

            return View("Index", newMessageVM);
        }

        [HeaderFilter]
        public ActionResult CopySentPhone(Guid id)
        {
            NewMessageViewModel bvm = new NewMessageBusinessLayer().CopySentPhone(id);
            return View("Index", bvm);
        }

        [HeaderFilter]
        public ActionResult SendMessageToContact(Guid id)
        {
            NewMessageViewModel bvm = new NewMessageBusinessLayer().SendMessageToContact(id);
            return View("Index", bvm);
        }


    }
}