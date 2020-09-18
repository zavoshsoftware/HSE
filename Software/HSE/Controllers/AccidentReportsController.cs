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
    public class AccidentReportsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: AccidentReports
        public ActionResult Index()
        {
            return View(db.AccidentReports.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: AccidentReports/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentReport accidentReport = db.AccidentReports.Find(id);
            if (accidentReport == null)
            {
                return HttpNotFound();
            }
            return View(accidentReport);
        }

        // GET: AccidentReports/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccidentReports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccidentReport accidentReport, HttpPostedFileBase fileupload)
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

                    accidentReport.BaseFileUrl = newFilenameUrl;
                }
                #endregion
                accidentReport.IsDeleted=false;
				accidentReport.CreationDate= DateTime.Now; 
                accidentReport.Id = Guid.NewGuid();
                db.AccidentReports.Add(accidentReport);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(accidentReport);
        }

        // GET: AccidentReports/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentReport accidentReport = db.AccidentReports.Find(id);
            if (accidentReport == null)
            {
                return HttpNotFound();
            }
            return View(accidentReport);
        }

        // POST: AccidentReports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccidentReport accidentReport, HttpPostedFileBase fileupload)
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

                    accidentReport.BaseFileUrl = newFilenameUrl;
                }
                #endregion
                accidentReport.IsDeleted = false;
				accidentReport.LastModifiedDate = DateTime.Now;
                db.Entry(accidentReport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(accidentReport);
        }

        // GET: AccidentReports/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccidentReport accidentReport = db.AccidentReports.Find(id);
            if (accidentReport == null)
            {
                return HttpNotFound();
            }
            return View(accidentReport);
        }

        // POST: AccidentReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AccidentReport accidentReport = db.AccidentReports.Find(id);
			accidentReport.IsDeleted=true;
			accidentReport.DeletionDate=DateTime.Now;
 
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
