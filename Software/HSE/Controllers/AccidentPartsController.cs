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
    public class AccidentPartsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: AccidentParts
        public ActionResult Index()
        {
            return View(db.AccidentParts.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: AccidentParts/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentPart accidentPart = db.AccidentParts.Find(id);
            if (accidentPart == null)
            {
                return HttpNotFound();
            }
            return View(accidentPart);
        }

        // GET: AccidentParts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccidentParts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccidentPart accidentPart)
        {
            if (ModelState.IsValid)
            {
				accidentPart.IsDeleted=false;
				accidentPart.CreationDate= DateTime.Now; 
                accidentPart.Id = Guid.NewGuid();

        
                db.AccidentParts.Add(accidentPart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(accidentPart);
        }

        // GET: AccidentParts/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentPart accidentPart = db.AccidentParts.Find(id);
            if (accidentPart == null)
            {
                return HttpNotFound();
            }
            return View(accidentPart);
        }

        // POST: AccidentParts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] AccidentPart accidentPart)
        {
            if (ModelState.IsValid)
            {
				accidentPart.IsDeleted = false;
				accidentPart.LastModifiedDate = DateTime.Now;
                db.Entry(accidentPart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(accidentPart);
        }

        // GET: AccidentParts/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentPart accidentPart = db.AccidentParts.Find(id);
            if (accidentPart == null)
            {
                return HttpNotFound();
            }
            return View(accidentPart);
        }

        // POST: AccidentParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AccidentPart accidentPart = db.AccidentParts.Find(id);
			accidentPart.IsDeleted=true;
			accidentPart.DeletionDate=DateTime.Now;
 
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
