using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;

namespace HSE.Controllers
{
    [Authorize(Roles = "Administrator,company,supervisor")]
    public class RelationsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Relations
        public ActionResult Index(Guid id)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            ViewBag.role = roleName;
            var relations = db.Relations.Include(r => r.RelationType).Where(r=>r.RelationTypeId==id&& r.IsDeleted==false).OrderByDescending(r=>r.CreationDate);
            return View(relations.ToList());
        }
         
        public ActionResult Create(Guid id)
        {
            ViewBag.RelationTypeId = id;
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Relation relation, Guid id)
        {
            if (ModelState.IsValid)
            {
                relation.RelationTypeId = id;
				relation.IsDeleted=false;
				relation.CreationDate= DateTime.Now; 
                relation.Id = Guid.NewGuid();
                db.Relations.Add(relation);
                db.SaveChanges();
                return RedirectToAction("Index",new{id=id});
            }

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
        public ActionResult Edit(Relation relation)
        {
            if (ModelState.IsValid)
            {
				relation.IsDeleted = false;
				relation.LastModifiedDate = DateTime.Now;
                db.Entry(relation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new{id= relation.RelationTypeId});
            }
            ViewBag.RelationTypeId = relation.RelationTypeId;
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
