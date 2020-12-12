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
    public class CovidTypesController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: CovidTypes
        public ActionResult Index()
        {
            return View(db.CovidTypes.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: CovidTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CovidType covidType = db.CovidTypes.Find(id);
            if (covidType == null)
            {
                return HttpNotFound();
            }
            return View(covidType);
        }

        // GET: CovidTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CovidTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] CovidType covidType)
        {
            if (ModelState.IsValid)
            {
				covidType.IsDeleted=false;
				covidType.CreationDate= DateTime.Now; 
                covidType.Id = Guid.NewGuid();
                db.CovidTypes.Add(covidType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(covidType);
        }

        // GET: CovidTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CovidType covidType = db.CovidTypes.Find(id);
            if (covidType == null)
            {
                return HttpNotFound();
            }
            return View(covidType);
        }

        // POST: CovidTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] CovidType covidType)
        {
            if (ModelState.IsValid)
            {
				covidType.IsDeleted = false;
				covidType.LastModifiedDate = DateTime.Now;
                db.Entry(covidType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(covidType);
        }

        // GET: CovidTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CovidType covidType = db.CovidTypes.Find(id);
            if (covidType == null)
            {
                return HttpNotFound();
            }
            return View(covidType);
        }

        // POST: CovidTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CovidType covidType = db.CovidTypes.Find(id);
			covidType.IsDeleted=true;
			covidType.DeletionDate=DateTime.Now;
 
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
