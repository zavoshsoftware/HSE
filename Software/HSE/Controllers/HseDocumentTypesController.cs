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
    public class HseDocumentTypesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            return View(db.HseDocumentTypes.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HseDocumentType hseDocumentType = db.HseDocumentTypes.Find(id);
            if (hseDocumentType == null)
            {
                return HttpNotFound();
            }
            return View(hseDocumentType);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HseDocumentType hseDocumentType)
        {
            if (ModelState.IsValid)
            {
				hseDocumentType.IsDeleted=false;
				hseDocumentType.CreationDate= DateTime.Now; 
                hseDocumentType.Id = Guid.NewGuid();
                db.HseDocumentTypes.Add(hseDocumentType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hseDocumentType);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HseDocumentType hseDocumentType = db.HseDocumentTypes.Find(id);
            if (hseDocumentType == null)
            {
                return HttpNotFound();
            }
            return View(hseDocumentType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HseDocumentType hseDocumentType)
        {
            if (ModelState.IsValid)
            {
				hseDocumentType.IsDeleted = false;
				hseDocumentType.LastModifiedDate = DateTime.Now;
                db.Entry(hseDocumentType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hseDocumentType);
        }

        // GET: HseDocumentTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HseDocumentType hseDocumentType = db.HseDocumentTypes.Find(id);
            if (hseDocumentType == null)
            {
                return HttpNotFound();
            }
            return View(hseDocumentType);
        }

        // POST: HseDocumentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            HseDocumentType hseDocumentType = db.HseDocumentTypes.Find(id);
			hseDocumentType.IsDeleted=true;
			hseDocumentType.DeletionDate=DateTime.Now;
 
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
