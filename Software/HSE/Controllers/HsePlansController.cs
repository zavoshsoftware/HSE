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
    public class HsePlansController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: HsePlans

        public ActionResult List()
        {
            List<Company> companies = db.Companies.Where(c => c.IsDeleted == false && c.IsActive == true).ToList();

            return View(companies);
        }

        public ActionResult Index(Guid id)
        {
            List<HsePlan> hsePlans = db.HsePlans.Include(h => h.Company)
                .Where(h => h.CompanyId == id && h.IsDeleted == false)
                .OrderByDescending(h => h.CreationDate).Include(h => h.User).ToList();

            return View(hsePlans.ToList());
        }

        public ActionResult IndexCompany()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string id = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            Guid userId = new Guid(id);

            User user = db.Users.FirstOrDefault(c => c.Id == userId);

            //CompanyUser companyUser = db.CompanyUsers.FirstOrDefault(c => c.UserId == userId);


            List<HsePlan> hsePlans = new List<HsePlan>();

            if (user != null)
            {
                hsePlans = db.HsePlans.Include(h => h.Company)
                    .Where(h => h.CompanyId == user.CompanyId && h.IsDeleted == false)
                    .OrderByDescending(h => h.CreationDate).Include(h => h.User).ToList();

            }
            return View(hsePlans);
        }

        public ActionResult ListSupervisor()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string id = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;


            Guid userId = new Guid(id);

            List<Company> companies = db.Companies
                .Where(c => c.SupervisorUserId == userId && c.IsDeleted == false && c.IsActive).ToList();
            
            return View(companies);
        }


        // GET: HsePlans/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HsePlan hsePlan = db.HsePlans.Find(id);
            if (hsePlan == null)
            {
                return HttpNotFound();
            }
            return View(hsePlan);
        }

        // GET: HsePlans/Create
        public ActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title");
            
            return View();
        }

        // POST: HsePlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HsePlan hsePlan, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/hsePlans/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    hsePlan.FileUrl = newFilenameUrl;
                }
                #endregion


                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                string id = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

                hsePlan.IsActive = true;
                hsePlan.UserId = new Guid(id);
                hsePlan.IsDeleted=false;
				hsePlan.CreationDate= DateTime.Now; 
                hsePlan.Id = Guid.NewGuid();
                db.HsePlans.Add(hsePlan);
                db.SaveChanges();
                return RedirectToAction("IndexCompany");
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title", hsePlan.CompanyId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", hsePlan.UserId);
            return View(hsePlan);
        }

        // GET: HsePlans/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HsePlan hsePlan = db.HsePlans.Find(id);
            if (hsePlan == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title", hsePlan.CompanyId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", hsePlan.UserId);
            return View(hsePlan);
        }

        // POST: HsePlans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HsePlan hsePlan, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/hsePlans/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    hsePlan.FileUrl = newFilenameUrl;
                }
                #endregion

                hsePlan.IsDeleted = false;
				hsePlan.LastModifiedDate = DateTime.Now;
                db.Entry(hsePlan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexCompany");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title", hsePlan.CompanyId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", hsePlan.UserId);
            return View(hsePlan);
        }

        public ActionResult SupervisorComment(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HsePlan hsePlan = db.HsePlans.Find(id);
            if (hsePlan == null)
            {
                return HttpNotFound();
            }
            return View(hsePlan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SupervisorComment(HsePlan hsePlan)
        {
            if (ModelState.IsValid)
            {
               
                hsePlan.IsDeleted = false;
                hsePlan.LastModifiedDate = DateTime.Now;
                db.Entry(hsePlan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new{id=hsePlan.CompanyId});
            }
            return View(hsePlan);
        }


        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HsePlan hsePlan = db.HsePlans.Find(id);
            if (hsePlan == null)
            {
                return HttpNotFound();
            }
            return View(hsePlan);
        }

        // POST: HsePlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            HsePlan hsePlan = db.HsePlans.Find(id);
			hsePlan.IsDeleted=true;
			hsePlan.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("IndexCompany");
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
