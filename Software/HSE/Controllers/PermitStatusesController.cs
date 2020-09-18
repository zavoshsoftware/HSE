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
    public class PermitStatusesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: PermitStatus
        public ActionResult Index()
        {
            return View(db.PermitStatuses.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: PermitStatus/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermitStatus permitStatus = db.PermitStatuses.Find(id);
            if (permitStatus == null)
            {
                return HttpNotFound();
            }
            return View(permitStatus);
        }

        // GET: PermitStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PermitStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Code,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] PermitStatus permitStatus)
        {
            if (ModelState.IsValid)
            {
				permitStatus.IsDeleted=false;
				permitStatus.CreationDate= DateTime.Now; 
                permitStatus.Id = Guid.NewGuid();
                db.PermitStatuses.Add(permitStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(permitStatus);
        }

        // GET: PermitStatus/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermitStatus permitStatus = db.PermitStatuses.Find(id);
            if (permitStatus == null)
            {
                return HttpNotFound();
            }
            return View(permitStatus);
        }

        // POST: PermitStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Code,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] PermitStatus permitStatus)
        {
            if (ModelState.IsValid)
            {
				permitStatus.IsDeleted = false;
				permitStatus.LastModifiedDate = DateTime.Now;
                db.Entry(permitStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(permitStatus);
        }

        // GET: PermitStatus/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermitStatus permitStatus = db.PermitStatuses.Find(id);
            if (permitStatus == null)
            {
                return HttpNotFound();
            }
            return View(permitStatus);
        }

        // POST: PermitStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PermitStatus permitStatus = db.PermitStatuses.Find(id);
			permitStatus.IsDeleted=true;
			permitStatus.DeletionDate=DateTime.Now;
 
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
