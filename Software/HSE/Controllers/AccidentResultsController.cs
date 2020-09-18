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
    public class AccidentResultsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: AccidentResults
        public ActionResult Index()
        {
            return View(db.AccidentResults.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: AccidentResults/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentResult accidentResult = db.AccidentResults.Find(id);
            if (accidentResult == null)
            {
                return HttpNotFound();
            }
            return View(accidentResult);
        }

        // GET: AccidentResults/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccidentResults/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccidentResult accidentResult)
        {
            if (ModelState.IsValid)
            {
				accidentResult.IsDeleted=false;
				accidentResult.CreationDate= DateTime.Now; 
                accidentResult.Id = Guid.NewGuid();


                //if (accidentResult.Description != null)
                //{
                //    string[] acc = accidentResult.Description.Split('|');

                //    foreach (string s in acc)
                //    {
                //        AccidentResult oAccidentResult = new AccidentResult()
                //        {
                //            Id = Guid.NewGuid(),
                //            CreationDate = DateTime.Now,
                //            IsActive = true,
                //            IsDeleted = false,
                //            Title = s,

                //        };

                //        db.AccidentResults.Add(oAccidentResult);
                //    }
                //}



                 db.AccidentResults.Add(accidentResult);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(accidentResult);
        }

        // GET: AccidentResults/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentResult accidentResult = db.AccidentResults.Find(id);
            if (accidentResult == null)
            {
                return HttpNotFound();
            }
            return View(accidentResult);
        }

        // POST: AccidentResults/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] AccidentResult accidentResult)
        {
            if (ModelState.IsValid)
            {
				accidentResult.IsDeleted = false;
				accidentResult.LastModifiedDate = DateTime.Now;
                db.Entry(accidentResult).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(accidentResult);
        }

        // GET: AccidentResults/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentResult accidentResult = db.AccidentResults.Find(id);
            if (accidentResult == null)
            {
                return HttpNotFound();
            }
            return View(accidentResult);
        }

        // POST: AccidentResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AccidentResult accidentResult = db.AccidentResults.Find(id);
			accidentResult.IsDeleted=true;
			accidentResult.DeletionDate=DateTime.Now;
 
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
