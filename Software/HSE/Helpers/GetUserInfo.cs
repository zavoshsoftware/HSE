using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 
using Models;

namespace Helpers
{
    public static class GetUserInfo
    {
        public static User GetUserFullName()
        {
            DatabaseContext db = new DatabaseContext();

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {

                var identity = (System.Security.Claims.ClaimsIdentity)HttpContext.Current.User.Identity;

                Guid id = new Guid(identity.Name);

                Models.User user = db.Users.FirstOrDefault(current => current.Id == id);

                if (user != null)
                    return user;
            }

            return new User();
        }


        public static List<User> GetCompanyUsersBySupervisor(Guid supUserId)
        {
            DatabaseContext db = new DatabaseContext();
            var companies = db.Companies.Where(c => c.SupervisorUserId == supUserId);

            List<User> users = new List<User>();

            foreach (var company in companies)
            {
                var us = db.Users.Where(c => c.CompanyId == company.Id).ToList();

                foreach (var user in us)
                {
                    users.Add(user);
                }
            }

            return users;
        }
    }
}