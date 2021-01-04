using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Models;

namespace HSE.Controllers
{
    public class EquipmentsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult CompanyTypeList()
        {
            List<CompanyType> companyTypes = db.CompanyTypes.Where(c => c.IsDeleted == false && c.IsActive).ToList();
            ViewBag.baseUrl = "Equipments";
            ViewBag.Title = "تجهیزات و ماشین آلات";

            return View(companyTypes);
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
                    companies = db.Companies.Where(c => c.IsDeleted == false && c.IsActive ).ToList();
                else
                    companies = db.Companies.Where(c => c.CompanyTypeId == id && c.IsDeleted == false && c.IsActive ).ToList();
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
            var equipments = db.Equipments.Include(c => c.Company)
                .Where(c => c.CompanyId == id&&c.IsActive && c.IsDeleted == false).OrderByDescending(c => c.CreationDate);

            return View(equipments.ToList());
        }

        [Authorize(Roles = "Administrator,supervisor,company")]
        public ActionResult Index(Guid? id)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            if (id != null && (roleName == "Administrator" || roleName == "supervisor"))
                return RedirectToAction("IndexAdmin", new { id = id });


            List<Equipment> equipments = new List<Equipment>();


            string uid = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

            Guid userId = new Guid(uid);

            User user = db.Users.FirstOrDefault(c => c.Id == userId && c.IsDeleted == false && c.IsActive);

            if (user != null)
            {
                equipments = db.Equipments.Include(c => c.Company)
                    .Where(c => c.CompanyId == user.CompanyId && c.IsDeleted == false)
                    .OrderByDescending(c => c.CreationDate).ToList();

            }
            return View(equipments);
        }

        public Guid? GetOnlineUserCompanyId()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

            string uid = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

            Guid userId = new Guid(uid);

            User user = db.Users.FirstOrDefault(c => c.Id == userId && c.IsDeleted == false && c.IsActive);

            return user.CompanyId;
        }
        public ActionResult Create()
        {
            ViewBag.CompanyId = GetOnlineUserCompanyId();
            ViewBag.EquipmentTypeId = new SelectList(db.EquipmentTypes, "Id", "Title");

            return View();
        }

        // POST: Equipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Equipment equipment, HttpPostedFileBase fileupload)
        {
            Guid? companyId = GetOnlineUserCompanyId();
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/equipment/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    equipment.CertificateFileUrl = newFilenameUrl;
                }
                #endregion


                equipment.CompanyId = companyId.Value;
                equipment.IsDeleted = false;
                equipment.CreationDate = DateTime.Now;
                equipment.Id = Guid.NewGuid();
                db.Equipments.Add(equipment);
                db.SaveChanges();

                Company co = db.Companies.Find(companyId);
                Helpers.NotificationHelper.InsertNotification(co.Title, "/Equipments/IndexAdmin/" + companyId, "ماشین آلات و تجهیزات");
                Helpers.NotificationHelper.InsertNotificationForSup(companyId, co.Title, "/Equipments/IndexAdmin/" + companyId, "ماشین آلات و تجهیزات");

                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = companyId;
            ViewBag.EquipmentTypeId = new SelectList(db.EquipmentTypes, "Id", "Title");
            return View(equipment);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = db.Equipments.Find(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            ViewBag.EquipmentTypeId = new SelectList(db.EquipmentTypes, "Id", "Title",equipment.EquipmentTypeId);
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title", equipment.CompanyId);
            return View(equipment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Equipment equipment, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/equipment/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    equipment.CertificateFileUrl = newFilenameUrl;
                }
                #endregion
                 
                equipment.IsDeleted = false;
                equipment.LastModifiedDate = DateTime.Now;
                db.Entry(equipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EquipmentTypeId = new SelectList(db.EquipmentTypes, "Id", "Title",equipment.EquipmentTypeId);
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title", equipment.CompanyId);
            return View(equipment);
        }

        // GET: Equipments/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = db.Equipments.Find(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            return View(equipment);
        }

        // POST: Equipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Equipment equipment = db.Equipments.Find(id);
            equipment.IsDeleted = true;
            equipment.DeletionDate = DateTime.Now;

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


     
        public ActionResult SupervisorComment(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = db.Equipments.Find(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            return View(equipment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SupervisorComment(Equipment equipment)
        {
            if (ModelState.IsValid)
            {

                equipment.IsDeleted = false;
                equipment.LastModifiedDate = DateTime.Now;
                db.Entry(equipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexAdmin", new { id = equipment.CompanyId });
            }
            return View(equipment);
        }

    }
}
