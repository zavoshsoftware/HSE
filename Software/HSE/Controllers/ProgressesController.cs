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
    public class ProgressesController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Progresses
        public ActionResult Index(Guid id, Guid companyId)
        {
            Company co = db.Companies.Find(companyId);
            ProgressGroup progressGroup = db.ProgressGroups.Find(id);
            ViewBag.Title = "فهرست " + progressGroup.Title + " مربوط به " + co.Title;

            var progresses = db.Progresses.Where(p => p.ProgressGroupId == id && p.CompanyId == companyId && p.IsDeleted == false).OrderByDescending(p => p.CreationDate);

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;
            ViewBag.roleName = roleName;

            return View(progresses.ToList());
        }

        public ActionResult Create(Guid id, Guid companyId)
        {
            ViewBag.CompanyId = companyId;
            ViewBag.ProgressGroupId = id;

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;



            ViewBag.roleName = roleName;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Progress progress, Guid id, Guid companyId, HttpPostedFileBase fileUpload)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            if (ModelState.IsValid)
            {
                if (fileUpload != null)
                {
                    string filename = Path.GetFileName(fileUpload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/progress/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileUpload.SaveAs(physicalFilename);

                    progress.ImageUrl = newFilenameUrl;
                }

                var totalAmount = GetTotalProgress(id, companyId, null);

                ProgressGroup progressGroup = db.ProgressGroups.Find(id);

                if (roleName == "company")
                {
                    progress.Total = totalAmount + progress.CompanyPercent;
                    if (progress.Total > progressGroup.MaxAmount)
                    {
                        ModelState.AddModelError("invalidAmount",
                            "مقدار وارد شده برای درصد پیشرفت بیشتر از درصد کل این الزام می باشد");
                        ViewBag.CompanyId = companyId;
                        ViewBag.ProgressGroupId = id;


                        ViewBag.roleName = roleName;
                        return View(progress);
                    }
                }
                if (roleName == "Administrator")
                {
                    if (progress.AdminPercent != null)
                        progress.Total = totalAmount + progress.AdminPercent.Value;
                    if (progress.Total > progressGroup.MaxAmount)
                    {
                        ModelState.AddModelError("invalidAmount",
                            "مقدار وارد شده برای درصد پیشرفت بیشتر از درصد کل این الزام می باشد");
                        ViewBag.CompanyId = companyId;
                        ViewBag.ProgressGroupId = id;


                        ViewBag.roleName = roleName;
                        return View(progress);
                    }
                }

                if (roleName == "supervisor")
                {
                    if (progress.SupPercent != null)
                        progress.Total = totalAmount + progress.SupPercent.Value;
                    if (progress.Total > progressGroup.MaxAmount)
                    {
                        ModelState.AddModelError("invalidAmount",
                            "مقدار وارد شده برای درصد پیشرفت بیشتر از درصد کل این الزام می باشد");
                        ViewBag.CompanyId = companyId;
                        ViewBag.ProgressGroupId = id;


                        ViewBag.roleName = roleName;
                        return View(progress);
                    }
                }
                progress.IsActive = true;
                progress.ProgressGroupId = id;
                progress.CompanyId = companyId;
                progress.IsDeleted = false;
                progress.CreationDate = DateTime.Now;
                progress.Id = Guid.NewGuid();
                db.Progresses.Add(progress);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id, companyId = companyId });
            }

            ViewBag.CompanyId = companyId;
            ViewBag.ProgressGroupId = id;


            ViewBag.roleName = roleName;
            return View(progress);
        }

        public decimal GetTotalProgress(Guid id, Guid companyId, Guid? currentProgressId)
        {
            List<Progress> progresses = new List<Progress>();
            if (currentProgressId == null)
                progresses = db.Progresses.Where(c => c.ProgressGroupId == id && c.CompanyId == companyId).ToList();

            else
                progresses = db.Progresses.Where(c => c.ProgressGroupId == id && c.CompanyId == companyId && c.Id != currentProgressId).ToList();

            decimal total = 0;
            foreach (Progress progress in progresses)
            {
                if (progress.AdminPercent != null)
                    total += progress.AdminPercent.Value;
                else if (progress.SupPercent != null)
                    total += progress.SupPercent.Value;
                else
                    total += progress.CompanyPercent;

            }

            return total;
        }


        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Progress progress = db.Progresses.Find(id);
            if (progress == null)
            {
                return HttpNotFound();
            }
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            ViewBag.roleName = roleName;

            ViewBag.CompanyId = progress.CompanyId;
            ViewBag.ProgressGroupId = progress.ProgressGroupId;
            return View(progress);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult Edit(Progress progress, HttpPostedFileBase fileUpload)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            ViewBag.roleName = roleName;
            if (ModelState.IsValid)
            {

                if (fileUpload != null)
                {
                    string filename = Path.GetFileName(fileUpload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/progress/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileUpload.SaveAs(physicalFilename);

                    progress.ImageUrl = newFilenameUrl;
                }

                var totalAmount = GetTotalProgress(progress.ProgressGroupId, progress.CompanyId, progress.Id);

                ProgressGroup progressGroup = db.ProgressGroups.Find(progress.ProgressGroupId);



                if (roleName == "company")
                {
                    decimal total = totalAmount + progress.CompanyPercent;
                    if (progress.SupPercent == null && progress.AdminPercent == null)
                    {
                        progress.Total = total;
                    }
                    if (total > progressGroup.MaxAmount)
                    {
                        ModelState.AddModelError("invalidAmount",
                            "مقدار وارد شده برای درصد پیشرفت بیشتر از درصد کل این الزام می باشد");
                        ViewBag.CompanyId = progress.CompanyId;
                        ViewBag.ProgressGroupId = progress.ProgressGroupId;


                        ViewBag.roleName = roleName;
                        return View(progress);
                    }
                }
                if (roleName == "Administrator")
                {

                    if (progress.AdminPercent != null)
                    {

                        progress.Total = totalAmount + progress.AdminPercent.Value;

                    }

                    if (progress.Total > progressGroup.MaxAmount)
                    {
                        ModelState.AddModelError("invalidAmount",
                            "مقدار وارد شده برای درصد پیشرفت بیشتر از درصد کل این الزام می باشد");
                        ViewBag.CompanyId = progress.CompanyId;
                        ViewBag.ProgressGroupId = progress.ProgressGroupId;


                        ViewBag.roleName = roleName;
                        return View(progress);
                    }
                }

                if (roleName == "supervisor")
                {
                    decimal total = totalAmount + progress.SupPercent.Value;
                   
                        if (progress.SupPercent != null && progress.AdminPercent == null)
                    {
                        progress.Total = total;
                    }
                    if (total > progressGroup.MaxAmount)
                    {
                        ModelState.AddModelError("invalidAmount",
                            "مقدار وارد شده برای درصد پیشرفت بیشتر از درصد کل این الزام می باشد");
                        ViewBag.CompanyId = progress.CompanyId;
                        ViewBag.ProgressGroupId = progress.ProgressGroupId;

                        ViewBag.roleName = roleName;
                        return View(progress);
                    }
                }
                db.Entry(progress).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = progress.ProgressGroupId, companyId = progress.CompanyId });
            }
            ViewBag.roleName = roleName;

            ViewBag.CompanyId = progress.CompanyId;
            ViewBag.ProgressGroupId = progress.ProgressGroupId;
            return View(progress);
        }

        // GET: Progresses/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Progress progress = db.Progresses.Find(id);
            if (progress == null)
            {
                return HttpNotFound();
            }
            return View(progress);
        }

        // POST: Progresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Progress progress = db.Progresses.Find(id);
            progress.IsDeleted = true;
            progress.DeletionDate = DateTime.Now;

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
