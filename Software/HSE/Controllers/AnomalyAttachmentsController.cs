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
    public class AnomalyAttachmentsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index(Guid id)
        {
            var anomalyAttachments = db.AnomalyAttachments.Include(a => a.Anomaly).Where(a=>a.AnomalyId==id&& a.IsDeleted==false).OrderByDescending(a=>a.CreationDate);
            return View(anomalyAttachments.ToList());
        }

        // GET: AnomalyAttachments/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnomalyAttachment anomalyAttachment = db.AnomalyAttachments.Find(id);
            if (anomalyAttachment == null)
            {
                return HttpNotFound();
            }
            return View(anomalyAttachment);
        }

        // GET: AnomalyAttachments/Create
        public ActionResult Create()
        {
            ViewBag.AnomalyId = new SelectList(db.Anomalies, "Id", "Code");
            return View();
        }

        // POST: AnomalyAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ImageUrl,AnomalyId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] AnomalyAttachment anomalyAttachment)
        {
            if (ModelState.IsValid)
            {
				anomalyAttachment.IsDeleted=false;
				anomalyAttachment.CreationDate= DateTime.Now; 
                anomalyAttachment.Id = Guid.NewGuid();
                db.AnomalyAttachments.Add(anomalyAttachment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AnomalyId = new SelectList(db.Anomalies, "Id", "Code", anomalyAttachment.AnomalyId);
            return View(anomalyAttachment);
        }

        // GET: AnomalyAttachments/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnomalyAttachment anomalyAttachment = db.AnomalyAttachments.Find(id);
            if (anomalyAttachment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnomalyId = new SelectList(db.Anomalies, "Id", "Code", anomalyAttachment.AnomalyId);
            return View(anomalyAttachment);
        }

        // POST: AnomalyAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ImageUrl,AnomalyId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] AnomalyAttachment anomalyAttachment)
        {
            if (ModelState.IsValid)
            {
				anomalyAttachment.IsDeleted = false;
				anomalyAttachment.LastModifiedDate = DateTime.Now;
                db.Entry(anomalyAttachment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AnomalyId = new SelectList(db.Anomalies, "Id", "Code", anomalyAttachment.AnomalyId);
            return View(anomalyAttachment);
        }

        // GET: AnomalyAttachments/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnomalyAttachment anomalyAttachment = db.AnomalyAttachments.Find(id);
            if (anomalyAttachment == null)
            {
                return HttpNotFound();
            }
            return View(anomalyAttachment);
        }

        // POST: AnomalyAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AnomalyAttachment anomalyAttachment = db.AnomalyAttachments.Find(id);
			anomalyAttachment.IsDeleted=true;
			anomalyAttachment.DeletionDate=DateTime.Now;
 
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
