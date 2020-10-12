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
    public class PassiveDefenseTypesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: PassiveDefenseTypes
        public ActionResult Index()
        {
            return View(db.PassiveDefenseTypes.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: PassiveDefenseTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PassiveDefenseType passiveDefenseType = db.PassiveDefenseTypes.Find(id);
            if (passiveDefenseType == null)
            {
                return HttpNotFound();
            }
            return View(passiveDefenseType);
        }

        // GET: PassiveDefenseTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PassiveDefenseTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] PassiveDefenseType passiveDefenseType)
        {
            if (ModelState.IsValid)
            {
				passiveDefenseType.IsDeleted=false;
				passiveDefenseType.CreationDate= DateTime.Now; 
                passiveDefenseType.Id = Guid.NewGuid();
                db.PassiveDefenseTypes.Add(passiveDefenseType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(passiveDefenseType);
        }

        // GET: PassiveDefenseTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PassiveDefenseType passiveDefenseType = db.PassiveDefenseTypes.Find(id);
            if (passiveDefenseType == null)
            {
                return HttpNotFound();
            }
            return View(passiveDefenseType);
        }

        // POST: PassiveDefenseTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] PassiveDefenseType passiveDefenseType)
        {
            if (ModelState.IsValid)
            {
				passiveDefenseType.IsDeleted = false;
				passiveDefenseType.LastModifiedDate = DateTime.Now;
                db.Entry(passiveDefenseType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(passiveDefenseType);
        }

        // GET: PassiveDefenseTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PassiveDefenseType passiveDefenseType = db.PassiveDefenseTypes.Find(id);
            if (passiveDefenseType == null)
            {
                return HttpNotFound();
            }
            return View(passiveDefenseType);
        }

        // POST: PassiveDefenseTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PassiveDefenseType passiveDefenseType = db.PassiveDefenseTypes.Find(id);
			passiveDefenseType.IsDeleted=true;
			passiveDefenseType.DeletionDate=DateTime.Now;
 
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
