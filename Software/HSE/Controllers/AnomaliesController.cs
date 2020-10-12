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
    [Authorize(Roles = "Administrator,company,supervisor")]
    public class AnomaliesController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string id = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            Guid userId = new Guid(id);

            ViewBag.roleName = roleName;
            List<Anomaly> anomalies = new List<Anomaly>();


            if (roleName == "Administrator")
            {
                anomalies = db.Anomalies.Where(a => a.IsDeleted == false)
                    .OrderByDescending(a => a.CreationDate).ToList();
            }

            if (roleName == "company")
            {
                anomalies = db.Anomalies.Where(a => a.IsDeleted == false && a.ResponseUserId == userId)
                    .OrderByDescending(a => a.CreationDate).ToList();
            }
            if (roleName == "supervisor")
            {
                User user = db.Users.FirstOrDefault(current => current.Id == userId);

                if (user != null)
                {
                    anomalies = db.Anomalies.Where(a => a.IsDeleted == false && a.CreatorUserId == user.Id)
                        .OrderByDescending(a => a.CreationDate).ToList();
                }
            }
            return View(anomalies);
        }
        public ActionResult IndexAdmin(Guid id)
        {
            List<Anomaly> anomalies = new List<Anomaly>();

            anomalies = db.Anomalies.Where(a => a.CompanyId == id && a.IsDeleted == false)
                .OrderByDescending(a => a.CreationDate).ToList();

            return View(anomalies);
        }

        public ActionResult CompanyTypeList()
        {
            List<CompanyType> companyTypes = db.CompanyTypes.Where(c => c.IsDeleted == false && c.IsActive).ToList();
            ViewBag.baseUrl = "Anomalies";

            return View(companyTypes);
        }


        public ActionResult List(Guid? id)
        {
            List<Company> companies = db.Companies.Where(c => c.CompanyTypeId == id && c.IsDeleted == false && c.IsActive == true).ToList();

            return View(companies);
        }
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Anomaly anomaly = db.Anomalies.Find(id);
            if (anomaly == null)
            {
                return HttpNotFound();
            }

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;
            if (roleName == "company")
            {
                ViewBag.AnomalyResultId = new SelectList(db.AnomalyResults, "Id", "Title", anomaly.AnomalyResultId);
            }
            if (roleName == "Administrator")
            {
                ViewBag.companyId = anomaly.CompanyId;
            }
            return View(anomaly);
        }

        [HttpPost]
        public ActionResult Details(Anomaly anomaly)
        {

            if (ModelState.IsValid)
            {
                anomaly.IsDeleted = false;
                anomaly.StatusId = db.Status.OrderBy(c => c.Order).Skip(1).FirstOrDefault().Id;
                anomaly.LastModifiedDate = DateTime.Now;
                db.Entry(anomaly).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(anomaly);
        }


        public ActionResult DetailsForSup(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Anomaly anomaly = db.Anomalies.Find(id);
            if (anomaly == null)
            {
                return HttpNotFound();
            }

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;
            if (roleName == "supervisor")
            {
                ViewBag.StatusId = new SelectList(db.Status.Where(c => c.Order == 3 || c.Order == 4), "Id", "Title", anomaly.AnomalyResultId);
            }
            return View(anomaly);
        }



        [HttpPost]
        public ActionResult DetailsForSup(Anomaly anomaly)
        {

            if (ModelState.IsValid)
            {
                anomaly.IsDeleted = false;
                anomaly.LastModifiedDate = DateTime.Now;
                db.Entry(anomaly).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(anomaly);
        }


        // GET: Anomalies/Create
        public ActionResult Create()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;
            string id = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

            Guid userId = new Guid(id);
            ViewBag.role = roleName;
            User user = db.Users.Find(userId);
            if (roleName == "company")
            {
                if (user != null)
                {
                    ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title", user.CompanyId);


                }
                else
                {
                    ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title");
                }
            }
            if (roleName == "supervisor")
            {
                ViewBag.CompanyId = new SelectList(db.Companies.Where(c=>c.SupervisorUserId==userId), "Id", "Title");
            }
            else
            {
                ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title");
            }
            ViewBag.AnomalyHseId = new SelectList(db.AnomalyHses, "Id", "Title");
            ViewBag.AnomalyLevelId = new SelectList(db.AnomalyLevels, "Id", "Title");
            ViewBag.AnomalyResultId = new SelectList(db.AnomalyResults, "Id", "Title");

            return View();
        }

        // POST: Anomalies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Anomaly anomaly, HttpPostedFileBase fileupload)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string id = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
            Guid userId = new Guid(id);
            User user = db.Users.Find(userId);
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/anomaly/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    anomaly.ImageUrl = newFilenameUrl;
                }
                #endregion




                if (roleName == "company" && user.CompanyId != null)
                    anomaly.CompanyId = user.CompanyId.Value;

                anomaly.CreatorUserId = userId;
                anomaly.AnomalyResultId = new Guid("0EDEBF8C-622B-4816-8F0B-FF06C676F37B");
                anomaly.StatusId = db.Status.OrderBy(c => c.Order).FirstOrDefault().Id;
                anomaly.IsDeleted = false;
                anomaly.CreationDate = DateTime.Now;
                anomaly.Id = Guid.NewGuid();
                db.Anomalies.Add(anomaly);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AnomalyHseId = new SelectList(db.AnomalyHses, "Id", "Title");
            ViewBag.AnomalyLevelId = new SelectList(db.AnomalyLevels, "Id", "Title");
            ViewBag.AnomalyResultId = new SelectList(db.AnomalyResults, "Id", "Title");

            if (roleName == "company")
            {

                if (user != null)
                {
                    ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title", user.CompanyId);
                }
                else
                {
                    ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title");
                }
            }
            if (roleName == "supervisor")
            {
                ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.SupervisorUserId == userId), "Id", "Title");
            }
            else
            {
                ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title");
            }

            return View(anomaly);
        }

        // GET: Anomalies/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Anomaly anomaly = db.Anomalies.Find(id);
            if (anomaly == null)
            {
                return HttpNotFound();
            }

            return View(anomaly);
        }

        // POST: Anomalies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Anomaly anomaly)
        {
            if (ModelState.IsValid)
            {
                anomaly.IsDeleted = false;
                anomaly.LastModifiedDate = DateTime.Now;
                db.Entry(anomaly).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(anomaly);
        }

        // GET: Anomalies/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Anomaly anomaly = db.Anomalies.Find(id);
            if (anomaly == null)
            {
                return HttpNotFound();
            }
            return View(anomaly);
        }

        // POST: Anomalies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Anomaly anomaly = db.Anomalies.Find(id);
            anomaly.IsDeleted = true;
            anomaly.DeletionDate = DateTime.Now;

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


        [AllowAnonymous]
        public ActionResult FillResponseUser(string id)
        {
            Guid companyId = new Guid(id);

            List<ResponseUserViewModel> companyItems = new List<ResponseUserViewModel>();

            List<User> users = db.Users.Where(c => c.CompanyId == companyId && c.IsDeleted == false).ToList();
            foreach (User user in users)
            {
                companyItems.Add(new ResponseUserViewModel()
                {
                    Title = user.FullName,
                    Value = user.Id.ToString()
                });
            }

            return Json(companyItems, JsonRequestBehavior.AllowGet);
        }

    }
}
