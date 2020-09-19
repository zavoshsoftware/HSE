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
    public class CrisisTypesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: CrisisTypes
        public ActionResult Index()
        {
            return View(db.CrisisTypes.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: CrisisTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrisisType crisisType = db.CrisisTypes.Find(id);
            if (crisisType == null)
            {
                return HttpNotFound();
            }
            return View(crisisType);
        }

        // GET: CrisisTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CrisisTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] CrisisType crisisType)
        {
            if (ModelState.IsValid)
            {
				crisisType.IsDeleted=false;
				crisisType.CreationDate= DateTime.Now; 
                crisisType.Id = Guid.NewGuid();
                db.CrisisTypes.Add(crisisType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(crisisType);
        }

        // GET: CrisisTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrisisType crisisType = db.CrisisTypes.Find(id);
            if (crisisType == null)
            {
                return HttpNotFound();
            }
            return View(crisisType);
        }

        // POST: CrisisTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] CrisisType crisisType)
        {
            if (ModelState.IsValid)
            {
				crisisType.IsDeleted = false;
				crisisType.LastModifiedDate = DateTime.Now;
                db.Entry(crisisType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(crisisType);
        }

        // GET: CrisisTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrisisType crisisType = db.CrisisTypes.Find(id);
            if (crisisType == null)
            {
                return HttpNotFound();
            }
            return View(crisisType);
        }

        // POST: CrisisTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CrisisType crisisType = db.CrisisTypes.Find(id);
			crisisType.IsDeleted=true;
			crisisType.DeletionDate=DateTime.Now;
 
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
