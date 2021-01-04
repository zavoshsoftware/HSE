using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace HSE.Controllers
{
    public class NotificationsController : Infrastructure.BaseController
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

    [Authorize(Roles = "Administrator,company,supervisor")]
        public ActionResult List()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string uId = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

            Guid userId = new Guid(uId);

            var notifications = db.Notifications
                .Where(c => c.UserId == userId && c.IsDeleted == false && c.IsVisited == false).ToList();
           
            return View(notifications);
        }
    }
}