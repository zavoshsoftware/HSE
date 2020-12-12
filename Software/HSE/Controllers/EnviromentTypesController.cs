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
    public class EnviromentTypesController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: EnviromentTypes
        public ActionResult Index()
        {
            return View(db.EnviromentTypes.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: EnviromentTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnviromentType enviromentType = db.EnviromentTypes.Find(id);
            if (enviromentType == null)
            {
                return HttpNotFound();
            }
            return View(enviromentType);
        }

        // GET: EnviromentTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EnviromentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EnviromentType enviromentType, HttpPostedFileBase fileupload)
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

                    enviromentType.FileUrl = newFilenameUrl;
                }
                #endregion
                enviromentType.IsDeleted=false;
				enviromentType.CreationDate= DateTime.Now; 
                enviromentType.Id = Guid.NewGuid();
                db.EnviromentTypes.Add(enviromentType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(enviromentType);
        }

        // GET: EnviromentTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnviromentType enviromentType = db.EnviromentTypes.Find(id);
            if (enviromentType == null)
            {
                return HttpNotFound();
            }
            return View(enviromentType);
        }

        // POST: EnviromentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EnviromentType enviromentType, HttpPostedFileBase fileupload)
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

                    enviromentType.FileUrl = newFilenameUrl;
                }
                #endregion
                enviromentType.IsDeleted = false;
				enviromentType.LastModifiedDate = DateTime.Now;
                db.Entry(enviromentType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(enviromentType);
        }

        // GET: EnviromentTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnviromentType enviromentType = db.EnviromentTypes.Find(id);
            if (enviromentType == null)
            {
                return HttpNotFound();
            }
            return View(enviromentType);
        }

        // POST: EnviromentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            EnviromentType enviromentType = db.EnviromentTypes.Find(id);
			enviromentType.IsDeleted=true;
			enviromentType.DeletionDate=DateTime.Now;
 
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
