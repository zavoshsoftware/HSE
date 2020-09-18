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
    public class AccidentReasonConditionsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: AccidentReasonConditions
        public ActionResult Index()
        {
            return View(db.AccidentReasonConditions.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: AccidentReasonConditions/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentReasonCondition accidentReasonCondition = db.AccidentReasonConditions.Find(id);
            if (accidentReasonCondition == null)
            {
                return HttpNotFound();
            }
            return View(accidentReasonCondition);
        }

        // GET: AccidentReasonConditions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccidentReasonConditions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccidentReasonCondition accidentReasonCondition)
        {
            if (ModelState.IsValid)
            {
				accidentReasonCondition.IsDeleted=false;
				accidentReasonCondition.CreationDate= DateTime.Now; 
                accidentReasonCondition.Id = Guid.NewGuid();


                db.AccidentReasonConditions.Add(accidentReasonCondition);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(accidentReasonCondition);
        }

        // GET: AccidentReasonConditions/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentReasonCondition accidentReasonCondition = db.AccidentReasonConditions.Find(id);
            if (accidentReasonCondition == null)
            {
                return HttpNotFound();
            }
            return View(accidentReasonCondition);
        }

        // POST: AccidentReasonConditions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] AccidentReasonCondition accidentReasonCondition)
        {
            if (ModelState.IsValid)
            {
				accidentReasonCondition.IsDeleted = false;
				accidentReasonCondition.LastModifiedDate = DateTime.Now;
                db.Entry(accidentReasonCondition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(accidentReasonCondition);
        }

        // GET: AccidentReasonConditions/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentReasonCondition accidentReasonCondition = db.AccidentReasonConditions.Find(id);
            if (accidentReasonCondition == null)
            {
                return HttpNotFound();
            }
            return View(accidentReasonCondition);
        }

        // POST: AccidentReasonConditions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AccidentReasonCondition accidentReasonCondition = db.AccidentReasonConditions.Find(id);
			accidentReasonCondition.IsDeleted=true;
			accidentReasonCondition.DeletionDate=DateTime.Now;
 
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
