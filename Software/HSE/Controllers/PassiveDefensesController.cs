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
    public class PassiveDefensesController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index(Guid id)
        {
            var passiveDefenses = db.PassiveDefenses.Include(p => p.Company)
                .Where(p => p.PassiveDefenseTypeId == id && p.IsDeleted == false).OrderByDescending(p => p.CreationDate)
                .Include(p => p.PassiveDefenseType);

            PassiveDefenseType passiveDefenseType = db.PassiveDefenseTypes.Find(id);

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;
            ViewBag.roleName = roleName;

            if (passiveDefenseType != null)
                ViewBag.typeTitle = passiveDefenseType.Title;

            return View(passiveDefenses.ToList());
        }

        // GET: PassiveDefenses/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PassiveDefense passiveDefense = db.PassiveDefenses.Find(id);
            if (passiveDefense == null)
            {
                return HttpNotFound();
            }
            return View(passiveDefense);
        }

        // GET: PassiveDefenses/Create
        public ActionResult Create(Guid id)
        {
            ViewBag.CompanyId = new SelectList(db.Companies.Where(c=>c.IsActive), "Id", "Title");
            ViewBag.PassiveDefenseTypeId = id;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PassiveDefense passiveDefense, Guid id, HttpPostedFileBase fileupload)
        {

            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/pd/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    passiveDefense.FileUrl = newFilenameUrl;
                }
                #endregion

                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                string uId = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value; 
               
                Guid userId = new Guid(uId);
                User user = db.Users.Find(userId);

                if (user != null)
                    passiveDefense.CompanyId = user.CompanyId;

                passiveDefense.IsActive = true;
                passiveDefense.PassiveDefenseTypeId = id;
                passiveDefense.IsDeleted = false;
                passiveDefense.CreationDate = DateTime.Now;
                passiveDefense.Id = Guid.NewGuid();
                db.PassiveDefenses.Add(passiveDefense);
                db.SaveChanges();

                Company co = db.Companies.Find(passiveDefense.CompanyId);
                Helpers.NotificationHelper.InsertNotification(co.Title, "/PassiveDefenses/index/" + passiveDefense.PassiveDefenseTypeId, "پدافند غیرعامل");

                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.IsActive), "Id", "Title"); 
            ViewBag.PassiveDefenseTypeId =  passiveDefense.PassiveDefenseTypeId ;
            return View(passiveDefense);
        }

        // GET: PassiveDefenses/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PassiveDefense passiveDefense = db.PassiveDefenses.Find(id);
            if (passiveDefense == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.IsActive), "Id", "Title",passiveDefense.CompanyId); 

            ViewBag.PassiveDefenseTypeId =  passiveDefense.PassiveDefenseTypeId;
            return View(passiveDefense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PassiveDefense passiveDefense, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/pd/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    passiveDefense.FileUrl = newFilenameUrl;
                }
                #endregion

                passiveDefense.IsDeleted = false;
                passiveDefense.LastModifiedDate = DateTime.Now;
                db.Entry(passiveDefense).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = passiveDefense.PassiveDefenseTypeId });
            }
            ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.IsActive), "Id", "Title", passiveDefense.CompanyId);

            ViewBag.PassiveDefenseTypeId = passiveDefense.PassiveDefenseTypeId;
            return View(passiveDefense);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PassiveDefense passiveDefense = db.PassiveDefenses.Find(id);
            if (passiveDefense == null)
            {
                return HttpNotFound();
            }
            ViewBag.PassiveDefenseTypeId = passiveDefense.PassiveDefenseTypeId;

            return View(passiveDefense);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PassiveDefense passiveDefense = db.PassiveDefenses.Find(id);
            passiveDefense.IsDeleted = true;
            passiveDefense.DeletionDate = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index", new { id = passiveDefense.PassiveDefenseTypeId });
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
            PassiveDefense passiveDefense = db.PassiveDefenses.Include(c=>c.Company).FirstOrDefault(c=>c.Id==id);
            if (passiveDefense == null)
            {
                return HttpNotFound();
            }
            return View(passiveDefense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SupervisorComment(PassiveDefense passiveDefense)
        {
            if (ModelState.IsValid)
            {

                passiveDefense.IsDeleted = false;
                passiveDefense.LastModifiedDate = DateTime.Now;
                db.Entry(passiveDefense).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("index", new { id = passiveDefense.PassiveDefenseTypeId });
            }
            return View(passiveDefense);
        }

    }
}
