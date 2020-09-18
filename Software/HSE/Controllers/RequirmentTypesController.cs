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
    public class RequirmentTypesController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: RequirmentTypes
        public ActionResult Index()
        {
            return View(db.RequirmentTypes.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: RequirmentTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequirmentType requirmentType = db.RequirmentTypes.Find(id);
            if (requirmentType == null)
            {
                return HttpNotFound();
            }
            return View(requirmentType);
        }

        // GET: RequirmentTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RequirmentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Order,Title,Weight,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] RequirmentType requirmentType)
        {
            if (ModelState.IsValid)
            {
				requirmentType.IsDeleted=false;
				requirmentType.CreationDate= DateTime.Now; 
                requirmentType.Id = Guid.NewGuid();
                db.RequirmentTypes.Add(requirmentType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(requirmentType);
        }

        // GET: RequirmentTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequirmentType requirmentType = db.RequirmentTypes.Find(id);
            if (requirmentType == null)
            {
                return HttpNotFound();
            }
            return View(requirmentType);
        }

        // POST: RequirmentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Order,Title,Weight,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] RequirmentType requirmentType)
        {
            if (ModelState.IsValid)
            {
				requirmentType.IsDeleted = false;
				requirmentType.LastModifiedDate = DateTime.Now;
                db.Entry(requirmentType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(requirmentType);
        }

        // GET: RequirmentTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequirmentType requirmentType = db.RequirmentTypes.Find(id);
            if (requirmentType == null)
            {
                return HttpNotFound();
            }
            return View(requirmentType);
        }

        // POST: RequirmentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            RequirmentType requirmentType = db.RequirmentTypes.Find(id);
			requirmentType.IsDeleted=true;
			requirmentType.DeletionDate=DateTime.Now;
 
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
