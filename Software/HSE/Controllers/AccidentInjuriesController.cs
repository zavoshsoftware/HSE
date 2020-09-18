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
    public class AccidentInjuriesController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: AccidentInjuries
        public ActionResult Index()
        {
            return View(db.AccidentInjuries.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: AccidentInjuries/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentInjury accidentInjury = db.AccidentInjuries.Find(id);
            if (accidentInjury == null)
            {
                return HttpNotFound();
            }
            return View(accidentInjury);
        }

        // GET: AccidentInjuries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccidentInjuries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccidentInjury accidentInjury)
        {
            if (ModelState.IsValid)
            {
				accidentInjury.IsDeleted=false;
				accidentInjury.CreationDate= DateTime.Now; 
                accidentInjury.Id = Guid.NewGuid();


                db.AccidentInjuries.Add(accidentInjury);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(accidentInjury);
        }

        // GET: AccidentInjuries/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentInjury accidentInjury = db.AccidentInjuries.Find(id);
            if (accidentInjury == null)
            {
                return HttpNotFound();
            }
            return View(accidentInjury);
        }

        // POST: AccidentInjuries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] AccidentInjury accidentInjury)
        {
            if (ModelState.IsValid)
            {
				accidentInjury.IsDeleted = false;
				accidentInjury.LastModifiedDate = DateTime.Now;
                db.Entry(accidentInjury).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(accidentInjury);
        }

        // GET: AccidentInjuries/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentInjury accidentInjury = db.AccidentInjuries.Find(id);
            if (accidentInjury == null)
            {
                return HttpNotFound();
            }
            return View(accidentInjury);
        }

        // POST: AccidentInjuries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AccidentInjury accidentInjury = db.AccidentInjuries.Find(id);
			accidentInjury.IsDeleted=true;
			accidentInjury.DeletionDate=DateTime.Now;
 
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
