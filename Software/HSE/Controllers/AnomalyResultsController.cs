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
    public class AnomalyResultsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: AnomalyResults
        public ActionResult Index()
        {
            return View(db.AnomalyResults.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: AnomalyResults/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnomalyResult anomalyResult = db.AnomalyResults.Find(id);
            if (anomalyResult == null)
            {
                return HttpNotFound();
            }
            return View(anomalyResult);
        }

        // GET: AnomalyResults/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AnomalyResults/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] AnomalyResult anomalyResult)
        {
            if (ModelState.IsValid)
            {
				anomalyResult.IsDeleted=false;
				anomalyResult.CreationDate= DateTime.Now; 
                anomalyResult.Id = Guid.NewGuid();
                db.AnomalyResults.Add(anomalyResult);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(anomalyResult);
        }

        // GET: AnomalyResults/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnomalyResult anomalyResult = db.AnomalyResults.Find(id);
            if (anomalyResult == null)
            {
                return HttpNotFound();
            }
            return View(anomalyResult);
        }

        // POST: AnomalyResults/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] AnomalyResult anomalyResult)
        {
            if (ModelState.IsValid)
            {
				anomalyResult.IsDeleted = false;
				anomalyResult.LastModifiedDate = DateTime.Now;
                db.Entry(anomalyResult).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(anomalyResult);
        }

        // GET: AnomalyResults/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnomalyResult anomalyResult = db.AnomalyResults.Find(id);
            if (anomalyResult == null)
            {
                return HttpNotFound();
            }
            return View(anomalyResult);
        }

        // POST: AnomalyResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AnomalyResult anomalyResult = db.AnomalyResults.Find(id);
			anomalyResult.IsDeleted=true;
			anomalyResult.DeletionDate=DateTime.Now;
 
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
