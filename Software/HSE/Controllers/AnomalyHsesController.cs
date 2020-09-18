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
    public class AnomalyHsesController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: AnomalyHses
        public ActionResult Index()
        {
            return View(db.AnomalyHses.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: AnomalyHses/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnomalyHse anomalyHse = db.AnomalyHses.Find(id);
            if (anomalyHse == null)
            {
                return HttpNotFound();
            }
            return View(anomalyHse);
        }

        // GET: AnomalyHses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AnomalyHses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] AnomalyHse anomalyHse)
        {
            if (ModelState.IsValid)
            {
				anomalyHse.IsDeleted=false;
				anomalyHse.CreationDate= DateTime.Now; 
                anomalyHse.Id = Guid.NewGuid();
                db.AnomalyHses.Add(anomalyHse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(anomalyHse);
        }

        // GET: AnomalyHses/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnomalyHse anomalyHse = db.AnomalyHses.Find(id);
            if (anomalyHse == null)
            {
                return HttpNotFound();
            }
            return View(anomalyHse);
        }

        // POST: AnomalyHses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] AnomalyHse anomalyHse)
        {
            if (ModelState.IsValid)
            {
				anomalyHse.IsDeleted = false;
				anomalyHse.LastModifiedDate = DateTime.Now;
                db.Entry(anomalyHse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(anomalyHse);
        }

        // GET: AnomalyHses/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnomalyHse anomalyHse = db.AnomalyHses.Find(id);
            if (anomalyHse == null)
            {
                return HttpNotFound();
            }
            return View(anomalyHse);
        }

        // POST: AnomalyHses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AnomalyHse anomalyHse = db.AnomalyHses.Find(id);
			anomalyHse.IsDeleted=true;
			anomalyHse.DeletionDate=DateTime.Now;
 
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
