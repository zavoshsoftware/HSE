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
    public class RequirmentsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Requirments
        public ActionResult Index()
        {
            var requirments = db.Requirments.Include(r => r.RequirmentType).Where(r=>r.IsDeleted==false).OrderByDescending(r=>r.CreationDate);
            return View(requirments.ToList());
        }

        // GET: Requirments/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requirment requirment = db.Requirments.Find(id);
            if (requirment == null)
            {
                return HttpNotFound();
            }
            return View(requirment);
        }

        // GET: Requirments/Create
        public ActionResult Create()
        {
            ViewBag.RequirmentTypeId = new SelectList(db.RequirmentTypes.OrderBy(c=>c.Order), "Id", "Title");
            return View();
        }

        // POST: Requirments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Order,Title,Weight,RequirmentTypeId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Requirment requirment)
        {
            if (ModelState.IsValid)
            {
				requirment.IsDeleted=false;
				requirment.CreationDate= DateTime.Now; 
                requirment.Id = Guid.NewGuid();
                db.Requirments.Add(requirment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RequirmentTypeId = new SelectList(db.RequirmentTypes, "Id", "Title", requirment.RequirmentTypeId);
            return View(requirment);
        }

        // GET: Requirments/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requirment requirment = db.Requirments.Find(id);
            if (requirment == null)
            {
                return HttpNotFound();
            }
            ViewBag.RequirmentTypeId = new SelectList(db.RequirmentTypes, "Id", "Title", requirment.RequirmentTypeId);
            return View(requirment);
        }

        // POST: Requirments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Order,Title,Weight,RequirmentTypeId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Requirment requirment)
        {
            if (ModelState.IsValid)
            {
				requirment.IsDeleted = false;
				requirment.LastModifiedDate = DateTime.Now;
                db.Entry(requirment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RequirmentTypeId = new SelectList(db.RequirmentTypes, "Id", "Title", requirment.RequirmentTypeId);
            return View(requirment);
        }

        // GET: Requirments/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requirment requirment = db.Requirments.Find(id);
            if (requirment == null)
            {
                return HttpNotFound();
            }
            return View(requirment);
        }

        // POST: Requirments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Requirment requirment = db.Requirments.Find(id);
			requirment.IsDeleted=true;
			requirment.DeletionDate=DateTime.Now;
 
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
