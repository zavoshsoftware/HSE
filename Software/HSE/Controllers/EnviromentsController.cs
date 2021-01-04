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

        public ActionResult CompanyTypeList(Guid enviromentTypeId)
        {
            List<CompanyType> companyTypes = db.CompanyTypes.Where(c => c.IsDeleted == false && c.IsActive).ToList();
            ViewBag.baseUrl = "Enviroments";
            ViewBag.Title = GetTitle(enviromentTypeId.ToString());
            ViewBag.enviromentTypeId = enviromentTypeId;
            return View(companyTypes);
        }

        public string GetTitle(string enviromentTypeId)
        {
            if (enviromentTypeId == "5fe6b043-e89e-4e6c-9d08-63b8f53dacf4" ||
                enviromentTypeId == "e0fc1ce2-cb91-4790-91cb-4665169ecb6d" ||
                enviromentTypeId == "04cad662-c587-4439-bb00-42a6b6971110" ||
                enviromentTypeId == "d666e68e-1112-40e0-bc0a-1e5a3789d045" ||
                enviromentTypeId == "ba6c1d7d-5388-463e-9af6-9dfbe9e59963" ||
                enviromentTypeId == "cce19caf-6e0e-4ead-8b3d-9f72d4986451" ||
                enviromentTypeId == "4cf9a83c-e9c0-433f-8b6e-bb75b8268de7" ||
                enviromentTypeId == "570458bc-a2f4-41b0-95f1-a55b5b65824d")
                return "فهرست بهداشت";
                return "فهرست محیط زیست";
        }

        public ActionResult List(Guid? id, Guid enviromentTypeId)
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
            EnviromentType enviromentType = db.EnviromentTypes.Find(enviromentTypeId);
            ViewBag.TypeTitle = enviromentType.Title;
         
            ViewBag.enviromentTypeId = enviromentTypeId;
            return View(companies.OrderBy(c => c.Title).ToList());
        }
        public ActionResult Index(Guid? id, Guid enviromentTypeId)
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
                companyId = id;
            }

            var enviroments = db.Enviroments.Include(p => p.Company).Include(c => c.EnviromentType)
                .Where(p => p.CompanyId == companyId && p.EnviromentTypeId == enviromentTypeId && p.IsDeleted == false)
                .OrderByDescending(p => p.PermitStatusId).Include(p => p.PermitStatus);

            ViewBag.companyId = companyId;
            ViewBag.enviromentTypeId = enviromentTypeId;
            ViewBag.Title = GetTitle(enviromentTypeId.ToString());

            return View(enviroments.ToList());
        }



        public ActionResult UploadFile(Guid id, Guid enviromentTypeId)
        {
           
            ViewBag.companyId = id;
            ViewBag.enviromentTypeId = enviromentTypeId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile(Enviroment enviroment, HttpPostedFileBase fileupload, Guid id, Guid enviromentTypeId)
        {

            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed

                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/enviroment/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    enviroment.FileUrl = newFilenameUrl;
                    enviroment.PermitStatusId = db.PermitStatuses.FirstOrDefault(c => c.Code == 1).Id;
                }
                else
                {
                    ModelState.AddModelError("noUpload","فایل مورد نظر را بارگزاری کنید");
                    ViewBag.companyId = id;
                    ViewBag.enviromentTypeId = enviromentTypeId;
                    return View();
                }
                #endregion

                enviroment.EnviromentTypeId = enviromentTypeId;
                enviroment.CompanyId = id;
                enviroment.IsActive = true;
                enviroment.IsDeleted = false;
                enviroment.CreationDate = DateTime.Now;
                enviroment.Id = Guid.NewGuid();
                db.Enviroments.Add(enviroment);
                db.SaveChanges();

                Company co = db.Companies.Find(enviroment.CompanyId);
                Helpers.NotificationHelper.InsertNotification(co.Title, "/Enviroments/Index/" + enviroment.CompanyId + "?enviromentTypeId=" + enviromentTypeId, "محیط زیست");
                Helpers.NotificationHelper.InsertNotificationForSup(co.Id,co.Title, "/Enviroments/Index/" + enviroment.CompanyId + "?enviromentTypeId=" + enviromentTypeId, "محیط زیست");

                return RedirectToAction("Index", new { id = id, enviromentTypeId = enviromentTypeId });
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
            ViewBag.companyId = enviroment.CompanyId;
            ViewBag.enviromentTypeId = enviroment.EnviromentTypeId;
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

                Helpers.NotificationHelper.InsertNotificationForCompany(enviroment.CompanyId, "ناظر", "/enviroments/index?enviromentTypeId=" + enviroment.EnviromentTypeId, "محیط زیست و بهداشت", "edit");

                return RedirectToAction("Index", new { id = enviroment.CompanyId, enviromentTypeId = enviroment.EnviromentTypeId });
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



        public ActionResult Edit(Guid? id)
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
        public ActionResult Edit(Enviroment enviroment, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/enviroment/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    enviroment.FileUrl = newFilenameUrl;
                    enviroment.PermitStatusId = db.PermitStatuses.FirstOrDefault(c => c.Code == 1).Id;
                }

                enviroment.IsDeleted = false;
                enviroment.LastModifiedDate = DateTime.Now;
                db.Entry(enviroment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = enviroment.CompanyId, enviromentTypeId = enviroment.EnviromentTypeId });
            }
            return View(enviroment);
        }

        public ActionResult Delete(Guid? id)
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Enviroment enviroment = db.Enviroments.Find(id);
            enviroment.IsDeleted = true;
            enviroment.DeletionDate = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index", new { id = enviroment.CompanyId, enviromentTypeId = enviroment.EnviromentTypeId });
        }

    }
}
