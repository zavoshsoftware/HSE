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
    public class RelationTypesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: RelationTypes
        public ActionResult Index()
        {
            return View(db.RelationTypes.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: RelationTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RelationType relationType = db.RelationTypes.Find(id);
            if (relationType == null)
            {
                return HttpNotFound();
            }
            return View(relationType);
        }

        // GET: RelationTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RelationTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] RelationType relationType)
        {
            if (ModelState.IsValid)
            {
				relationType.IsDeleted=false;
				relationType.CreationDate= DateTime.Now; 
                relationType.Id = Guid.NewGuid();
                db.RelationTypes.Add(relationType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(relationType);
        }

        // GET: RelationTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RelationType relationType = db.RelationTypes.Find(id);
            if (relationType == null)
            {
                return HttpNotFound();
            }
            return View(relationType);
        }

        // POST: RelationTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] RelationType relationType)
        {
            if (ModelState.IsValid)
            {
				relationType.IsDeleted = false;
				relationType.LastModifiedDate = DateTime.Now;
                db.Entry(relationType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(relationType);
        }

        // GET: RelationTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RelationType relationType = db.RelationTypes.Find(id);
            if (relationType == null)
            {
                return HttpNotFound();
            }
            return View(relationType);
        }

        // POST: RelationTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            RelationType relationType = db.RelationTypes.Find(id);
			relationType.IsDeleted=true;
			relationType.DeletionDate=DateTime.Now;
 
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
