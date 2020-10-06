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
    public class HseDocumentsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();


        public ActionResult List()
        {
            List<Company> companies = db.Companies.Where(c => c.IsDeleted == false && c.IsActive).ToList();

            return View(companies);
        }

        public ActionResult Index(Guid id)
        {
            List<HseDocument> hsePlans = db.HseDocuments.Include(h => h.Company)
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

            List<HseDocument> hsePlans = new List<HseDocument>();

            if (user != null)
            {
                hsePlans = db.HseDocuments.Include(h => h.Company)
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


        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HseDocument hseDocument = db.HseDocuments.Find(id);
            if (hseDocument == null)
            {
                return HttpNotFound();
            }
            return View(hseDocument);
        }

        // GET: HseDocuments/Create
        public ActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title");
            ViewBag.HseDocumentTypeId = new SelectList(db.HseDocumentTypes, "Id", "Title");
            return View();
        }

        // POST: HseDocuments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HseDocument hseDocument, HttpPostedFileBase fileupload)
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

                    hseDocument.FileUrl = newFilenameUrl;
                }
                #endregion


                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                string id = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

                hseDocument.IsActive = true;
                hseDocument.UserId = new Guid(id);
                hseDocument.IsDeleted=false;
				hseDocument.CreationDate= DateTime.Now; 
                hseDocument.Id = Guid.NewGuid();
                db.HseDocuments.Add(hseDocument);
                db.SaveChanges();
                return RedirectToAction("Index",new{id=hseDocument.CompanyId});
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title", hseDocument.CompanyId);
            ViewBag.HseDocumentTypeId = new SelectList(db.HseDocumentTypes, "Id", "Title", hseDocument.HseDocumentTypeId);
            return View(hseDocument);
        }

        // GET: HseDocuments/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HseDocument hseDocument = db.HseDocuments.Find(id);
            if (hseDocument == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title", hseDocument.CompanyId);
            ViewBag.HseDocumentTypeId = new SelectList(db.HseDocumentTypes, "Id", "Title", hseDocument.HseDocumentTypeId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", hseDocument.UserId);
            return View(hseDocument);
        }

        // POST: HseDocuments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HseDocument hseDocument, HttpPostedFileBase fileupload)
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

                    hseDocument.FileUrl = newFilenameUrl;
                }
                #endregion

                hseDocument.IsDeleted = false;
				hseDocument.LastModifiedDate = DateTime.Now;
                db.Entry(hseDocument).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = hseDocument.CompanyId });
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title", hseDocument.CompanyId);
            ViewBag.HseDocumentTypeId = new SelectList(db.HseDocumentTypes, "Id", "Title", hseDocument.HseDocumentTypeId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", hseDocument.UserId);
            return View(hseDocument);
        }

        // GET: HseDocuments/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HseDocument hseDocument = db.HseDocuments.Find(id);
            if (hseDocument == null)
            {
                return HttpNotFound();
            }
            return View(hseDocument);
        }

        // POST: HseDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            HseDocument hseDocument = db.HseDocuments.Find(id);
			hseDocument.IsDeleted=true;
			hseDocument.DeletionDate=DateTime.Now;
 
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
