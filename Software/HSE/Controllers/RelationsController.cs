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
    public class RelationsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();
   

        public ActionResult Index(Guid id)
        {

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            string uid = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

            Guid userId = new Guid(uid);

            ViewBag.role = roleName;
            List<Relation> relations;

            User user = db.Users.Find(userId);

            if (roleName == "company")
                relations = db.Relations.Include(r => r.RelationType)
                    .Where(r => r.CompanyId == user.CompanyId && r.RelationTypeId == id && r.IsDeleted == false)
                    .OrderByDescending(r => r.CreationDate)
                    .ToList();

            else
                relations = db.Relations.Include(r => r.RelationType)
                    .Where(r => r.RelationTypeId == id && r.IsDeleted == false)
                    .OrderByDescending(r => r.CreationDate)
                    .ToList();

            RelationType relationType = db.RelationTypes.FirstOrDefault(c => c.Id == id);

            if (relationType != null)
                ViewBag.Title = "ارتباطات " + relationType.Title;
            return View(relations);
        }
         
        public ActionResult Create(Guid id)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            string uid = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

            Guid userId = new Guid(uid);
            User user = db.Users.Find(userId);

            ViewBag.role = roleName;
            ViewBag.CompanyId = new SelectList(db.Companies.Where(c=>c.IsDeleted==false&&c.IsActive), "Id", "Title", user.CompanyId);
            ViewBag.RelationTypeId = id;
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Relation relation, Guid id, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/relation/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    relation.FileUrl = newFilenameUrl;
                }
                #endregion

                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

                string uid = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

                Guid userId = new Guid(uid);
                User user = db.Users.Find(userId);
                if(user!=null)
                relation.CompanyId = user.CompanyId;
                relation.RelationTypeId = id;
				relation.IsDeleted=false;
				relation.CreationDate= DateTime.Now; 
                relation.Id = Guid.NewGuid();
                db.Relations.Add(relation);
                db.SaveChanges();
                return RedirectToAction("Index",new{id=id});
            }

            ViewBag.CompanyId = new SelectList(db.Companies.Where(c=>c.IsDeleted==false&&c.IsActive), "Id", "Title");
            ViewBag.RelationTypeId = id;
            return View(relation);
        }
         
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Relation relation = db.Relations.Find(id);
            if (relation == null)
            {
                return HttpNotFound();
            }
            ViewBag.RelationTypeId = relation.RelationTypeId;
            ViewBag.CompanyId = new SelectList(db.Companies.Where(c=>c.IsDeleted==false&&c.IsActive), "Id", "Title",relation.CompanyId);
            return View(relation);
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Relation relation = db.Relations.Find(id);
            if (relation == null)
            {
                return HttpNotFound();
            }
            ViewBag.RelationTypeId = relation.RelationTypeId;
            return View(relation);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Relation relation, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/relation/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    relation.FileUrl = newFilenameUrl;
                }
                #endregion
                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

                string uid = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

                Guid userId = new Guid(uid);
                User user = db.Users.Find(userId);
                if (user != null)
                    relation.CompanyId = user.CompanyId;

                relation.IsDeleted = false;
				relation.LastModifiedDate = DateTime.Now;
                db.Entry(relation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new{id= relation.RelationTypeId});
            }
            ViewBag.RelationTypeId = relation.RelationTypeId;
            ViewBag.CompanyId = new SelectList(db.Companies.Where(c=>c.IsDeleted==false&&c.IsActive), "Id", "Title",relation.CompanyId);
            return View(relation);
        }

        // GET: Relations/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Relation relation = db.Relations.Find(id);
            if (relation == null)
            {
                return HttpNotFound();
            }
            ViewBag.RelationTypeId = relation.RelationTypeId;

            return View(relation);
        }

        // POST: Relations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Relation relation = db.Relations.Find(id);
			relation.IsDeleted=true;
			relation.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("Index", new { id = relation.RelationTypeId });
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
