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
    public class CompanyHumanResourcesController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();


        public ActionResult CompanyTypeList()
        {
            List<CompanyType> companyTypes = db.CompanyTypes.Where(c => c.IsDeleted == false && c.IsActive).ToList();
            ViewBag.baseUrl = "CompanyHumanResources";

            return View(companyTypes);
        }

        [Authorize(Roles = "Administrator,supervisor")]
        public ActionResult List(Guid? id)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string uid = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            Guid userId = new Guid(uid);

            ViewBag.roleName = roleName;

            List<Company> companies = new List<Company>();


            if (roleName == "Administrator")
            {
                if (id == null)
                    companies = db.Companies.Where(c => c.IsDeleted == false && c.IsActive == true).ToList();
                else
                    companies = db.Companies.Where(c => c.CompanyTypeId == id && c.IsDeleted == false && c.IsActive == true).ToList();
            }

            if (roleName == "supervisor")
            {
                companies = db.Companies
                     .Where(c => c.SupervisorUserId == userId && c.IsDeleted == false && c.IsActive).ToList();
            }

            return View(companies);
        }

        [Authorize(Roles = "Administrator,supervisor")]
        public ActionResult IndexAdmin(Guid id)
        {
            var companyHumanResources = db.CompanyHumanResources.Include(c => c.Company)
                .Where(c => c.CompanyId == id && c.IsDeleted == false).OrderByDescending(c => c.CreationDate)
                .Include(c => c.UserJobRate);



            Company company = db.Companies.Find(id);

            if (company != null)
                ViewBag.chart = company.ChartFileUrl;

            return View(companyHumanResources.ToList());
        }

        [Authorize(Roles = "Administrator,supervisor,company")]
        public ActionResult Index(Guid? id)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            if (id != null && (roleName == "Administrator" || roleName == "supervisor"))
                return RedirectToAction("IndexAdmin", new { id = id });


            List<CompanyHumanResource> companyHumanResources = new List<CompanyHumanResource>();


            string uid = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

            Guid userId = new Guid(uid);

            User user = db.Users.FirstOrDefault(c => c.Id == userId && c.IsDeleted == false && c.IsActive);

            if (user != null)
            {
                companyHumanResources = db.CompanyHumanResources.Include(c => c.Company)
                   .Where(c => c.CompanyId == user.CompanyId && c.IsDeleted == false).OrderByDescending(c => c.CreationDate)
                   .Include(c => c.UserJobRate).ToList();

                Company company = db.Companies.Find(user.CompanyId);

                if (company != null)
                    ViewBag.chart = company.ChartFileUrl;
            }
            return View(companyHumanResources);
        }

        // GET: CompanyHumanResources/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyHumanResource companyHumanResource = db.CompanyHumanResources.Find(id);
            if (companyHumanResource == null)
            {
                return HttpNotFound();
            }
            return View(companyHumanResource);
        }

        [Authorize(Roles = "Administrator,supervisor,company")]
        public ActionResult Create(Guid? id)
        {
            Guid? companyId = null;
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;


            if (roleName == "Administrator" || roleName == "supervisor")
            {
                companyId = id;
            }
            else if (roleName == "company")
            {
                string uId = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

                Guid userId = new Guid(uId);

                User user = db.Users.Find(userId);

                companyId = user.CompanyId;
            }

            ViewBag.CompanyId = companyId.Value;
            ViewBag.UserJobRateId = new SelectList(db.UserJobRates, "Id", "Title");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,supervisor,company")]
        public ActionResult Create(CompanyHumanResource companyHumanResource, Guid? id, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/companyHumanResource/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    companyHumanResource.ResumeFileUrl = newFilenameUrl;
                }
                #endregion
                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

                companyHumanResource.IsActive = true;
                companyHumanResource.IsDeleted = false;
                companyHumanResource.CreationDate = DateTime.Now;
                companyHumanResource.Id = Guid.NewGuid();
                db.CompanyHumanResources.Add(companyHumanResource);

                if (roleName == "Administrator" || roleName == "supervisor")
                {
                    companyHumanResource.CompanyId = id.Value;
                    db.SaveChanges();

                    return RedirectToAction("IndexAdmin", new { id = id });

                }
                else if (roleName == "company")
                {
                    string uId = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

                    Guid userId = new Guid(uId);

                    User user = db.Users.Find(userId);

                    companyHumanResource.CompanyId = user.CompanyId.Value;
                    db.SaveChanges();

                    return RedirectToAction("Index");

                }


            }

            ViewBag.CompanyId = id;
            ViewBag.UserJobRateId = new SelectList(db.UserJobRates, "Id", "Title", companyHumanResource.UserJobRateId);
            return View(companyHumanResource);
        }

        [Authorize(Roles = "Administrator,supervisor,company")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyHumanResource companyHumanResource = db.CompanyHumanResources.Find(id);
            if (companyHumanResource == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = companyHumanResource.CompanyId;
            ViewBag.UserJobRateId = new SelectList(db.UserJobRates, "Id", "Title", companyHumanResource.UserJobRateId);
            return View(companyHumanResource);
        }

        [Authorize(Roles = "Administrator,supervisor,company")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompanyHumanResource companyHumanResource, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/companyHumanResource/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    companyHumanResource.ResumeFileUrl = newFilenameUrl;
                }
                #endregion
                companyHumanResource.IsDeleted = false;
                companyHumanResource.LastModifiedDate = DateTime.Now;
                db.Entry(companyHumanResource).State = EntityState.Modified;
                db.SaveChanges();

                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

                if (roleName == "Administrator" || roleName == "supervisor")
                    return RedirectToAction("IndexAdmin", new { id = companyHumanResource.CompanyId });

                if (roleName == "company")
                    return RedirectToAction("Index");

            }
            ViewBag.CompanyId = companyHumanResource.CompanyId;
            ViewBag.UserJobRateId = new SelectList(db.UserJobRates, "Id", "Title", companyHumanResource.UserJobRateId);
            return View(companyHumanResource);
        }

        [Authorize(Roles = "Administrator,supervisor,company")]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyHumanResource companyHumanResource = db.CompanyHumanResources.Find(id);
            if (companyHumanResource == null)
            {
                return HttpNotFound();
            }
            return View(companyHumanResource);
        }

        [Authorize(Roles = "Administrator,supervisor,company")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CompanyHumanResource companyHumanResource = db.CompanyHumanResources.Find(id);
            companyHumanResource.IsDeleted = true;
            companyHumanResource.DeletionDate = DateTime.Now;

            db.SaveChanges();

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            if (roleName == "Administrator" || roleName == "supervisor")
                return RedirectToAction("IndexAdmin", new { id = companyHumanResource.CompanyId });

            else
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

