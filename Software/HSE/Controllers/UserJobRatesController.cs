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
    public class UserJobRatesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            return View(db.UserJobRates.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: UserJobRates/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserJobRate userJobRate = db.UserJobRates.Find(id);
            if (userJobRate == null)
            {
                return HttpNotFound();
            }
            return View(userJobRate);
        }

        // GET: UserJobRates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserJobRates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] UserJobRate userJobRate)
        {
            if (ModelState.IsValid)
            {
				userJobRate.IsDeleted=false;
				userJobRate.CreationDate= DateTime.Now; 
                userJobRate.Id = Guid.NewGuid();
                db.UserJobRates.Add(userJobRate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userJobRate);
        }

        // GET: UserJobRates/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserJobRate userJobRate = db.UserJobRates.Find(id);
            if (userJobRate == null)
            {
                return HttpNotFound();
            }
            return View(userJobRate);
        }

        // POST: UserJobRates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] UserJobRate userJobRate)
        {
            if (ModelState.IsValid)
            {
				userJobRate.IsDeleted = false;
				userJobRate.LastModifiedDate = DateTime.Now;
                db.Entry(userJobRate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userJobRate);
        }

        // GET: UserJobRates/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserJobRate userJobRate = db.UserJobRates.Find(id);
            if (userJobRate == null)
            {
                return HttpNotFound();
            }
            return View(userJobRate);
        }

        // POST: UserJobRates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UserJobRate userJobRate = db.UserJobRates.Find(id);
			userJobRate.IsDeleted=true;
			userJobRate.DeletionDate=DateTime.Now;
 
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
