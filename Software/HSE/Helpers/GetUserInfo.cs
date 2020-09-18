using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 
using Models;

namespace Helpers
{
    public static class GetUserInfo
    {
        public static string GetUserFullName()
        {
            DatabaseContext db = new DatabaseContext();

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {

                var identity = (System.Security.Claims.ClaimsIdentity)HttpContext.Current.User.Identity;

                Guid id = new Guid(identity.Name);

                Models.User user = db.Users.FirstOrDefault(current => current.Id == id);

                if (user != null)
                    return user.FullName;
            }

            return string.Empty;
        }
    }
}