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
    public class SafetiesController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();


        public ActionResult List(Guid id)
        {
            List<Company> companies = db.Companies.Where(c => c.IsDeleted == false && c.IsActive).ToList();
            ViewBag.SafetyTypeId = id.ToString();

            SafetyType safetyType = db.SafetyTypes.Find(id);
            ViewBag.SafetyTypeTitle ="مشاهده اسناد "+ safetyType.Title;

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string uId = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;
            Guid userId = new Guid(uId);

            if (roleName == "supervisor")
            {
                companies = db.Companies
                    .Where(c => c.SupervisorUserId == userId && c.IsDeleted == false && c.IsActive).ToList();
            }

            return View(companies);
        }

        

        [Route("safety/adminindex/{id:Guid}/{companyId:Guid}")]
        public ActionResult AdminIndex(Guid id, Guid companyId)
        {
            List<Safety> safeties = db.Safeties.Include(h => h.Company)
                .Where(h => h.CompanyId == companyId && h.SafetyTypeId == id && h.IsDeleted == false)
                .OrderByDescending(h => h.CreationDate).Include(h => h.SafetyType).ToList();

            SafetyType safetyType = db.SafetyTypes.Find(id);

            if (safetyType != null)
                ViewBag.Title = "فهرست " + safetyType.Title;

            return View(safeties);
        }

        public ActionResult Index(Guid id)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string uId = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            Guid userId = new Guid(uId);

            User user = db.Users.FirstOrDefault(c => c.Id == userId);

            var safeties = db.Safeties.Include(s => s.Company).Where(s =>
                    s.SafetyTypeId == id && s.CompanyId == user.CompanyId && s.IsDeleted == false)
                .OrderByDescending(s => s.CreationDate).Include(s => s.SafetyFileType).Include(s => s.SafetyType);


            SafetyType safetyType = db.SafetyTypes.Find(id);

            if (safetyType != null)
                ViewBag.Title = "فهرست " + safetyType.Title;


            return View(safeties.ToList());
        }

        public ActionResult Create(Guid id)
        {
            ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.IsActive), "Id", "Title");
            ViewBag.SafetyFileTypeId = new SelectList(GetFileType(id), "Id", "Title");
            ViewBag.SafetyTypeId = id.ToString();
            return View();
        }

        public List<SafetyFileType> GetFileType(Guid typeId)
        {
            if (typeId == new Guid("714B6624-2B90-4CAD-84B9-148E11541809"))
                return db.SafetyFileTypes.Where(c => c.Code == 1 && c.IsDeleted == false && c.IsActive).ToList();

            if (typeId == new Guid("88AD834B-4D74-41F9-8C61-C226935B5913"))
                return db.SafetyFileTypes.Where(c => c.Code == 3 && c.IsDeleted == false && c.IsActive).ToList();

            return new List<SafetyFileType>();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Safety safety, Guid id, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/safety/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    safety.FileUrl = newFilenameUrl;
                }
                #endregion

                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

                string uid = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

                Guid userId = new Guid(uid);

                User user = db.Users.Find(userId);

                if (user.CompanyId != null)
                    safety.CompanyId = user.CompanyId.Value;

                safety.SafetyTypeId = id;
                safety.IsActive = true;
                safety.IsDeleted = false;
                safety.CreationDate = DateTime.Now;
                safety.Id = Guid.NewGuid();
                db.Safeties.Add(safety);
                db.SaveChanges();


                Company co = db.Companies.Find(user.CompanyId);

                Helpers.NotificationHelper.InsertNotification(co.Title, "/safety/adminindex/" + safety.SafetyTypeId+"/" + co.Id, "ایمنی");

                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.IsActive), "Id", "Title");
            ViewBag.SafetyFileTypeId = new SelectList(GetFileType(id), "Id", "Title");
            ViewBag.SafetyTypeId = id.ToString();
            return View(safety);
        }


        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Safety safety = db.Safeties.Find(id);
            if (safety == null)
            {
                return HttpNotFound();
            }

            ViewBag.SafetyFileTypeId = new SelectList(GetFileType(safety.SafetyTypeId), "Id", "Title", safety.SafetyFileTypeId);
            ViewBag.SafetyTypeId = safety.SafetyTypeId.ToString();
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title", safety.CompanyId);

            return View(safety);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Safety safety, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/safety/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    safety.FileUrl = newFilenameUrl;
                }
                #endregion

                safety.IsDeleted = false;
                safety.LastModifiedDate = DateTime.Now;
                db.Entry(safety).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = safety.SafetyTypeId });
            }
            ViewBag.SafetyFileTypeId = new SelectList(GetFileType(safety.SafetyTypeId), "Id", "Title", safety.SafetyFileTypeId);
            ViewBag.SafetyTypeId = safety.SafetyTypeId.ToString();
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title", safety.CompanyId);
            return View(safety);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Safety safety = db.Safeties.Find(id);
            if (safety == null)
            {
                return HttpNotFound();
            }
            ViewBag.SafetyTypeId = safety.SafetyTypeId.ToString();

            return View(safety);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Safety safety = db.Safeties.Find(id);
            safety.IsDeleted = true;
            safety.DeletionDate = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index", new { id = safety.SafetyTypeId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        public ActionResult SupervisorComment(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Safety safety = db.Safeties.Find(id);
            if (safety == null)
            {
                return HttpNotFound();
            }
            return View(safety);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SupervisorComment(Safety safety)
        {
            if (ModelState.IsValid)
            {

                safety.IsDeleted = false;
                safety.LastModifiedDate = DateTime.Now;
                db.Entry(safety).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdminIndex", new { id = safety.SafetyTypeId, companyId=safety.CompanyId });
            }
            return View(safety);
        }

    }
}
