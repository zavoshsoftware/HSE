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
    public class WbsUserTypesController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: WbsUserTypes
        public ActionResult Index()
        {
            var wbsUserTypes = db.WbsUserTypes.Include(w => w.WbsRequirment).Where(w=>w.IsDeleted==false).OrderByDescending(w=>w.CreationDate);
            return View(wbsUserTypes.ToList());
        }

        // GET: WbsUserTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WbsUserType wbsUserType = db.WbsUserTypes.Find(id);
            if (wbsUserType == null)
            {
                return HttpNotFound();
            }
            return View(wbsUserType);
        }

        // GET: WbsUserTypes/Create
        public ActionResult Create()
        {
            ViewBag.WbsRequirmentId = new SelectList(db.WbsRequirments, "Id", "Title");
            return View();
        }

        // POST: WbsUserTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,WbsRequirmentId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] WbsUserType wbsUserType)
        {
            if (ModelState.IsValid)
            {
				wbsUserType.IsDeleted=false;
				wbsUserType.CreationDate= DateTime.Now; 
                wbsUserType.Id = Guid.NewGuid();
                db.WbsUserTypes.Add(wbsUserType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WbsRequirmentId = new SelectList(db.WbsRequirments, "Id", "Title", wbsUserType.WbsRequirmentId);
            return View(wbsUserType);
        }

        // GET: WbsUserTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WbsUserType wbsUserType = db.WbsUserTypes.Find(id);
            if (wbsUserType == null)
            {
                return HttpNotFound();
            }
            ViewBag.WbsRequirmentId = new SelectList(db.WbsRequirments, "Id", "Title", wbsUserType.WbsRequirmentId);
            return View(wbsUserType);
        }

        // POST: WbsUserTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,WbsRequirmentId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] WbsUserType wbsUserType)
        {
            if (ModelState.IsValid)
            {
				wbsUserType.IsDeleted = false;
				wbsUserType.LastModifiedDate = DateTime.Now;
                db.Entry(wbsUserType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WbsRequirmentId = new SelectList(db.WbsRequirments, "Id", "Title", wbsUserType.WbsRequirmentId);
            return View(wbsUserType);
        }

        // GET: WbsUserTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WbsUserType wbsUserType = db.WbsUserTypes.Find(id);
            if (wbsUserType == null)
            {
                return HttpNotFound();
            }
            return View(wbsUserType);
        }

        // POST: WbsUserTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            WbsUserType wbsUserType = db.WbsUserTypes.Find(id);
			wbsUserType.IsDeleted=true;
			wbsUserType.DeletionDate=DateTime.Now;
 
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
