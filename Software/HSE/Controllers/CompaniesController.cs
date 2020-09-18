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
    public class CompaniesController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

    [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var companies = db.Companies.Include(c => c.SupervisorUser).Where(c=>c.IsDeleted==false).OrderByDescending(c=>c.CreationDate);
            return View(companies.ToList());
        }

    [Authorize(Roles = "Administrator")]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
    [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            ViewBag.SupervisorUserId = new SelectList(db.Users.Where(current=>current.Role.Name== "supervisor"&&current.IsDeleted==false), "Id", "FullName");
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Company company, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/companyContract/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    company.ContractItemFileUrl = newFilenameUrl;
                }
                #endregion
                company.IsDeleted=false;
				company.CreationDate= DateTime.Now; 
                company.Id = Guid.NewGuid();
                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SupervisorUserId = new SelectList(db.Users.Where(current => current.Role.Name == "supervisor" && current.IsDeleted == false), "Id", "FullName", company.SupervisorUserId);
            return View(company);
        }

        // GET: Companies/Edit/5
    [Authorize(Roles = "Administrator")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupervisorUserId = new SelectList(db.Users.Where(current => current.Role.Name == "supervisor" && current.IsDeleted == false), "Id", "FullName", company.SupervisorUserId);
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
    [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company company, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/companyContract/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    company.ContractItemFileUrl = newFilenameUrl;
                }
                #endregion
                company.IsDeleted = false;
				company.LastModifiedDate = DateTime.Now;
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SupervisorUserId = new SelectList(db.Users.Where(current => current.Role.Name == "supervisor" && current.IsDeleted == false), "Id", "FullName", company.SupervisorUserId);
            return View(company);
        }

        // GET: Companies/Delete/5
    [Authorize(Roles = "Administrator")]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Company company = db.Companies.Find(id);
			company.IsDeleted=true;
			company.DeletionDate=DateTime.Now;
 
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


        [Authorize(Roles = "company")]
        public ActionResult DownloadContact()
        {

            if (User.Identity.IsAuthenticated)
            {
                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

                string name = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

                Guid id = new Guid(name);

                CompanyUser companyUser = db.CompanyUsers.FirstOrDefault(c => c.UserId == id);

                if (companyUser != null)
                {
                    Guid companyId = companyUser.CompanyId;

                    Company company = db.Companies.Find(companyId);

                    if (company != null)
                        ViewBag.contract = "../../" + company.ContractItemFileUrl;
                }

            }
            return View();
        }


    }
}
