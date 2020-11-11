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
using ViewModels;

namespace HSE.Controllers
{
    public class PermitsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

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
                {

                    companies = db.Companies.Where(c => c.CompanyTypeId == id && c.IsDeleted == false && c.IsActive)
                        .ToList();
                }
            }


            return View(companies.OrderBy(c => c.Title).ToList());
        }

        public ActionResult CompanyTypeList()
        {
            List<CompanyType> companyTypes = db.CompanyTypes.Where(c => c.IsDeleted == false && c.IsActive).ToList();

            ViewBag.baseUrl = "permits";
            ViewBag.Title = "پرمیت";

            return View(companyTypes);
        }

        public ActionResult PermitTypeList(Guid? id)
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

            List<PermitType> permitTypes = db.PermitTypes.Where(c => c.IsDeleted == false && c.IsActive).ToList();
            List<PermitTypeListViewModel> result=new List<PermitTypeListViewModel>();

            foreach (PermitType permitType in permitTypes)
            {
                result.Add(new PermitTypeListViewModel()
                {
                    PermitType = permitType,
                    Qty = db.Permits.Count(c =>
                        c.PermitTypeId == permitType.Id && c.CompanyId == companyId.Value && c.IsDeleted == false &&
                        c.PermitStatus.Code > 0)
                });
            }
            ViewBag.id = id;

            return View(result);
        }
        public ActionResult Index(Guid? id, Guid permitTypeId)
        {
            ViewBag.permitTypeId = permitTypeId;

            PermitType permitType = db.PermitTypes.Find(permitTypeId);
            if (permitType != null)
                ViewBag.fileUrl = permitType.FileUrl;

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
             
            var permits = db.Permits.Include(p => p.Company).Include(c => c.PermitType)
                .Where(p => p.CompanyId == companyId && p.PermitTypeId == permitTypeId && p.IsDeleted == false)
                .OrderByDescending(p => p.PermitStatusId).Include(p => p.PermitStatus);

            return View(permits.ToList());
        }



        // GET: Permits/Details/5
        public ActionResult Details(Guid? id)
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

        // GET: Permits/Create
        public ActionResult Create(Guid id)
        {
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title");
            ViewBag.PermitStatusId = new SelectList(db.PermitStatuses, "Id", "Title");
            ViewBag.PermitTypeId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Permit permit, Guid id, HttpPostedFileBase fileupload)
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
                }
                #endregion
                 
                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                string uid = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
                string roleTitle = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

                ViewBag.roleTitle = roleTitle;
                Guid userId = new Guid(uid);

                User user = db.Users.Find(userId);

                Guid companyId = user.CompanyId.Value;


                permit.PermitTypeId = id;
                permit.PermitStatusId = db.PermitStatuses.FirstOrDefault(c => c.Code == 1).Id;
                permit.CompanyId = companyId;
                permit.IsActive = true;

                permit.IsDeleted=false;
				permit.CreationDate= DateTime.Now; 
                permit.Id = Guid.NewGuid();
                db.Permits.Add(permit);
                db.SaveChanges();
                return RedirectToAction("Index",new{ permitTypeId =id});
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title", permit.CompanyId);
            ViewBag.PermitStatusId = new SelectList(db.PermitStatuses, "Id", "Title", permit.PermitStatusId);
            ViewBag.PermitTypeId = id;
            return View(permit);
        }

        // GET: Permits/Edit/5
        public ActionResult Edit(Guid? id)
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
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title", permit.CompanyId);
            ViewBag.PermitStatusId = new SelectList(db.PermitStatuses, "Id", "Title", permit.PermitStatusId);
            ViewBag.PermitTypeId = permit.PermitTypeId;
            return View(permit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Permit permit, HttpPostedFileBase fileupload)
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
                }
                #endregion

                permit.IsDeleted = false;
				permit.LastModifiedDate = DateTime.Now;
                db.Entry(permit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new { PermitTypeId= permit.PermitTypeId });
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title", permit.CompanyId);
            ViewBag.PermitStatusId = new SelectList(db.PermitStatuses, "Id", "Title", permit.PermitStatusId);
            ViewBag.PermitTypeId =   permit.PermitTypeId ;
            return View(permit);
        }

        // GET: Permits/Delete/5
        public ActionResult Delete(Guid? id)
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
            ViewBag.PermitTypeId = permit.PermitTypeId;

            return View(permit);
        }

        // POST: Permits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Permit permit = db.Permits.Find(id);
			permit.IsDeleted=true;
			permit.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("Index", new { PermitTypeId = permit.PermitTypeId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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

            ViewBag.PermitStatusId = new SelectList(db.PermitStatuses, "Id", "Title", permit.PermitStatusId);

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
                return RedirectToAction("Index", new { id = permit.CompanyId,permittypeid=permit.PermitTypeId });
            }

            return View(permit);
        }


        public string DeletUnusedPermits()
        {
            List<Permit> permits = db.Permits.Where(c => c.PermitStatus.Code < 1&&c.IsDeleted==false).ToList();

            foreach (Permit permit in permits)
            {
                db.Permits.Remove(permit);
            }

            db.SaveChanges();
            return string.Empty;
        }
    }
}
