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
    [Authorize(Roles = "Administrator")]
    public class ReportTypesController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: ReportTypes
        public ActionResult Index()
        {
            return View(db.ReportTypes.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: ReportTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportType reportType = db.ReportTypes.Find(id);
            if (reportType == null)
            {
                return HttpNotFound();
            }
            return View(reportType);
        }

        // GET: ReportTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReportTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReportType reportType, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/reports/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    reportType.SampleFile = newFilenameUrl;
                }
                #endregion
                reportType.IsDeleted=false;
				reportType.CreationDate= DateTime.Now; 
                reportType.Id = Guid.NewGuid();
                db.ReportTypes.Add(reportType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reportType);
        }

        // GET: ReportTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportType reportType = db.ReportTypes.Find(id);
            if (reportType == null)
            {
                return HttpNotFound();
            }
            return View(reportType);
        }

        // POST: ReportTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReportType reportType, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/reports/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    reportType.SampleFile = newFilenameUrl;
                }
                #endregion
                reportType.IsDeleted = false;
				reportType.LastModifiedDate = DateTime.Now;
                db.Entry(reportType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reportType);
        }

        // GET: ReportTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportType reportType = db.ReportTypes.Find(id);
            if (reportType == null)
            {
                return HttpNotFound();
            }
            return View(reportType);
        }

        // POST: ReportTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ReportType reportType = db.ReportTypes.Find(id);
			reportType.IsDeleted=true;
			reportType.DeletionDate=DateTime.Now;
 
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
