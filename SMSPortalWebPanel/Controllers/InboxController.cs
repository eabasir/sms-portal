using SMSPortalWebPanel.Filters;
using SMSPortalWebPanel.Models;
using SMSPortalWebPanel.ViewModels;

using System.Web.Mvc;

namespace SMSPortalWebPanel.Controllers
{
    [PortalAuthorizatoin]
    public class InboxController : Controller
    {
        [HeaderFilter]
        public ActionResult Index()
        {
            InboxListViewModel inboxListVM = new InboxBusinessLayer().GetInbox();
            return View("index" , inboxListVM);
        }
    }
}