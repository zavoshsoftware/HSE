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

    public class ContractRquirmentsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: ContractRquirments
        public ActionResult Index()
        {

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string id = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            Guid userId = new Guid(id);

            ViewBag.roleName = roleName;

            List<ContractRquirment> contractRquirments = new List<ContractRquirment>();

            if (roleName == "Administrator")
            {
                contractRquirments = db.ContractRquirments.Include(c => c.Company).Where(c => c.IsDeleted == false)
                    .OrderByDescending(c => c.CreationDate).ToList();
            }

            if (roleName == "company")
            {
                User user = db.Users.FirstOrDefault(c => c.Id == userId);

                contractRquirments = db.ContractRquirments.Include(c => c.Company)
                    .Where(c => c.IsDeleted == false && c.CompanyId == user.CompanyId)
                    .OrderByDescending(c => c.CreationDate).ToList();
            }

            return View(contractRquirments);
        }

        // GET: ContractRquirments/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractRquirment contractRquirment = db.ContractRquirments.Find(id);
            if (contractRquirment == null)
            {
                return HttpNotFound();
            }
            return View(contractRquirment);
        }

        // GET: ContractRquirments/Create
        public ActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title");
            return View();
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContractRquirment contractRquirment, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/contractRquirment/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    contractRquirment.FileUrl = newFilenameUrl;
                }
                #endregion

                contractRquirment.IsDeleted=false;
				contractRquirment.CreationDate= DateTime.Now; 
                contractRquirment.Id = Guid.NewGuid();
                db.ContractRquirments.Add(contractRquirment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title", contractRquirment.CompanyId);
            return View(contractRquirment);
        }

        // GET: ContractRquirments/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractRquirment contractRquirment = db.ContractRquirments.Find(id);
            if (contractRquirment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title", contractRquirment.CompanyId);
            return View(contractRquirment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContractRquirment contractRquirment, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/contractRquirment/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    contractRquirment.FileUrl = newFilenameUrl;
                }
                #endregion

                contractRquirment.IsDeleted = false;
				contractRquirment.LastModifiedDate = DateTime.Now;
                db.Entry(contractRquirment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title", contractRquirment.CompanyId);
            return View(contractRquirment);
        }

        // GET: ContractRquirments/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractRquirment contractRquirment = db.ContractRquirments.Find(id);
            if (contractRquirment == null)
            {
                return HttpNotFound();
            }
            return View(contractRquirment);
        }

        // POST: ContractRquirments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ContractRquirment contractRquirment = db.ContractRquirments.Find(id);
			contractRquirment.IsDeleted=true;
			contractRquirment.DeletionDate=DateTime.Now;
 
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
