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
    public class RisksController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Risks
        public ActionResult Index(Guid id)
        {
            var risks = db.Risks.Include(r => r.Stage).Where(r=>r.StageId==id&& r.IsDeleted==false).OrderByDescending(r=>r.CreationDate);

            Stage stage = db.Stages.Find(id);

            if (stage != null)
                ViewBag.Title = "مدیریت ریسک های مربوط به مرحله " + stage.Title;

            return View(risks.ToList());
        }

         
        public ActionResult Create(Guid id)
        {
            ViewBag.StageId = id;
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Risk risk, Guid id)
        {
            if (ModelState.IsValid)
            {
                risk.Code = ReturnRiskCode();
                risk.StageId = id;
				risk.IsDeleted=false;
				risk.CreationDate= DateTime.Now; 
                risk.Id = Guid.NewGuid();
                db.Risks.Add(risk);
                db.SaveChanges();
                return RedirectToAction("Index",new{id=id});
            }

            ViewBag.StageId = id;
            return View(risk);
        }


        public int ReturnRiskCode()
        {

            Risk risk = db.Risks.OrderByDescending(current => current.Code).FirstOrDefault();
            if (risk != null)
            {
                return risk.Code + 1;
            }
           
                return 1;
           
        }


        // GET: Risks/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Risk risk = db.Risks.Find(id);
            if (risk == null)
            {
                return HttpNotFound();
            }
            ViewBag.StageId = risk.StageId;
            return View(risk);
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Risk risk)
        {
            if (ModelState.IsValid)
            {
				risk.IsDeleted = false;
				risk.LastModifiedDate = DateTime.Now;
                db.Entry(risk).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new {id=risk.StageId});
            }
            ViewBag.StageId = risk.StageId;
            return View(risk);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Risk risk = db.Risks.Find(id);
            if (risk == null)
            {
                return HttpNotFound();
            }
            ViewBag.StageId = risk.StageId;
            return View(risk);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Risk risk = db.Risks.Find(id);
			risk.IsDeleted=true;
			risk.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            ViewBag.StageId = risk.StageId;
            return RedirectToAction("Index", new { id = risk.StageId });
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
