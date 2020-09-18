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
    public class AccidentEmployeeTypesController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: AccidentEmployeeTypes
        public ActionResult Index()
        {
            return View(db.AccidentEmployeeTypes.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: AccidentEmployeeTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentEmployeeType accidentEmployeeType = db.AccidentEmployeeTypes.Find(id);
            if (accidentEmployeeType == null)
            {
                return HttpNotFound();
            }
            return View(accidentEmployeeType);
        }

        // GET: AccidentEmployeeTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccidentEmployeeTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] AccidentEmployeeType accidentEmployeeType)
        {
            if (ModelState.IsValid)
            {
				accidentEmployeeType.IsDeleted=false;
				accidentEmployeeType.CreationDate= DateTime.Now; 
                accidentEmployeeType.Id = Guid.NewGuid();
                db.AccidentEmployeeTypes.Add(accidentEmployeeType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(accidentEmployeeType);
        }

        // GET: AccidentEmployeeTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentEmployeeType accidentEmployeeType = db.AccidentEmployeeTypes.Find(id);
            if (accidentEmployeeType == null)
            {
                return HttpNotFound();
            }
            return View(accidentEmployeeType);
        }

        // POST: AccidentEmployeeTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] AccidentEmployeeType accidentEmployeeType)
        {
            if (ModelState.IsValid)
            {
				accidentEmployeeType.IsDeleted = false;
				accidentEmployeeType.LastModifiedDate = DateTime.Now;
                db.Entry(accidentEmployeeType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(accidentEmployeeType);
        }

        // GET: AccidentEmployeeTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentEmployeeType accidentEmployeeType = db.AccidentEmployeeTypes.Find(id);
            if (accidentEmployeeType == null)
            {
                return HttpNotFound();
            }
            return View(accidentEmployeeType);
        }

        // POST: AccidentEmployeeTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AccidentEmployeeType accidentEmployeeType = db.AccidentEmployeeTypes.Find(id);
			accidentEmployeeType.IsDeleted=true;
			accidentEmployeeType.DeletionDate=DateTime.Now;
 
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
