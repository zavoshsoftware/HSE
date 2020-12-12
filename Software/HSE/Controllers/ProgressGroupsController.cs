using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using ViewModels;

namespace HSE.Controllers
{
    public class ProgressGroupsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: ProgressGroups
        public ActionResult Index()
        {
            return View(db.ProgressGroups.Where(a => a.IsDeleted == false).OrderByDescending(a => a.CreationDate).ToList());
        }

        // GET: ProgressGroups/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgressGroup progressGroup = db.ProgressGroups.Find(id);
            if (progressGroup == null)
            {
                return HttpNotFound();
            }
            return View(progressGroup);
        }

        // GET: ProgressGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProgressGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProgressGroup progressGroup)
        {
            if (ModelState.IsValid)
            {
                progressGroup.IsDeleted = false;
                progressGroup.CreationDate = DateTime.Now;
                progressGroup.Id = Guid.NewGuid();
                db.ProgressGroups.Add(progressGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(progressGroup);
        }

        // GET: ProgressGroups/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgressGroup progressGroup = db.ProgressGroups.Find(id);
            if (progressGroup == null)
            {
                return HttpNotFound();
            }
            return View(progressGroup);
        }

        // POST: ProgressGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProgressGroup progressGroup)
        {
            if (ModelState.IsValid)
            {
                progressGroup.IsDeleted = false;
                progressGroup.LastModifiedDate = DateTime.Now;
                db.Entry(progressGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(progressGroup);
        }

        // GET: ProgressGroups/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgressGroup progressGroup = db.ProgressGroups.Find(id);
            if (progressGroup == null)
            {
                return HttpNotFound();
            }
            return View(progressGroup);
        }

        // POST: ProgressGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProgressGroup progressGroup = db.ProgressGroups.Find(id);
            progressGroup.IsDeleted = true;
            progressGroup.DeletionDate = DateTime.Now;

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

        public ActionResult ProgressGroupList(Guid? companyId)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string id = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            Guid userId = new Guid(id);

            ViewBag.roleName = roleName;

            if (roleName == "company")
            {
                var user = db.Users.Find(userId);
                companyId = user.CompanyId;
            }
            if (roleName == "Administrator")
            {
            }

            if (roleName == "supervisor")
            {
            }

            if (companyId == null)
                return Redirect("/home/dashboard");

            ViewBag.CompanyId = companyId;
            var progressGroups = db.ProgressGroups.Where(a => a.IsDeleted == false)
                .OrderByDescending(a => a.CreationDate).Select(c => new { c.Id, c.Title }).ToList();

            List<ProgressGroupViewModel> result = new List<ProgressGroupViewModel>();

            foreach (var progressGroup in progressGroups)
            {
                var progresses = db.Progresses
                    .Where(c => c.ProgressGroupId == progressGroup.Id && c.CompanyId == companyId)
                    .OrderByDescending(c => c.CreationDate).FirstOrDefault();

                string tot = "0";

                if (progresses != null)
                    tot = progresses.Total.ToString("0");
                result.Add(new ProgressGroupViewModel()
                {
                    Id = progressGroup.Id,
                    Title = progressGroup.Title,
                    TotalPercent = tot
                });

            }

            return View(result);
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
            ViewBag.baseUrl = "ProgressGroups";

            ViewBag.Title = "گزارشات پیشرفت WBS";

            return View(companyTypes);
        }
    }
}
