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
    public class RiskProbabilitiesController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: RiskProbabilities
        public ActionResult Index()
        {
            return View(db.RiskProbabilities.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: RiskProbabilities/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RiskProbability riskProbability = db.RiskProbabilities.Find(id);
            if (riskProbability == null)
            {
                return HttpNotFound();
            }
            return View(riskProbability);
        }

        // GET: RiskProbabilities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RiskProbabilities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Level,Summery,Summery2,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] RiskProbability riskProbability)
        {
            if (ModelState.IsValid)
            {
				riskProbability.IsDeleted=false;
				riskProbability.CreationDate= DateTime.Now; 
                riskProbability.Id = Guid.NewGuid();
                db.RiskProbabilities.Add(riskProbability);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(riskProbability);
        }

        // GET: RiskProbabilities/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RiskProbability riskProbability = db.RiskProbabilities.Find(id);
            if (riskProbability == null)
            {
                return HttpNotFound();
            }
            return View(riskProbability);
        }

        // POST: RiskProbabilities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Level,Summery,Summery2,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] RiskProbability riskProbability)
        {
            if (ModelState.IsValid)
            {
				riskProbability.IsDeleted = false;
				riskProbability.LastModifiedDate = DateTime.Now;
                db.Entry(riskProbability).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(riskProbability);
        }

        // GET: RiskProbabilities/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RiskProbability riskProbability = db.RiskProbabilities.Find(id);
            if (riskProbability == null)
            {
                return HttpNotFound();
            }
            return View(riskProbability);
        }

        // POST: RiskProbabilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            RiskProbability riskProbability = db.RiskProbabilities.Find(id);
			riskProbability.IsDeleted=true;
			riskProbability.DeletionDate=DateTime.Now;
 
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
