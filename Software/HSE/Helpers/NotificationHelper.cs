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
                    Title = "یک رکورد جدید توسط " + companyTitle + " در ماژول " + notifType + " ثبت شده است",
                    Url = url,
                };

                db.Notifications.Add(notification);

            }
            db.SaveChanges();

        }

        public static void InsertNotificationForSup(Guid? companyId, string companyTitle, string url, string notifType)
        {
            DatabaseContext db = new DatabaseContext();

            Company company = db.Companies.FirstOrDefault(c => c.Id == companyId);

            if (company != null)
            {
                Guid? supervisorUserId = company.SupervisorUserId;

                if (supervisorUserId != null)
                {
                    Notification notification = new Notification()
                    {
                        UserId = supervisorUserId.Value,
                        CreationDate = DateTime.Now,
                        Id = Guid.NewGuid(),
                        IsActive = true,
                        IsDeleted = false,
                        IsVisited = false,
                        Title = "یک رکورد جدید توسط " + companyTitle + " در ماژول " + notifType + " ثبت شده است",
                        Url = url,
                    };

                    db.Notifications.Add(notification);
                }
                db.SaveChanges();

            }
        }
        public static void InsertNotificationForCompany(Guid? companyId, string role, string url, string notifType,string editMode)
        {
            DatabaseContext db = new DatabaseContext();

            User user = db.Users.FirstOrDefault(c => c.CompanyId == companyId);

            if (user != null)
            {
                string title;
                if (editMode == "edit")
                    title = "تغییری در یکی از رکوردهای ماژول " + notifType + " توسط " + role + " ثبت شده است.";
                else
                    title = "یک رکورد جدید توسط " + role + " در ماژول " + notifType + " ثبت شده است";

                Notification notification = new Notification()
                    {
                        UserId = user.Id,
                        CreationDate = DateTime.Now,
                        Id = Guid.NewGuid(),
                        IsActive = true,
                        IsDeleted = false,
                        IsVisited = false,
                        Title = title,
                        Url = url,
                    };

                    db.Notifications.Add(notification);
                
                db.SaveChanges();

            }
        }
    }
}