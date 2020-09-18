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
    public class EquipmentTypesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: EquipmentTypes
        public ActionResult Index()
        {
            return View(db.EquipmentTypes.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: EquipmentTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentType equipmentType = db.EquipmentTypes.Find(id);
            if (equipmentType == null)
            {
                return HttpNotFound();
            }
            return View(equipmentType);
        }

        // GET: EquipmentTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EquipmentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] EquipmentType equipmentType)
        {
            if (ModelState.IsValid)
            {
				equipmentType.IsDeleted=false;
				equipmentType.CreationDate= DateTime.Now; 
                equipmentType.Id = Guid.NewGuid();
                db.EquipmentTypes.Add(equipmentType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(equipmentType);
        }

        // GET: EquipmentTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentType equipmentType = db.EquipmentTypes.Find(id);
            if (equipmentType == null)
            {
                return HttpNotFound();
            }
            return View(equipmentType);
        }

        // POST: EquipmentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] EquipmentType equipmentType)
        {
            if (ModelState.IsValid)
            {
				equipmentType.IsDeleted = false;
				equipmentType.LastModifiedDate = DateTime.Now;
                db.Entry(equipmentType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(equipmentType);
        }

        // GET: EquipmentTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentType equipmentType = db.EquipmentTypes.Find(id);
            if (equipmentType == null)
            {
                return HttpNotFound();
            }
            return View(equipmentType);
        }

        // POST: EquipmentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            EquipmentType equipmentType = db.EquipmentTypes.Find(id);
			equipmentType.IsDeleted=true;
			equipmentType.DeletionDate=DateTime.Now;
 
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
