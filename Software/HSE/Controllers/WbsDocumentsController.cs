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
    public class WbsDocumentsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: WbsDocuments
        public ActionResult Index()
        {
            var wbsDocuments = db.WbsDocuments.Include(w => w.WbsUserType).Where(w=>w.IsDeleted==false).OrderByDescending(w=>w.CreationDate);
            return View(wbsDocuments.ToList());
        }

        // GET: WbsDocuments/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WbsDocument wbsDocument = db.WbsDocuments.Find(id);
            if (wbsDocument == null)
            {
                return HttpNotFound();
            }
            return View(wbsDocument);
        }

        // GET: WbsDocuments/Create
        public ActionResult Create()
        {
            ViewBag.WbsUserTypeId = new SelectList(db.WbsUserTypes, "Id", "Title");
            return View();
        }

        // POST: WbsDocuments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,WbsUserTypeId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] WbsDocument wbsDocument)
        {
            if (ModelState.IsValid)
            {
				wbsDocument.IsDeleted=false;
				wbsDocument.CreationDate= DateTime.Now; 
                wbsDocument.Id = Guid.NewGuid();
                db.WbsDocuments.Add(wbsDocument);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WbsUserTypeId = new SelectList(db.WbsUserTypes, "Id", "Title", wbsDocument.WbsUserTypeId);
            return View(wbsDocument);
        }

        // GET: WbsDocuments/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WbsDocument wbsDocument = db.WbsDocuments.Find(id);
            if (wbsDocument == null)
            {
                return HttpNotFound();
            }
            ViewBag.WbsUserTypeId = new SelectList(db.WbsUserTypes, "Id", "Title", wbsDocument.WbsUserTypeId);
            return View(wbsDocument);
        }

        // POST: WbsDocuments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,WbsUserTypeId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] WbsDocument wbsDocument)
        {
            if (ModelState.IsValid)
            {
				wbsDocument.IsDeleted = false;
				wbsDocument.LastModifiedDate = DateTime.Now;
                db.Entry(wbsDocument).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WbsUserTypeId = new SelectList(db.WbsUserTypes, "Id", "Title", wbsDocument.WbsUserTypeId);
            return View(wbsDocument);
        }

        // GET: WbsDocuments/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WbsDocument wbsDocument = db.WbsDocuments.Find(id);
            if (wbsDocument == null)
            {
                return HttpNotFound();
            }
            return View(wbsDocument);
        }

        // POST: WbsDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            WbsDocument wbsDocument = db.WbsDocuments.Find(id);
			wbsDocument.IsDeleted=true;
			wbsDocument.DeletionDate=DateTime.Now;
 
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
