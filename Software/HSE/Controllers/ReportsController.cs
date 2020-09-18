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
    public class ReportsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index(Guid id)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;
            List<Report> reports = new List<Report>();
            Guid userId = new Guid(identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value);
            ViewBag.roleName = roleName;

            if (roleName == "Administrator")
            {
                reports = db.Reports.Include(r => r.Company).Where(r => r.IsDeleted == false && r.ReportTypeId == id)
                    .OrderByDescending(r => r.CreationDate).Include(r => r.ReportType).Include(r => r.Status).ToList();

            }

            if (roleName == "company")
            {

                Guid  companyId = GetLoginUserCompanyId();

                 
                    reports = db.Reports.Include(r => r.Company).Where(r =>
                            r.IsDeleted == false && r.ReportTypeId == id && r.CompanyId == companyId)
                        .OrderByDescending(r => r.CreationDate).Include(r => r.ReportType).Include(r => r.Status)
                        .ToList();
                
            }
            if (roleName == "supervisor")
            {
                User user = db.Users.FirstOrDefault(current => current.Id == userId);

                if (user != null)
                {
                    reports = db.Reports.Include(r => r.Company).Where(r =>
                            r.IsDeleted == false && r.ReportTypeId == id && r.Company.SupervisorUserId == user.Id)
                        .OrderByDescending(r => r.CreationDate).Include(r => r.ReportType).Include(r => r.Status)
                        .ToList();
                }
            }


            ReportType reportType = db.ReportTypes.Find(id);

            if (reportType != null)
            {
                if (reportType.Name == "daily")
                    ViewBag.Title = "فهرست گزارش روزانه";

                if (reportType.Name == "weekly")
                    ViewBag.Title = "فهرست گزارش هفتگی";

                if (reportType.Name == "monthly")
                    ViewBag.Title = "فهرست گزارش ماهانه";


                ViewBag.sampleFile = reportType.SampleFile;
            }
            return View(reports.ToList());
        }

        public Guid GetLoginUserCompanyId()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            Guid userId = new Guid(identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value);

           // CompanyUser companyUser = db.CompanyUsers.FirstOrDefault(c => c.UserId == userId);
            User user = db.Users.FirstOrDefault(c => c.Id == userId);

            return user.CompanyId.Value;
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // GET: Reports/Create
        public ActionResult Create(Guid id)
        {
            ViewBag.ReportTypeId = id;
            ViewBag.StatusId = new SelectList(db.Status, "Id", "Title");
            ReportType reportType = db.ReportTypes.Find(id);

            if (reportType != null)
                ViewBag.sampleFile = reportType.SampleFile;
            return View();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Report report, Guid id, HttpPostedFileBase fileupload)
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

                    report.FileUrl = newFilenameUrl;
                }
                #endregion


                report.StatusId = db.Status.OrderBy(c => c.Order).FirstOrDefault().Id;
                report.CompanyId = GetLoginUserCompanyId();
                report.ReportTypeId = id;
                report.IsDeleted = false;
                report.CreationDate = DateTime.Now;
                report.Id = Guid.NewGuid();
                db.Reports.Add(report);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.ReportTypeId = id;
            ViewBag.StatusId = new SelectList(db.Status, "Id", "Title", report.StatusId);

            ReportType reportType = db.ReportTypes.Find(id);

            if (reportType != null)
                ViewBag.sampleFile = reportType.SampleFile;

            return View(report);
        }

        // GET: Reports/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReportTypeId =  report.ReportTypeId;
            ViewBag.StatusId = new SelectList(db.Status, "Id", "Title", report.StatusId);
            ReportType reportType = db.ReportTypes.Find(report.ReportTypeId);

            if (reportType != null)
                ViewBag.sampleFile = reportType.SampleFile;
            return View(report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Report report, HttpPostedFileBase fileupload)
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

                    report.FileUrl = newFilenameUrl;
                }
                #endregion
                report.IsDeleted = false;
                report.LastModifiedDate = DateTime.Now;
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = report.CompanyId });
            }
            ReportType reportType = db.ReportTypes.Find(report.ReportTypeId);

            if (reportType != null)
                ViewBag.sampleFile = reportType.SampleFile;
            ViewBag.ReportTypeId = report.ReportTypeId;
            ViewBag.StatusId = new SelectList(db.Status, "Id", "Title", report.StatusId);
            return View(report);
        }

        // GET: Reports/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReportTypeId = report.ReportTypeId;

            return View(report);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Report report = db.Reports.Find(id);
            report.IsDeleted = true;
            report.DeletionDate = DateTime.Now;

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
