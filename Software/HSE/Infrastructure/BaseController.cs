using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Models;

namespace HSE.Infrastructure
{
    public class BaseController : Controller
    {
        public BaseController()
        {
        }

        protected override System.IAsyncResult BeginExecuteCore(System.AsyncCallback callback, object state)
        {
            System.Globalization.CultureInfo oCultureInfo =
                new System.Globalization.CultureInfo("fa-IR");

            System.Threading.Thread.CurrentThread.CurrentCulture = oCultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = oCultureInfo;
            DatabaseContext db = new DatabaseContext();

            User user = GetUserInfo.GetUserFullName();
            if (user != null)
            {
                ViewBag.Name = user.FullName;

                List<Notification> notifications = db.Notifications.Where(c =>c.UserId==user.Id&& c.IsVisited == false && c.IsDeleted == false).ToList();
                ViewBag.notif = notifications.Take(6).ToList();
                ViewBag.notifCount = notifications.Count();
            }
          

            return base.BeginExecuteCore(callback, state);
        }
    }
}