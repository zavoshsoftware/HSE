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
            List<Relation> relations=new List<Relation>();

            User user = db.Users.Find(userId);

            if (roleName == "company")
                relations = db.Relations.Include(r => r.RelationType)
                    .Where(r => r.CompanyId == user.CompanyId && r.RelationTypeId == id && r.IsDeleted == false)
                    .OrderByDescending(r => r.CreationDate)
                    .ToList();
            else if (roleName == "supervisor")
            {
                List<User> users = Helpers.GetUserInfo.GetCompanyUsersBySupervisor(userId);
                foreach (User user1 in users)
                {
                    List<Relation> companyRelations = db.Relations.Include(r => r.RelationType)
                        .Where(r => r.CompanyId == user1.CompanyId && r.RelationTypeId == id && r.IsDeleted == false)
                        .OrderByDescending(r => r.CreationDate)
                        .ToList();

                    foreach (Relation companyRelation in companyRelations)
                    {
                        relations.Add(companyRelation);
                    }
                }
            }
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
            ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.IsDeleted == false && c.IsActive), "Id", "Title", user.CompanyId);
            ViewBag.RelationTypeId = id;

            if (id == new Guid("6ae4c301-1bdc-4495-82de-f4e356f67e12") ||
                     id == new Guid("79d99e93-5b4e-43cb-9e35-a5634ddb709e"))
            {
                ViewBag.relationType = "edu";
            }
            else if (id == new Guid("e881a489-6110-4144-a6bf-9abf528d5601") ||
                     id == new Guid("ceec721f-34ba-4f30-b4b0-eb648d17fba5"))
            {
                ViewBag.relationType = "inorganization";
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Relation relation, Guid id, HttpPostedFileBase fileupload, HttpPostedFileBase ActionPlanFileUpload, List<HttpPostedFileBase> imagesFileUpload)
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
                if (ActionPlanFileUpload != null)
                {
                    string filename = Path.GetFileName(ActionPlanFileUpload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/relation/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    ActionPlanFileUpload.SaveAs(physicalFilename);

                    relation.ActionPlanFileUrl = newFilenameUrl;
                }

                #endregion

                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

                string uid = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

                Guid userId = new Guid(uid);
                User user = db.Users.Find(userId);

                if (user != null)
                    if (user.CompanyId != null)
                        relation.CompanyId = user.CompanyId;


                relation.RelationTypeId = id;
                relation.IsDeleted = false;
                relation.CreationDate = DateTime.Now;
                relation.Id = Guid.NewGuid();
                db.Relations.Add(relation);

                if (imagesFileUpload != null)
                {
                    foreach (HttpPostedFileBase t in imagesFileUpload)
                    {
                        string filename = Path.GetFileName(t.FileName);
                        string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                             + Path.GetExtension(filename);

                        string newFilenameUrl = "/Uploads/relation/" + newFilename;
                        string physicalFilename = Server.MapPath(newFilenameUrl);

                        t.SaveAs(physicalFilename);

                        RelationImage relationImage = new RelationImage()
                        {
                            Id = Guid.NewGuid(),
                            ImageUrl = newFilenameUrl,
                            CreationDate = DateTime.Now,
                            RelationId = relation.Id,
                            IsActive = true,
                            IsDeleted = false,
                        };
                        db.RelationImages.Add(relationImage);
                    }
                }

                db.SaveChanges();
                Company co = db.Companies.Find(relation.CompanyId);
                Helpers.NotificationHelper.InsertNotification(co.Title, "/relations/Index/" + relation.RelationTypeId, "ارتباطات");
                Helpers.NotificationHelper.InsertNotificationForSup(co.Id,co.Title, "/relations/Index/" + relation.RelationTypeId, "ارتباطات");

                return RedirectToAction("Index", new { id = id });
            }
            if (id == new Guid("6ae4c301-1bdc-4495-82de-f4e356f67e12") ||
                id == new Guid("79d99e93-5b4e-43cb-9e35-a5634ddb709e"))
            {
                ViewBag.relationType = "edu";
            }
            else if (id == new Guid("e881a489-6110-4144-a6bf-9abf528d5601") ||
                     id == new Guid("ceec721f-34ba-4f30-b4b0-eb648d17fba5"))
            {
                ViewBag.relationType = "inorganization";
            }

            ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.IsDeleted == false && c.IsActive), "Id", "Title");
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
            if (relation.RelationTypeId == new Guid("6ae4c301-1bdc-4495-82de-f4e356f67e12") ||
                relation.RelationTypeId == new Guid("79d99e93-5b4e-43cb-9e35-a5634ddb709e"))
            {
                ViewBag.relationType = "edu";
            }

            else if (relation.RelationTypeId == new Guid("e881a489-6110-4144-a6bf-9abf528d5601") ||
                     relation.RelationTypeId == new Guid("ceec721f-34ba-4f30-b4b0-eb648d17fba5"))
            {
                ViewBag.relationType = "inorganization";
            }
            ViewBag.RelationTypeId = relation.RelationTypeId;
            ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.IsDeleted == false && c.IsActive), "Id", "Title", relation.CompanyId);
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
            if (relation.RelationTypeId == new Guid("6ae4c301-1bdc-4495-82de-f4e356f67e12") ||
                relation.RelationTypeId == new Guid("79d99e93-5b4e-43cb-9e35-a5634ddb709e"))
            {
                ViewBag.relationType = "edu";
            }


            else if (relation.RelationTypeId == new Guid("e881a489-6110-4144-a6bf-9abf528d5601") ||
                     relation.RelationTypeId == new Guid("ceec721f-34ba-4f30-b4b0-eb648d17fba5"))
            {
                ViewBag.relationType = "inorganization";
            }
            relationDetailViewModel result=new relationDetailViewModel()
            {
                Relation = relation,
                RelationImages = db.RelationImages.Where(c=>c.IsDeleted==false&&c.RelationId==id).ToList()
            };
            ViewBag.RelationTypeId = relation.RelationTypeId;
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Relation relation, HttpPostedFileBase fileupload, HttpPostedFileBase ActionPlanFileUpload, List<HttpPostedFileBase> imagesFileUpload)
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

                if (ActionPlanFileUpload != null)
                {
                    string filename = Path.GetFileName(ActionPlanFileUpload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/relation/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    ActionPlanFileUpload.SaveAs(physicalFilename);

                    relation.ActionPlanFileUrl = newFilenameUrl;
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



                if (imagesFileUpload != null)
                {
                    foreach (HttpPostedFileBase t in imagesFileUpload)
                    {
                        string filename = Path.GetFileName(t.FileName);
                        string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                             + Path.GetExtension(filename);

                        string newFilenameUrl = "/Uploads/relation/" + newFilename;
                        string physicalFilename = Server.MapPath(newFilenameUrl);

                        t.SaveAs(physicalFilename);

                        RelationImage relationImage = new RelationImage()
                        {
                            Id = Guid.NewGuid(),
                            ImageUrl = newFilenameUrl,
                            CreationDate = DateTime.Now,
                            RelationId = relation.Id,
                            IsActive = true,
                            IsDeleted = false,
                        };
                        db.RelationImages.Add(relationImage);
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index", new { id = relation.RelationTypeId });
            }
            if (relation.RelationTypeId == new Guid("6ae4c301-1bdc-4495-82de-f4e356f67e12") ||
                relation.RelationTypeId == new Guid("79d99e93-5b4e-43cb-9e35-a5634ddb709e"))
            {
                ViewBag.relationType = "edu";
            }


            else if (relation.RelationTypeId == new Guid("e881a489-6110-4144-a6bf-9abf528d5601") ||
                     relation.RelationTypeId == new Guid("ceec721f-34ba-4f30-b4b0-eb648d17fba5"))
            {
                ViewBag.relationType = "inorganization";
            }
            ViewBag.RelationTypeId = relation.RelationTypeId;
            ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.IsDeleted == false && c.IsActive), "Id", "Title", relation.CompanyId);
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
            relation.IsDeleted = true;
            relation.DeletionDate = DateTime.Now;

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
