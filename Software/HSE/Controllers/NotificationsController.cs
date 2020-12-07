using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace HSE.Controllers
{
    public class NotificationsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        public ActionResult Index(Guid id)
        {
            Notification notification = db.Notifications.Find(id);
            if (notification != null)
            {
                notification.IsVisited = true;
                notification.LastModifiedDate=DateTime.Now;

                db.SaveChanges();
                return Redirect(notification.Url);
            }
            return View();
        }
    }
}