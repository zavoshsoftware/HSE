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
    public class EnviromentsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult CompanyTypeList()
        {
            List<CompanyType> companyTypes = db.CompanyTypes.Where(c => c.IsDeleted == false && c.IsActive).ToList();
            ViewBag.baseUrl = "Enviroments";
            ViewBag.Title = "محیط زیست و سلامت";
            return View(companyTypes);
        }


        public ActionResult List(Guid? id)
        {
            List<Company> companies;

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string uid = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            if (roleName == "supervisor")
            {
                Guid userId = new Guid(uid);

                companies = db.Companies
                    .Where(c => c.SupervisorUserId == userId && c.IsDeleted == false && c.IsActive).ToList();
            }
            else
            {
                if (id == null)
                    companies = db.Companies.Where(c => c.IsDeleted == false && c.IsActive)
                        .ToList();

                else
                    companies = db.Companies.Where(c => c.CompanyTypeId == id && c.IsDeleted == false && c.IsActive)
                        .ToList();

            }
            return View(companies.OrderBy(c => c.Title).ToList());
        }
        public ActionResult Index(Guid? id)
        {

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string uid = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
           string roleTitle = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            ViewBag.roleTitle = roleTitle;
            Guid userId = new Guid(uid);
            Guid? companyId = null;

            if (roleTitle == "company")
            {
                User user = db.Users.Find(userId);

                companyId = user.CompanyId;
            }
            else
            {
                companyId=id;
            }
            if (!db.Enviroments.Any(c => c.CompanyId == companyId))
            {
                List<EnviromentType> enviromentTypes = db.EnviromentTypes.Where(c => c.IsDeleted == false).ToList();

                foreach (EnviromentType enviromentType in enviromentTypes)
                {
                    Enviroment enviroment = new Enviroment()
                    {
                        Id = Guid.NewGuid(),
                        IsDeleted = false,
                        IsActive = true,
                        CreationDate = DateTime.Now,
                        EnviromentTypeId = enviromentType.Id,
                        PermitStatusId = db.PermitStatuses.FirstOrDefault(c => c.Code == 0).Id,
                        CompanyId = companyId.Value
                    };

                    db.Enviroments.Add(enviroment);
                }

                db.SaveChanges();
            }

            var enviroments = db.Enviroments.Include(p => p.Company).Include(c => c.EnviromentType)
                .Where(p => p.CompanyId == companyId && p.IsDeleted == false)
                .OrderByDescending(p => p.PermitStatusId).Include(p => p.PermitStatus);

            return View(enviroments.ToList());
        }

     
         
        public ActionResult UploadFile(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enviroment enviroment = db.Enviroments.Find(id);
            if (enviroment == null)
            {
                return HttpNotFound();
            }
            
            return View(enviroment);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile( Enviroment enviroment, HttpPostedFileBase fileupload)
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

                    enviroment.FileUrl = newFilenameUrl;
                    enviroment.PermitStatusId = db.PermitStatuses.FirstOrDefault(c => c.Code == 1).Id;
                }
                #endregion

                enviroment.IsDeleted = false;
                enviroment.LastModifiedDate = DateTime.Now;
                db.Entry(enviroment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
          
            return View(enviroment);
        }

        public ActionResult SuperVisorConfirmation(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enviroment enviroment = db.Enviroments.Find(id);
            if (enviroment == null)
            {
                return HttpNotFound();
            }

            ViewBag.PermitStatusId = new SelectList(db.PermitStatuses, "Id", "Title", enviroment.PermitStatusId);

            return View(enviroment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuperVisorConfirmation(Enviroment enviroment)
        {

            if (ModelState.IsValid)
            {
                enviroment.IsDeleted = false;
                enviroment.LastModifiedDate = DateTime.Now;
                db.Entry(enviroment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new{id= enviroment.CompanyId});
            }

            return View(enviroment);
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
