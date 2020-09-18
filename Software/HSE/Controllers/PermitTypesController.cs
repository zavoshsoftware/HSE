using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;

namespace HSE.Controllers
{
    public class PermitTypesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: PermitTypes
        public ActionResult Index()
        {
            return View(db.PermitTypes.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: PermitTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermitType permitType = db.PermitTypes.Find(id);
            if (permitType == null)
            {
                return HttpNotFound();
            }
            return View(permitType);
        }

        // GET: PermitTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PermitTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PermitType permitType, HttpPostedFileBase fileupload)
        {

            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/permit/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    permitType.FileUrl = newFilenameUrl;
                }
                #endregion
                permitType.IsDeleted=false;
				permitType.CreationDate= DateTime.Now; 
                permitType.Id = Guid.NewGuid();
                db.PermitTypes.Add(permitType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(permitType);
        }

        // GET: PermitTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermitType permitType = db.PermitTypes.Find(id);
            if (permitType == null)
            {
                return HttpNotFound();
            }
            return View(permitType);
        }

        // POST: PermitTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PermitType permitType, HttpPostedFileBase fileupload)
        {

            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/permit/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    permitType.FileUrl = newFilenameUrl;
                }
                #endregion
                permitType.IsDeleted = false;
				permitType.LastModifiedDate = DateTime.Now;
                db.Entry(permitType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(permitType);
        }

        // GET: PermitTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermitType permitType = db.PermitTypes.Find(id);
            if (permitType == null)
            {
                return HttpNotFound();
            }
            return View(permitType);
        }

        // POST: PermitTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PermitType permitType = db.PermitTypes.Find(id);
			permitType.IsDeleted=true;
			permitType.DeletionDate=DateTime.Now;
 
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
