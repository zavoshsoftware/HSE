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
    public class SafetyTypesController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: SafetyTypes
        public ActionResult Index()
        {
            return View(db.SafetyTypes.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: SafetyTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SafetyType safetyType = db.SafetyTypes.Find(id);
            if (safetyType == null)
            {
                return HttpNotFound();
            }
            return View(safetyType);
        }

        // GET: SafetyTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SafetyTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] SafetyType safetyType)
        {
            if (ModelState.IsValid)
            {
				safetyType.IsDeleted=false;
				safetyType.CreationDate= DateTime.Now; 
                safetyType.Id = Guid.NewGuid();
                db.SafetyTypes.Add(safetyType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(safetyType);
        }

        // GET: SafetyTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SafetyType safetyType = db.SafetyTypes.Find(id);
            if (safetyType == null)
            {
                return HttpNotFound();
            }
            return View(safetyType);
        }

        // POST: SafetyTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] SafetyType safetyType)
        {
            if (ModelState.IsValid)
            {
				safetyType.IsDeleted = false;
				safetyType.LastModifiedDate = DateTime.Now;
                db.Entry(safetyType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(safetyType);
        }

        // GET: SafetyTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SafetyType safetyType = db.SafetyTypes.Find(id);
            if (safetyType == null)
            {
                return HttpNotFound();
            }
            return View(safetyType);
        }

        // POST: SafetyTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            SafetyType safetyType = db.SafetyTypes.Find(id);
			safetyType.IsDeleted=true;
			safetyType.DeletionDate=DateTime.Now;
 
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
