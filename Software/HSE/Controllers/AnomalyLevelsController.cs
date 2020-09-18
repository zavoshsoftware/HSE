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
    public class AnomalyLevelsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: AnomalyLevels
        public ActionResult Index()
        {
            return View(db.AnomalyLevels.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: AnomalyLevels/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnomalyLevel anomalyLevel = db.AnomalyLevels.Find(id);
            if (anomalyLevel == null)
            {
                return HttpNotFound();
            }
            return View(anomalyLevel);
        }

        // GET: AnomalyLevels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AnomalyLevels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] AnomalyLevel anomalyLevel)
        {
            if (ModelState.IsValid)
            {
				anomalyLevel.IsDeleted=false;
				anomalyLevel.CreationDate= DateTime.Now; 
                anomalyLevel.Id = Guid.NewGuid();
                db.AnomalyLevels.Add(anomalyLevel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(anomalyLevel);
        }

        // GET: AnomalyLevels/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnomalyLevel anomalyLevel = db.AnomalyLevels.Find(id);
            if (anomalyLevel == null)
            {
                return HttpNotFound();
            }
            return View(anomalyLevel);
        }

        // POST: AnomalyLevels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] AnomalyLevel anomalyLevel)
        {
            if (ModelState.IsValid)
            {
				anomalyLevel.IsDeleted = false;
				anomalyLevel.LastModifiedDate = DateTime.Now;
                db.Entry(anomalyLevel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(anomalyLevel);
        }

        // GET: AnomalyLevels/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnomalyLevel anomalyLevel = db.AnomalyLevels.Find(id);
            if (anomalyLevel == null)
            {
                return HttpNotFound();
            }
            return View(anomalyLevel);
        }

        // POST: AnomalyLevels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AnomalyLevel anomalyLevel = db.AnomalyLevels.Find(id);
			anomalyLevel.IsDeleted=true;
			anomalyLevel.DeletionDate=DateTime.Now;
 
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
