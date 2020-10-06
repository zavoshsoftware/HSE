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
    public class SafetyFileTypesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: SafetyFileTypes
        public ActionResult Index()
        {
            return View(db.SafetyFileTypes.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: SafetyFileTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SafetyFileType safetyFileType = db.SafetyFileTypes.Find(id);
            if (safetyFileType == null)
            {
                return HttpNotFound();
            }
            return View(safetyFileType);
        }

        // GET: SafetyFileTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SafetyFileTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Code,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] SafetyFileType safetyFileType)
        {
            if (ModelState.IsValid)
            {
				safetyFileType.IsDeleted=false;
				safetyFileType.CreationDate= DateTime.Now; 
                safetyFileType.Id = Guid.NewGuid();
                db.SafetyFileTypes.Add(safetyFileType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(safetyFileType);
        }

        // GET: SafetyFileTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SafetyFileType safetyFileType = db.SafetyFileTypes.Find(id);
            if (safetyFileType == null)
            {
                return HttpNotFound();
            }
            return View(safetyFileType);
        }

        // POST: SafetyFileTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Code,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] SafetyFileType safetyFileType)
        {
            if (ModelState.IsValid)
            {
				safetyFileType.IsDeleted = false;
				safetyFileType.LastModifiedDate = DateTime.Now;
                db.Entry(safetyFileType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(safetyFileType);
        }

        // GET: SafetyFileTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SafetyFileType safetyFileType = db.SafetyFileTypes.Find(id);
            if (safetyFileType == null)
            {
                return HttpNotFound();
            }
            return View(safetyFileType);
        }

        // POST: SafetyFileTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            SafetyFileType safetyFileType = db.SafetyFileTypes.Find(id);
			safetyFileType.IsDeleted=true;
			safetyFileType.DeletionDate=DateTime.Now;
 
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
