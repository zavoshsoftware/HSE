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
    [Authorize(Roles = "Administrator,company,supervisor")]
    public class DocumentsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Documents
        public ActionResult Index(Guid id)
        {
            var documents = db.Documents.Include(d => d.DocumentType).Where(d=>d.DocumentTypeId==id&& d.IsDeleted==false).OrderByDescending(d=>d.CreationDate);
            return View(documents.ToList());
        }
         
        public ActionResult Create(Guid id)
        {
            ViewBag.DocumentTypeId = id;
            return View();
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Document document,Guid id, HttpPostedFileBase fileupload)
        {
           
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/doc/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    document.FileUrl = newFilenameUrl;
                }
                #endregion
                document.DocumentTypeId = id;
				document.IsDeleted=false;
				document.CreationDate= DateTime.Now; 
                document.Id = Guid.NewGuid();
                db.Documents.Add(document);
                db.SaveChanges();
                return RedirectToAction("Index",new{id=id});
            }

            ViewBag.DocumentTypeId = id;
            return View(document);
        }

        // GET: Documents/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            ViewBag.DocumentTypeId =  document.DocumentTypeId;
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Document document, HttpPostedFileBase fileupload)
        {

            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/doc/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    document.FileUrl = newFilenameUrl;
                }
                #endregion
                document.IsDeleted = false;
				document.LastModifiedDate = DateTime.Now;
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = document.DocumentTypeId });
            }
            ViewBag.DocumentTypeId = document.DocumentTypeId;
            return View(document);
        }

        // GET: Documents/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            ViewBag.DocumentTypeId = document.DocumentTypeId;

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Document document = db.Documents.Find(id);
			document.IsDeleted=true;
			document.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("Index",new{id=document.DocumentTypeId});
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
