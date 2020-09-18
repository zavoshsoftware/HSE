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

namespace HSE.Controllers
{

    [Authorize(Roles = "Administrator")]
    public class UsersController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            List<User> user = db.Users.Include(h => h.Company)
                .Where(h=> h.IsDeleted == false)
                .OrderByDescending(h => h.CreationDate).ToList();

            return View(user);
        }


     

        // GET: Users/Details/5
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

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Title");
            ViewBag.CompanyId = new SelectList(db.Companies.Where(c=>c.IsDeleted==false), "Id", "Title");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            user.Code = CodeGenerator.GetUserCode(db);

            if (ModelState.IsValid)
            {

				user.IsDeleted=false;
				user.CreationDate= DateTime.Now; 
                user.Id = Guid.NewGuid();
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Title", user.RoleId);
            ViewBag.CompanyId = new SelectList(db.Companies.Where(c=>c.IsDeleted==false), "Id", "Title");
            return View(user);
        }

         

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
            ViewBag.CompanyId = new SelectList(db.Companies.Where(c=>c.IsDeleted==false), "Id", "Title",user.CompanyId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Password,Username,FullName,Code,Email,RoleId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] User user)
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
            ViewBag.CompanyId = new SelectList(db.Companies.Where(c=>c.IsDeleted==false), "Id", "Title",user.CompanyId);
            return View(user);
        }

        // GET: Users/Delete/5
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            User user = db.Users.Find(id);
			user.IsDeleted=true;
			user.DeletionDate=DateTime.Now;
 
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
