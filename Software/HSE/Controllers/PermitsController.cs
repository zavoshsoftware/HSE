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
    public class PermitsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult List()
        {
            List<Company> companies = db.Companies.Where(c => c.IsDeleted == false && c.IsActive ).ToList();

            return View(companies);
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
            if (!db.Permits.Any(c => c.CompanyId == companyId))
            {
                List<PermitType> permitTypes = db.PermitTypes.Where(c => c.IsDeleted == false).ToList();

                foreach (PermitType permitType in permitTypes)
                {
                    Permit permit = new Permit()
                    {
                        Id = Guid.NewGuid(),
                        IsDeleted = false,
                        IsActive = true,
                        CreationDate = DateTime.Now,
                        PermitTypeId = permitType.Id,
                        PermitStatusId = db.PermitStatuses.FirstOrDefault(c => c.Code == 0).Id,
                        CompanyId = companyId.Value
                    };

                    db.Permits.Add(permit);
                }

                db.SaveChanges();
            }

            var permits = db.Permits.Include(p => p.Company).Include(c => c.PermitType)
                .Where(p => p.CompanyId == companyId && p.IsDeleted == false)
                .OrderByDescending(p => p.PermitStatusId).Include(p => p.PermitStatus);

            return View(permits.ToList());
        }

     
         
        public ActionResult UploadFile(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Permit permit = db.Permits.Find(id);
            if (permit == null)
            {
                return HttpNotFound();
            }
            
            return View(permit);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile( Permit permit, HttpPostedFileBase fileupload)
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

                    permit.FileUrl = newFilenameUrl;
                    permit.PermitStatusId = db.PermitStatuses.FirstOrDefault(c => c.Code == 1).Id;
                }
                #endregion

                permit.IsDeleted = false;
                permit.LastModifiedDate = DateTime.Now;
                db.Entry(permit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
          
            return View(permit);
        }

        public ActionResult SuperVisorConfirmation(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Permit permit = db.Permits.Find(id);
            if (permit == null)
            {
                return HttpNotFound();
            }

            ViewBag.PermitStatusId = new SelectList(db.PermitStatuses, "Id", "Title",permit.PermitStatusId);

            return View(permit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuperVisorConfirmation(Permit permit)
        {

            if (ModelState.IsValid)
            {
                permit.IsDeleted = false;
                permit.LastModifiedDate = DateTime.Now;
                db.Entry(permit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new{id=permit.CompanyId});
            }

            return View(permit);
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
