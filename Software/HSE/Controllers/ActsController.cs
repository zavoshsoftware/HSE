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
    public class ActsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index(Guid id)
        {
            var acts = db.Acts.Include(a => a.Operation).Where(a=>a.OperationId==id&& a.IsDeleted==false).OrderByDescending(a=>a.CreationDate);
            Operation op = db.Operations.Find(id);
            if(op != null)
            {
                ViewBag.Title = "مدیریت فعالیت های مربوط به عملیات " + op.Title;
            }
            return View(acts.ToList());
        }
 
        public ActionResult Create(Guid id)
        {
            ViewBag.OperationId = id;
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Act act, Guid id)
        {
            if (ModelState.IsValid)
            {
                act.OperationId = id;
				act.IsDeleted=false;
				act.CreationDate= DateTime.Now; 
                act.Id = Guid.NewGuid();
                db.Acts.Add(act);
                db.SaveChanges();
                return RedirectToAction("Index",new{id=id});
            }

            ViewBag.OperationId = id;
            return View(act);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Act act = db.Acts.Find(id);
            if (act == null)
            {
                return HttpNotFound();
            }
            ViewBag.OperationId = act.OperationId;
            return View(act);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Act act)
        {
            if (ModelState.IsValid)
            {
				act.IsDeleted = false;
				act.LastModifiedDate = DateTime.Now;
                db.Entry(act).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new{id=act.OperationId});
            }
            ViewBag.OperationId = act.OperationId;
            return View(act);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Act act = db.Acts.Find(id);
            if (act == null)
            {
                return HttpNotFound();
            }
            ViewBag.OperationId = act.OperationId;
            return View(act);
        }

        // POST: Acts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Act act = db.Acts.Find(id);
			act.IsDeleted=true;
			act.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("Index", new { id = act.OperationId });
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
