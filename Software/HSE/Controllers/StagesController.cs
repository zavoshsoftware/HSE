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
    public class StagesController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        
        public ActionResult Index(Guid id)
        {
            var stages = db.Stages.Include(s => s.Act).Where(s=>s.ActId==id&& s.IsDeleted==false).OrderByDescending(s=>s.CreationDate);

            Act act = db.Acts.Find(id);

            if(act!=null)
                ViewBag.Title="مدیریت مراحل انجام کار مربوط به فعالیت "+act.Title;

            return View(stages.ToList());
        }
      
        public ActionResult Create(Guid id)
        {
            ViewBag.ActId = id;
            return View();
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Stage stage, Guid id)
        {
            if (ModelState.IsValid)
            {
                stage.ActId = id;
				stage.IsDeleted=false;
				stage.CreationDate= DateTime.Now; 
                stage.Id = Guid.NewGuid();
                db.Stages.Add(stage);
                db.SaveChanges();
                return RedirectToAction("Index",new{id= id });
            }

            ViewBag.ActId = id;
            return View(stage);
        }

        // GET: Stages/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stage stage = db.Stages.Find(id);
            if (stage == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActId = stage.ActId;
            return View(stage);
        }

        // POST: Stages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Code,ActId,OldId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Stage stage)
        {
            if (ModelState.IsValid)
            {
				stage.IsDeleted = false;
				stage.LastModifiedDate = DateTime.Now;
                db.Entry(stage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new{id=stage.ActId});
            }
            ViewBag.ActId = stage.ActId;
            return View(stage);
        }

        // GET: Stages/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stage stage = db.Stages.Find(id);
            if (stage == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index", new { id = stage.ActId });
        }

        // POST: Stages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Stage stage = db.Stages.Find(id);
			stage.IsDeleted=true;
			stage.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            ViewBag.ActId = stage.ActId;
            return RedirectToAction("Index", new { id = stage.ActId });
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
