using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Helpers
{
    public static class CodeGenerator
    {
        public static int GetUserCode(DatabaseContext db)
        {
            Guid adminRoleId = new Guid("f53d469b-4172-42a9-8355-20032367c627");
            User user = db.Users.Where(current => current.RoleId != adminRoleId).OrderByDescending(current => current.Code).FirstOrDefault();
            if (user != null)
            {
                return user.Code + 1;
            }
            else
            {
                return 1000;
            }
        }
    }
}