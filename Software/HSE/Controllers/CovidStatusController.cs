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
    public class CovidStatusController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: CovidStatus
        public ActionResult Index()
        {
            return View(db.CovidStatus.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: CovidStatus/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CovidStatus covidStatus = db.CovidStatus.Find(id);
            if (covidStatus == null)
            {
                return HttpNotFound();
            }
            return View(covidStatus);
        }

        // GET: CovidStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CovidStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] CovidStatus covidStatus)
        {
            if (ModelState.IsValid)
            {
				covidStatus.IsDeleted=false;
				covidStatus.CreationDate= DateTime.Now; 
                covidStatus.Id = Guid.NewGuid();
                db.CovidStatus.Add(covidStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(covidStatus);
        }

        // GET: CovidStatus/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CovidStatus covidStatus = db.CovidStatus.Find(id);
            if (covidStatus == null)
            {
                return HttpNotFound();
            }
            return View(covidStatus);
        }

        // POST: CovidStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] CovidStatus covidStatus)
        {
            if (ModelState.IsValid)
            {
				covidStatus.IsDeleted = false;
				covidStatus.LastModifiedDate = DateTime.Now;
                db.Entry(covidStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(covidStatus);
        }

        // GET: CovidStatus/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CovidStatus covidStatus = db.CovidStatus.Find(id);
            if (covidStatus == null)
            {
                return HttpNotFound();
            }
            return View(covidStatus);
        }

        // POST: CovidStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CovidStatus covidStatus = db.CovidStatus.Find(id);
			covidStatus.IsDeleted=true;
			covidStatus.DeletionDate=DateTime.Now;
 
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
