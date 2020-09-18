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
    public class OperationsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Operations
        public ActionResult Index()
        {
            var operations = db.Operations.Include(o => o.Project).Where(o=>o.IsDeleted==false).OrderByDescending(o=>o.ProjectId);
            return View(operations.ToList());
        }

        // GET: Operations/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operation operation = db.Operations.Find(id);
            if (operation == null)
            {
                return HttpNotFound();
            }
            return View(operation);
        }

        // GET: Operations/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title");
            return View();
        }

        // POST: Operations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Code,ProjectId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Operation operation)
        {
            if (ModelState.IsValid)
            {
				operation.IsDeleted=false;
				operation.CreationDate= DateTime.Now; 
                operation.Id = Guid.NewGuid();
                db.Operations.Add(operation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", operation.ProjectId);
            return View(operation);
        }

        // GET: Operations/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operation operation = db.Operations.Find(id);
            if (operation == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", operation.ProjectId);
            return View(operation);
        }

        // POST: Operations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Code,ProjectId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Operation operation)
        {
            if (ModelState.IsValid)
            {
				operation.IsDeleted = false;
				operation.LastModifiedDate = DateTime.Now;
                db.Entry(operation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", operation.ProjectId);
            return View(operation);
        }

        // GET: Operations/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operation operation = db.Operations.Find(id);
            if (operation == null)
            {
                return HttpNotFound();
            }
            return View(operation);
        }

        // POST: Operations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Operation operation = db.Operations.Find(id);
			operation.IsDeleted=true;
			operation.DeletionDate=DateTime.Now;
 
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
