using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Helpers
{
    public static class NotificationHelper
    {
 

        public static void InsertNotification(string companyTitle, string url, string notifType)
        {
            DatabaseContext db = new DatabaseContext();
            List<User> admin = db.Users.Where(c => c.Role.Name == "Administrator").ToList();

            foreach (User user in admin)
            {
                Notification notification = new Notification()
                {
                    UserId = user.Id,
                    CreationDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    IsActive = true,
                    IsDeleted = false,
                    IsVisited = false,
                    Title = "یک رکورد جدید توسط "+companyTitle+" در ماژول "+notifType+" ثبت شده است",
                    Url = url,
                };

                db.Notifications.Add(notification);
                
            }
            db.SaveChanges();

        }
    }
}