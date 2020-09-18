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
    public class RiskMatrisController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: RiskMatris
        public ActionResult Index()
        {
            var riskMatris = db.RiskMatris.Include(r => r.RiskIntensity).Where(r=>r.IsDeleted==false).OrderByDescending(r=>r.CreationDate).Include(r => r.RiskProbability).Where(r=>r.IsDeleted==false).OrderByDescending(r=>r.CreationDate);
            return View(riskMatris.ToList());
        }

        // GET: RiskMatris/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RiskMatris riskMatris = db.RiskMatris.Find(id);
            if (riskMatris == null)
            {
                return HttpNotFound();
            }
            return View(riskMatris);
        }

        // GET: RiskMatris/Create
        public ActionResult Create()
        {
            ViewBag.RiskIntensityId = new SelectList(db.RiskIntensities, "Id", "Level");
            ViewBag.RiskProbabilityId = new SelectList(db.RiskProbabilities, "Id", "Level");
            return View();
        }

        // POST: RiskMatris/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RiskNumber,RiskProbabilityId,RiskIntensityId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] RiskMatris riskMatris)
        {
            if (ModelState.IsValid)
            {
				riskMatris.IsDeleted=false;
				riskMatris.CreationDate= DateTime.Now; 
                riskMatris.Id = Guid.NewGuid();
                db.RiskMatris.Add(riskMatris);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RiskIntensityId = new SelectList(db.RiskIntensities, "Id", "Level", riskMatris.RiskIntensityId);
            ViewBag.RiskProbabilityId = new SelectList(db.RiskProbabilities, "Id", "Level", riskMatris.RiskProbabilityId);
            return View(riskMatris);
        }

        // GET: RiskMatris/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RiskMatris riskMatris = db.RiskMatris.Find(id);
            if (riskMatris == null)
            {
                return HttpNotFound();
            }
            ViewBag.RiskIntensityId = new SelectList(db.RiskIntensities, "Id", "Level", riskMatris.RiskIntensityId);
            ViewBag.RiskProbabilityId = new SelectList(db.RiskProbabilities, "Id", "Level", riskMatris.RiskProbabilityId);
            return View(riskMatris);
        }

        // POST: RiskMatris/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RiskNumber,RiskProbabilityId,RiskIntensityId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] RiskMatris riskMatris)
        {
            if (ModelState.IsValid)
            {
				riskMatris.IsDeleted = false;
				riskMatris.LastModifiedDate = DateTime.Now;
                db.Entry(riskMatris).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RiskIntensityId = new SelectList(db.RiskIntensities, "Id", "Level", riskMatris.RiskIntensityId);
            ViewBag.RiskProbabilityId = new SelectList(db.RiskProbabilities, "Id", "Level", riskMatris.RiskProbabilityId);
            return View(riskMatris);
        }

        // GET: RiskMatris/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RiskMatris riskMatris = db.RiskMatris.Find(id);
            if (riskMatris == null)
            {
                return HttpNotFound();
            }
            return View(riskMatris);
        }

        // POST: RiskMatris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            RiskMatris riskMatris = db.RiskMatris.Find(id);
			riskMatris.IsDeleted=true;
			riskMatris.DeletionDate=DateTime.Now;
 
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
