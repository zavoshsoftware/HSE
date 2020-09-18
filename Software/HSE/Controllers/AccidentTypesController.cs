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
    public class AccidentTypesController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: AccidentTypes
        public ActionResult Index()
        {
            return View(db.AccidentTypes.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: AccidentTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentType accidentType = db.AccidentTypes.Find(id);
            if (accidentType == null)
            {
                return HttpNotFound();
            }
            return View(accidentType);
        }

        // GET: AccidentTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccidentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccidentType accidentType)
        {
            if (ModelState.IsValid)
            {
				accidentType.IsDeleted=false;
				accidentType.CreationDate= DateTime.Now; 
                
                accidentType.Id = Guid.NewGuid();



             
                   db.AccidentTypes.Add(accidentType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(accidentType);
        }

        // GET: AccidentTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentType accidentType = db.AccidentTypes.Find(id);
            if (accidentType == null)
            {
                return HttpNotFound();
            }
            return View(accidentType);
        }

        // POST: AccidentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] AccidentType accidentType)
        {
            if (ModelState.IsValid)
            {
				accidentType.IsDeleted = false;
				accidentType.LastModifiedDate = DateTime.Now;
                db.Entry(accidentType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(accidentType);
        }

        // GET: AccidentTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentType accidentType = db.AccidentTypes.Find(id);
            if (accidentType == null)
            {
                return HttpNotFound();
            }
            return View(accidentType);
        }

        // POST: AccidentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AccidentType accidentType = db.AccidentTypes.Find(id);
			accidentType.IsDeleted=true;
			accidentType.DeletionDate=DateTime.Now;
 
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
