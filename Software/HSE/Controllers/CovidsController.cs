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
    public class CovidsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        [Authorize(Roles = "Administrator,supervisor,company")]
        public ActionResult Index(Guid? id)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            if (id != null && (roleName == "Administrator" || roleName == "supervisor"))
                return RedirectToAction("IndexAdmin", new { id = id });


            List<Covid> covids = new List<Covid>();


            string uid = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

            Guid userId = new Guid(uid);

            User user = db.Users.FirstOrDefault(c => c.Id == userId && c.IsDeleted == false && c.IsActive);

            if (user != null)
            {
                covids = db.Covids.Include(c => c.Company)
                    .Where(c => c.CompanyId == user.CompanyId && c.IsDeleted == false).OrderByDescending(c => c.CreationDate).ToList();
            }
            return View(covids);
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

            return View(companies.OrderBy(c => c.Title).ToList());
        }

        [Authorize(Roles = "Administrator,supervisor")]
        public ActionResult IndexAdmin(Guid id)
        {
            var covids = db.Covids.Include(c => c.Company)
                .Where(c => c.CompanyId == id && c.IsDeleted == false).OrderByDescending(c => c.CreationDate);



            Company company = db.Companies.Find(id);

            if (company != null)
            {
                ViewBag.Title = "فهرست افراد درگیر کرونا " + company.Title;
            }
            return View(covids.ToList());
        }
        public ActionResult CompanyTypeList()
        {
            List<CompanyType> companyTypes = db.CompanyTypes.Where(c => c.IsDeleted == false && c.IsActive).ToList();
            ViewBag.baseUrl = "Covids";

            ViewBag.Title = "افراد درگیر کرونا";
            return View(companyTypes);
        }
        // GET: Covids/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Covid covid = db.Covids.Find(id);
            if (covid == null)
            {
                return HttpNotFound();
            }
            return View(covid);
        }

        // GET: Covids/Create
        public ActionResult Create()
        {
            ViewBag.CovidStatusId = new SelectList(db.CovidStatus, "Id", "Title");
            ViewBag.CovidTypeId = new SelectList(db.CovidTypes, "Id", "Title");


            Guid? companyId = null;
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;
             
             if (roleName == "company")
            {
                string uId = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

                Guid userId = new Guid(uId);

                User user = db.Users.Find(userId);

                companyId = user.CompanyId;
            }

            ViewBag.CompanyId = companyId.Value;
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Covid covid, HttpPostedFileBase fileupload)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;


            string uId = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

            Guid userId = new Guid(uId);

            User user = db.Users.Find(userId);

            Guid companyId = user.CompanyId.Value;

            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/covid/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    covid.ImageUrl = newFilenameUrl;
                }
                #endregion

                covid.IsActive = true;
                covid.CompanyId = companyId;
				covid.IsDeleted=false;
				covid.CreationDate= DateTime.Now; 
                covid.Id = Guid.NewGuid();
                db.Covids.Add(covid);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

           

            ViewBag.CompanyId = companyId;
            ViewBag.CovidStatusId = new SelectList(db.CovidStatus, "Id", "Title", covid.CovidStatusId);
            ViewBag.CovidTypeId = new SelectList(db.CovidTypes, "Id", "Title", covid.CovidTypeId);
            return View(covid);
        }

        // GET: Covids/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Covid covid = db.Covids.Find(id);
            if (covid == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId =  covid.CompanyId ;
            ViewBag.CovidStatusId = new SelectList(db.CovidStatus, "Id", "Title", covid.CovidStatusId);
            ViewBag.CovidTypeId = new SelectList(db.CovidTypes, "Id", "Title", covid.CovidTypeId);
            return View(covid);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Covid covid, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/covid/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    covid.ImageUrl = newFilenameUrl;
                }
                #endregion

                covid.IsDeleted = false;
				covid.LastModifiedDate = DateTime.Now;
                db.Entry(covid).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = covid.CompanyId;

            ViewBag.CovidStatusId = new SelectList(db.CovidStatus, "Id", "Title", covid.CovidStatusId);
            ViewBag.CovidTypeId = new SelectList(db.CovidTypes, "Id", "Title", covid.CovidTypeId);
            return View(covid);
        }

        // GET: Covids/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Covid covid = db.Covids.Find(id);
            if (covid == null)
            {
                return HttpNotFound();
            }
            return View(covid);
        }

        // POST: Covids/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Covid covid = db.Covids.Find(id);
			covid.IsDeleted=true;
			covid.DeletionDate=DateTime.Now;
 
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
