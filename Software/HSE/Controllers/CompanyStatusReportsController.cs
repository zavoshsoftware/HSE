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
    public class CompanyStatusReportsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index(Guid id)
        {
            var companyStatusReports = db.CompanyStatusReports.Include(c => c.Company).Where(c=>c.CompanyId==id&& c.IsDeleted==false).OrderByDescending(c=>c.CreationDate);
            return View(companyStatusReports.ToList());
        }

        public ActionResult Create(Guid id)
        {
            ViewBag.CompanyId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompanyStatusReport companyStatusReport, Guid id, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/companyStatusReport/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    companyStatusReport.FileUrl = newFilenameUrl;
                }
                #endregion
                companyStatusReport.CompanyId = id;
				companyStatusReport.IsDeleted=false;
				companyStatusReport.CreationDate= DateTime.Now; 
                companyStatusReport.Id = Guid.NewGuid();
                db.CompanyStatusReports.Add(companyStatusReport);
                db.SaveChanges();
                return RedirectToAction("Index",new{id=id});
            }

            ViewBag.CompanyId =id;
            return View(companyStatusReport);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyStatusReport companyStatusReport = db.CompanyStatusReports.Find(id);
            if (companyStatusReport == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId =  companyStatusReport.CompanyId;
            return View(companyStatusReport);
        }

  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompanyStatusReport companyStatusReport, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/companyStatusReport/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    companyStatusReport.FileUrl = newFilenameUrl;
                }
                #endregion
                companyStatusReport.IsDeleted = false;
				companyStatusReport.LastModifiedDate = DateTime.Now;
                db.Entry(companyStatusReport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new{id=companyStatusReport.CompanyId});
            }
            ViewBag.CompanyId = companyStatusReport.CompanyId;
            return View(companyStatusReport);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyStatusReport companyStatusReport = db.CompanyStatusReports.Find(id);
            if (companyStatusReport == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = companyStatusReport.CompanyId;

            return View(companyStatusReport);
        }

        // POST: CompanyStatusReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CompanyStatusReport companyStatusReport = db.CompanyStatusReports.Find(id);
			companyStatusReport.IsDeleted=true;
			companyStatusReport.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("Index", new { id = companyStatusReport.CompanyId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    [Authorize(Roles = "company")]
        public ActionResult List()
        {
            List<CompanyStatusReport> reports=new List<CompanyStatusReport>();

            if (User.Identity.IsAuthenticated)
            {
                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

                string name = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

                Guid id = new Guid(name);

                CompanyUser companyUser = db.CompanyUsers.FirstOrDefault(c => c.UserId == id);

                if (companyUser != null)
                {
                    Guid companyId = companyUser.CompanyId;

                    reports = db.CompanyStatusReports.Include(c => c.Company)
                        .Where(c => c.CompanyId == companyId && c.IsDeleted == false).OrderByDescending(c => c.CreationDate)
                        .ToList();
                }
              
            }
            return View(reports);
        }

    }
}
