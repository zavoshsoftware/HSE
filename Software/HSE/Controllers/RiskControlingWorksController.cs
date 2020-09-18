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
    public class RiskControlingWorksController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: RiskControlingWorks
        public ActionResult Index(Guid id)
        {
            var riskControlingWorks = db.RiskControlingWorks.Include(r => r.Risk).Where(r=>r.RiskId==id&& r.IsDeleted==false).OrderByDescending(r=>r.CreationDate);

            Risk risk = db.Risks.Find(id);
            if (risk != null)
                ViewBag.Title = "لیست اقدامات کنترلی مربوط به ریسک " + risk.Title;

            return View(riskControlingWorks.ToList());
        }
        public ActionResult Create(Guid id)
        {
            ViewBag.RiskId = id;
            return View();
        }

        // POST: RiskControlingWorks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RiskControlingWork riskControlingWork, Guid id)
        {
            if (ModelState.IsValid)
            {
                riskControlingWork.RiskId = id;
				riskControlingWork.IsDeleted=false;
				riskControlingWork.CreationDate= DateTime.Now; 
                riskControlingWork.Id = Guid.NewGuid();
                db.RiskControlingWorks.Add(riskControlingWork);
                db.SaveChanges();
                return RedirectToAction("Index",new{id=id});
            }

            ViewBag.RiskId = id;
            return View(riskControlingWork);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RiskControlingWork riskControlingWork = db.RiskControlingWorks.Find(id);
            if (riskControlingWork == null)
            {
                return HttpNotFound();
            }
            ViewBag.RiskId = riskControlingWork.RiskId;
            return View(riskControlingWork);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Code,RiskId,OldId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] RiskControlingWork riskControlingWork)
        {
            if (ModelState.IsValid)
            {
				riskControlingWork.IsDeleted = false;
				riskControlingWork.LastModifiedDate = DateTime.Now;
                db.Entry(riskControlingWork).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new {id=riskControlingWork.RiskId});
            }
            ViewBag.RiskId = riskControlingWork.RiskId;
            return View(riskControlingWork);
        }

        // GET: RiskControlingWorks/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RiskControlingWork riskControlingWork = db.RiskControlingWorks.Find(id);
            if (riskControlingWork == null)
            {
                return HttpNotFound();
            }
            ViewBag.RiskId = riskControlingWork.RiskId;

            return View(riskControlingWork);
        }

        // POST: RiskControlingWorks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            RiskControlingWork riskControlingWork = db.RiskControlingWorks.Find(id);
			riskControlingWork.IsDeleted=true;
			riskControlingWork.DeletionDate=DateTime.Now;
 
            db.SaveChanges();

            return RedirectToAction("Index", new { id = riskControlingWork.RiskId });
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
