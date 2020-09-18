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
    public class AccidentReasonActionsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: AccidentReasonActions
        public ActionResult Index()
        {
            return View(db.AccidentReasonActions.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: AccidentReasonActions/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentReasonAction accidentReasonAction = db.AccidentReasonActions.Find(id);
            if (accidentReasonAction == null)
            {
                return HttpNotFound();
            }
            return View(accidentReasonAction);
        }

        // GET: AccidentReasonActions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccidentReasonActions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccidentReasonAction accidentReasonAction)
        {
            if (ModelState.IsValid)
            {
				accidentReasonAction.IsDeleted=false;
				accidentReasonAction.CreationDate= DateTime.Now; 
                accidentReasonAction.Id = Guid.NewGuid();

          
              db.AccidentReasonActions.Add(accidentReasonAction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(accidentReasonAction);
        }

        // GET: AccidentReasonActions/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentReasonAction accidentReasonAction = db.AccidentReasonActions.Find(id);
            if (accidentReasonAction == null)
            {
                return HttpNotFound();
            }
            return View(accidentReasonAction);
        }

        // POST: AccidentReasonActions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] AccidentReasonAction accidentReasonAction)
        {
            if (ModelState.IsValid)
            {
				accidentReasonAction.IsDeleted = false;
				accidentReasonAction.LastModifiedDate = DateTime.Now;
                db.Entry(accidentReasonAction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(accidentReasonAction);
        }

        // GET: AccidentReasonActions/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentReasonAction accidentReasonAction = db.AccidentReasonActions.Find(id);
            if (accidentReasonAction == null)
            {
                return HttpNotFound();
            }
            return View(accidentReasonAction);
        }

        // POST: AccidentReasonActions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AccidentReasonAction accidentReasonAction = db.AccidentReasonActions.Find(id);
			accidentReasonAction.IsDeleted=true;
			accidentReasonAction.DeletionDate=DateTime.Now;
 
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
