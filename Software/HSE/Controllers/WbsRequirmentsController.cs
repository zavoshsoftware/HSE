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
    public class WbsRequirmentsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: WbsRequirments
        public ActionResult Index()
        {
            return View(db.WbsRequirments.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: WbsRequirments/Details/5
        public ActionResult Details()
        {
         
          List<WbsRequirment> wbsRequirments = db.WbsRequirments.Where(c => c.IsDeleted == false && c.IsActive)
                .OrderBy(c => c.CreationDate).ToList();
          
            return View(wbsRequirments);
        }

        // GET: WbsRequirments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WbsRequirments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] WbsRequirment wbsRequirment)
        {
            if (ModelState.IsValid)
            {
				wbsRequirment.IsDeleted=false;
				wbsRequirment.CreationDate= DateTime.Now; 
                wbsRequirment.Id = Guid.NewGuid();
                db.WbsRequirments.Add(wbsRequirment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(wbsRequirment);
        }

        // GET: WbsRequirments/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WbsRequirment wbsRequirment = db.WbsRequirments.Find(id);
            if (wbsRequirment == null)
            {
                return HttpNotFound();
            }
            return View(wbsRequirment);
        }

        // POST: WbsRequirments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] WbsRequirment wbsRequirment)
        {
            if (ModelState.IsValid)
            {
				wbsRequirment.IsDeleted = false;
				wbsRequirment.LastModifiedDate = DateTime.Now;
                db.Entry(wbsRequirment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(wbsRequirment);
        }

        // GET: WbsRequirments/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WbsRequirment wbsRequirment = db.WbsRequirments.Find(id);
            if (wbsRequirment == null)
            {
                return HttpNotFound();
            }
            return View(wbsRequirment);
        }

        // POST: WbsRequirments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            WbsRequirment wbsRequirment = db.WbsRequirments.Find(id);
			wbsRequirment.IsDeleted=true;
			wbsRequirment.DeletionDate=DateTime.Now;
 
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
