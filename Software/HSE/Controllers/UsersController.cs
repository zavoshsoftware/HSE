using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Models;
using ViewModels;

namespace HSE.Controllers
{

    public class UsersController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            List<User> user = db.Users.Include(h => h.Company)
                .Where(h => h.IsDeleted == false)
                .OrderByDescending(h => h.CreationDate).ToList();

            return View(user);
        }




        [Authorize(Roles = "Administrator")]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Title");
            ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.IsDeleted == false), "Id", "Title");
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            user.Code = CodeGenerator.GetUserCode(db);

            if (ModelState.IsValid)
            {

                user.IsDeleted = false;
                user.CreationDate = DateTime.Now;
                user.Id = Guid.NewGuid();
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Title", user.RoleId);
            ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.IsDeleted == false), "Id", "Title");
            return View(user);
        }


        [Authorize()]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize()]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel changePassword)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string id = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

            Guid userId = new Guid(id);

            User user = db.Users.Find(userId);

            if (ModelState.IsValid)
            {
                if (user.Password != changePassword.OldPassword)
                {
                    ModelState.AddModelError("invalidOldPass", "کلمه عبور قدیمی صحیح نمی باشد.");
                    return View(changePassword);
                }
                if (changePassword.NewPassword != changePassword.RepeatNewPassword)
                {
                    ModelState.AddModelError("invalidOldPass", "تکرار کلمه عبور را به درستی وارد نمایید.");
                    return View(changePassword);
                }

                user.Password = changePassword.NewPassword;
                db.SaveChanges();
                ViewBag.success = "کلمه عبور شما با موفقیت تغییر یافت.";
                return View(changePassword);


            }
            return View(changePassword);


        }



        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Title", user.RoleId);
            ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.IsDeleted == false), "Id", "Title", user.CompanyId);
            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( User user)
        {
            if (ModelState.IsValid)
            {
                user.IsDeleted = false;
                user.LastModifiedDate = DateTime.Now;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Title", user.RoleId);
            ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.IsDeleted == false), "Id", "Title", user.CompanyId);
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            User user = db.Users.Find(id);
            user.IsDeleted = true;
            user.DeletionDate = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        //public string UpdateCompanyUsers()
        //{
        //    List<CompanyUser> companyUsers = db.CompanyUsers.ToList();

        //    foreach (CompanyUser companyUser in companyUsers)
        //    {
        //        User user = db.Users.Find(companyUser.UserId);

        //        if (user != null)
        //        {
        //            user.CompanyId = companyUser.CompanyId;
        //            user.LastModifiedDate = DateTime.Now;
        //        }
        //    }

        //    db.SaveChanges();
        //    return String.Empty;
        //}
    }
}
