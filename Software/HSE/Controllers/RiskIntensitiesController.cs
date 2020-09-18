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
    public class RiskIntensitiesController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: RiskIntensities
        public ActionResult Index()
        {
            return View(db.RiskIntensities.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: RiskIntensities/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RiskIntensity riskIntensity = db.RiskIntensities.Find(id);
            if (riskIntensity == null)
            {
                return HttpNotFound();
            }
            return View(riskIntensity);
        }

        // GET: RiskIntensities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RiskIntensities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Level,Summery,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] RiskIntensity riskIntensity)
        {
            if (ModelState.IsValid)
            {
				riskIntensity.IsDeleted=false;
				riskIntensity.CreationDate= DateTime.Now; 
                riskIntensity.Id = Guid.NewGuid();
                db.RiskIntensities.Add(riskIntensity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(riskIntensity);
        }

        // GET: RiskIntensities/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RiskIntensity riskIntensity = db.RiskIntensities.Find(id);
            if (riskIntensity == null)
            {
                return HttpNotFound();
            }
            return View(riskIntensity);
        }

        // POST: RiskIntensities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Level,Summery,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] RiskIntensity riskIntensity)
        {
            if (ModelState.IsValid)
            {
				riskIntensity.IsDeleted = false;
				riskIntensity.LastModifiedDate = DateTime.Now;
                db.Entry(riskIntensity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(riskIntensity);
        }

        // GET: RiskIntensities/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RiskIntensity riskIntensity = db.RiskIntensities.Find(id);
            if (riskIntensity == null)
            {
                return HttpNotFound();
            }
            return View(riskIntensity);
        }

        // POST: RiskIntensities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            RiskIntensity riskIntensity = db.RiskIntensities.Find(id);
			riskIntensity.IsDeleted=true;
			riskIntensity.DeletionDate=DateTime.Now;
 
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
