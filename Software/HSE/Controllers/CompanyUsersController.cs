using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;

namespace HSE.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CompanyUsersController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index(Guid id)
        {
            var companyUsers = db.CompanyUsers.Include(c => c.Company)
                .Where(c => c.IsDeleted == false && c.CompanyId == id).OrderByDescending(c => c.CreationDate)
                .Include(c => c.User);

            Company company = db.Companies.Find(id);

            if (company != null)
                ViewBag.CompanyTitle = company.Title;

            return View(companyUsers.ToList());
        }

        public ActionResult Create(Guid id)
        {
            ViewBag.UserId = new SelectList(db.Users.Where(current => current.Role.Name == "company" && current.IsDeleted == false), "Id", "FullName");
            ViewBag.CompanyId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompanyUser companyUser, Guid id)
        {
            if (ModelState.IsValid)
            {
                companyUser.CompanyId = id;
                companyUser.IsDeleted = false;
                companyUser.CreationDate = DateTime.Now;
                companyUser.Id = Guid.NewGuid();
                db.CompanyUsers.Add(companyUser);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.CompanyId = id;
            ViewBag.UserId = new SelectList(db.Users.Where(current => current.Role.Name == "company" && current.IsDeleted == false), "Id", "FullName", companyUser.UserId);
            return View(companyUser);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyUser companyUser = db.CompanyUsers.Find(id);
            if (companyUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = companyUser.CompanyId;
            ViewBag.UserId = new SelectList(db.Users.Where(current => current.Role.Name == "company" && current.IsDeleted == false), "Id", "FullName", companyUser.UserId);
            return View(companyUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompanyUser companyUser)
        {
            if (ModelState.IsValid)
            {
                companyUser.IsDeleted = false;
                companyUser.LastModifiedDate = DateTime.Now;
                db.Entry(companyUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = companyUser.CompanyId });

            }
            ViewBag.CompanyId = companyUser.CompanyId;
            ViewBag.UserId = new SelectList(db.Users.Where(current => current.Role.Name == "company" && current.IsDeleted == false), "Id", "FullName", companyUser.UserId);
            return View(companyUser);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyUser companyUser = db.CompanyUsers.Find(id);
            if (companyUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = companyUser.CompanyId;

            return View(companyUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CompanyUser companyUser = db.CompanyUsers.Find(id);
            companyUser.IsDeleted = true;
            companyUser.DeletionDate = DateTime.Now;

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
    }
}
