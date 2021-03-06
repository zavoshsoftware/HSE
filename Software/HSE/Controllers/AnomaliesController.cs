﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Models;
using ViewModels;

namespace HSE.Controllers
{
    [Authorize(Roles = "Administrator,company,supervisor")]
    public class AnomaliesController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string id = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            Guid userId = new Guid(id);

            ViewBag.roleName = roleName;
            List<Anomaly> anomalies = new List<Anomaly>();

            if (roleName == "Administrator")
            {
                anomalies = db.Anomalies.Where(a => a.IsDeleted == false)
                    .OrderByDescending(a => a.CreationDate).ToList();
            }

            if (roleName == "company")
            {
                anomalies = db.Anomalies.Where(a => a.IsDeleted == false && a.ResponseUserId == userId)
                    .OrderByDescending(a => a.CreationDate).ToList();
            }
            if (roleName == "supervisor")
            {
                List<User> users = GetUserInfo.GetCompanyUsersBySupervisor(userId);

                foreach (User user in users)
                {
                    List<Anomaly> companyAnomalies = db.Anomalies
                        .Where(a => a.IsDeleted == false && a.CompanyId == user.Company.Id)
                        .OrderByDescending(a => a.CreationDate).ToList();

                    foreach (Anomaly companyAnomaly in companyAnomalies)
                    {
                        anomalies.Add(companyAnomaly);
                    }
                }

            }
            return View(anomalies);
        }
        public ActionResult IndexAdmin(Guid id)
        {
            List<Anomaly> anomalies = new List<Anomaly>();

            anomalies = db.Anomalies.Where(a => a.CompanyId == id && a.IsDeleted == false)
                .OrderByDescending(a => a.CreationDate).ToList();

            return View(anomalies);
        }

        public ActionResult CompanyTypeList()
        {
            List<CompanyType> companyTypes = db.CompanyTypes.Where(c => c.IsDeleted == false && c.IsActive).ToList();
            ViewBag.baseUrl = "Anomalies";

            ViewBag.Title = "عدم انطباق (Anomaly)";

            return View(companyTypes);
        }


        public ActionResult List(Guid? id)
        {
            List<Company> companies = db.Companies.Where(c => c.CompanyTypeId == id && c.IsDeleted == false && c.IsActive == true).OrderBy(c => c.Title).ToList();

            return View(companies);
        }
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Anomaly anomaly = db.Anomalies.Find(id);
            if (anomaly == null)
            {
                return HttpNotFound();
            }

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;
            if (roleName == "company")
            {
                ViewBag.AnomalyResultId = new SelectList(db.AnomalyResults, "Id", "Title", anomaly.AnomalyResultId);
            }
            if (roleName == "Administrator")
            {
                ViewBag.companyId = anomaly.CompanyId;
            }

            ViewBag.rolename = roleName;

            AnomalyDetailViewModel result = new AnomalyDetailViewModel()
            {
                AnomalyAttachments = db.AnomalyAttachments.Where(c => c.AnomalyId == anomaly.Id).ToList(),
                Code = anomaly.Code,
                ImageUrl = anomaly.ImageUrl,
                CompanyTitle = anomaly.Company.Title,
                Summery = anomaly.Summery,
                DeadlineStr = anomaly.DeadlineStr,
                EventDateStr = anomaly.EventDateStr,
                AnomalyHseTitle = anomaly.AnomalyHse.Title,
                AnomalyLevelTitle = anomaly.AnomalyLevel.Title,
                AnomalyResultTitle = anomaly.AnomalyResult.Title,
                CompanyNotes = anomaly.CompanyNotes,
                CreatorUserFullName = anomaly.CreatorUser.FullName,
                EffectivnessDateStr = anomaly.EffectivnessDateStr,
                Notes = anomaly.Notes,
                Place = anomaly.Place,
                ResponseUserFullName = anomaly.ResponseUser.FullName,
                StatusTitle = anomaly.Status.Title,
                CreationDateStr = anomaly.CreationDateStr,
                StatusCode = anomaly.Status.Order,
                Id = anomaly.Id,
                EffectivnessDate = anomaly.EffectivnessDate
            };
            return View(result);
        }

        [HttpPost]
        public ActionResult Details(AnomalyDetailViewModel result, List<HttpPostedFileBase> fileUploadResultAttachment)
        {

            if (ModelState.IsValid)
            {
                Anomaly anomaly = db.Anomalies.Find(result.Id);

                anomaly.IsDeleted = false;
                anomaly.StatusId = db.Status.OrderBy(c => c.Order).Skip(1).FirstOrDefault().Id;
                anomaly.LastModifiedDate = DateTime.Now;
                anomaly.AnomalyResultId = result.AnomalyResultId;
                anomaly.EffectivnessDate = result.EffectivnessDate;
                anomaly.CompanyNotes = result.CompanyNotes;

                #region Upload and resize image if needed

                if (fileUploadResultAttachment != null)
                {
                    foreach (HttpPostedFileBase t in fileUploadResultAttachment)
                    {
                        if (t != null)
                        {
                            string filename = Path.GetFileName(t.FileName);
                            string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                                 + Path.GetExtension(filename);

                            string newFilenameUrl = "/Uploads/anomaly/" + newFilename;
                            string physicalFilename = Server.MapPath(newFilenameUrl);

                            t.SaveAs(physicalFilename);

                            AnomalyAttachment anomalyAttachment = new AnomalyAttachment()
                            {
                                Id = Guid.NewGuid(),
                                ImageUrl = newFilenameUrl,
                                CreationDate = DateTime.Now,
                                AnomalyId = anomaly.Id,
                                IsActive = true,
                                IsDeleted = false,
                                IsResultAttachment = true
                            };
                            db.AnomalyAttachments.Add(anomalyAttachment);
                        }
                    }
                }

                #endregion


                db.Entry(anomaly).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(result);
        }


        public ActionResult DetailsForSup(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Anomaly anomaly = db.Anomalies.Find(id);
            if (anomaly == null)
            {
                return HttpNotFound();
            }

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;
            if (roleName == "supervisor")
            {
                ViewBag.StatusId = new SelectList(db.Status.Where(c => c.Order == 3 || c.Order == 4), "Id", "Title", anomaly.AnomalyResultId);
            }

            Status st = db.Status.Find(anomaly.StatusId);
            ViewBag.statuscode = st.Order;
            return View(anomaly);
        }



        [HttpPost]
        public ActionResult DetailsForSup(Anomaly anomaly)
        {

            if (ModelState.IsValid)
            {
                anomaly.IsDeleted = false;
                anomaly.LastModifiedDate = DateTime.Now;
                db.Entry(anomaly).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(anomaly);
        }


        // GET: Anomalies/Create
        public ActionResult Create()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;
            string id = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

            Guid userId = new Guid(id);
            ViewBag.role = roleName;
            User user = db.Users.Find(userId);
            if (roleName == "company")
            {
                if (user != null)
                {
                    ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title", user.CompanyId);


                }
                else
                {
                    ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title");
                }
            }
            else if (roleName == "supervisor")
            {
                ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.SupervisorUserId == userId), "Id", "Title");
            }
            else
            {
                ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title");
            }
            ViewBag.AnomalyHseId = new SelectList(db.AnomalyHses, "Id", "Title");
            ViewBag.AnomalyLevelId = new SelectList(db.AnomalyLevels, "Id", "Title");
            ViewBag.AnomalyResultId = new SelectList(db.AnomalyResults, "Id", "Title");

            return View();
        }

        // POST: Anomalies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Anomaly anomaly, List<HttpPostedFileBase> fileupload)
        {


            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string id = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
            Guid userId = new Guid(id);
            User user = db.Users.Find(userId);
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            if (ModelState.IsValid)
            {

                if (roleName == "company" && user.CompanyId != null)
                    anomaly.CompanyId = user.CompanyId.Value;


                anomaly.CreatorUserId = userId;
                anomaly.AnomalyResultId = new Guid("0EDEBF8C-622B-4816-8F0B-FF06C676F37B");
                anomaly.StatusId = db.Status.OrderBy(c => c.Order).FirstOrDefault().Id;
                anomaly.IsDeleted = false;
                anomaly.CreationDate = DateTime.Now;
                anomaly.Id = Guid.NewGuid();
                db.Anomalies.Add(anomaly);


                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    foreach (HttpPostedFileBase t in fileupload)
                    {
                        if (t != null)
                        {
                            string filename = Path.GetFileName(t.FileName);
                            string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                                 + Path.GetExtension(filename);

                            string newFilenameUrl = "/Uploads/anomaly/" + newFilename;
                            string physicalFilename = Server.MapPath(newFilenameUrl);

                            t.SaveAs(physicalFilename);

                            AnomalyAttachment anomalyAttachment = new AnomalyAttachment()
                            {
                                Id = Guid.NewGuid(),
                                ImageUrl = newFilenameUrl,
                                CreationDate = DateTime.Now,
                                AnomalyId = anomaly.Id,
                                IsActive = true,
                                IsDeleted = false,
                                IsResultAttachment = false
                            };
                            db.AnomalyAttachments.Add(anomalyAttachment);
                        }
                    }
                }
                #endregion

                db.SaveChanges();
                Company co = db.Companies.Find(anomaly.CompanyId);
                Helpers.NotificationHelper.InsertNotification(co.Title, "/Anomalies/Indexadmin/" + co.Id, "عدم انطباق");
                if(roleName == "company")
                Helpers.NotificationHelper.InsertNotificationForSup(co.Id,co.Title, "/anomalies/index/", "عدم انطباق");
                else
                    Helpers.NotificationHelper.InsertNotificationForCompany(co.Id, co.Title, "/anomalies/index/", "عدم انطباق","create");

                return RedirectToAction("Index");
            }

            ViewBag.AnomalyHseId = new SelectList(db.AnomalyHses, "Id", "Title");
            ViewBag.AnomalyLevelId = new SelectList(db.AnomalyLevels, "Id", "Title");
            ViewBag.AnomalyResultId = new SelectList(db.AnomalyResults, "Id", "Title");

            if (roleName == "company")
            {

                if (user != null)
                {
                    ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title", user.CompanyId);
                }
                else
                {
                    ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title");
                }
            }
            if (roleName == "supervisor")
            {
                ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.SupervisorUserId == userId), "Id", "Title");
            }
            else
            {
                ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Title");
            }

            return View(anomaly);
        }




        // GET: Anomalies/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Anomaly anomaly = db.Anomalies.Find(id);
            if (anomaly == null)
            {
                return HttpNotFound();
            }

            return View(anomaly);
        }

        // POST: Anomalies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Anomaly anomaly)
        {
            if (ModelState.IsValid)
            {
                anomaly.IsDeleted = false;
                anomaly.LastModifiedDate = DateTime.Now;
                db.Entry(anomaly).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(anomaly);
        }

        // GET: Anomalies/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Anomaly anomaly = db.Anomalies.Find(id);
            if (anomaly == null)
            {
                return HttpNotFound();
            }
            return View(anomaly);
        }

        // POST: Anomalies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Anomaly anomaly = db.Anomalies.Find(id);
            anomaly.IsDeleted = true;
            anomaly.DeletionDate = DateTime.Now;

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


        [AllowAnonymous]
        public ActionResult FillResponseUser(string id)
        {
            Guid companyId = new Guid(id);

            List<ResponseUserViewModel> companyItems = new List<ResponseUserViewModel>();

            List<User> users = db.Users.Where(c => c.CompanyId == companyId && c.IsDeleted == false).ToList();
            foreach (User user in users)
            {
                companyItems.Add(new ResponseUserViewModel()
                {
                    Title = user.FullName,
                    Value = user.Id.ToString()
                });
            }

            return Json(companyItems, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RemoveAttachment(Guid id)
        {
            AnomalyAttachment anomalyAttachment = db.AnomalyAttachments.Find(id);
            if (anomalyAttachment != null)
            {
                anomalyAttachment.IsDeleted = true;
                anomalyAttachment.DeletionDate = DateTime.Now;

                db.SaveChanges();

                return RedirectToAction("Details", new { id = anomalyAttachment.AnomalyId });
            }

            return RedirectToAction("Index");
        }
        //public string ConvertAttachment()
        //{
        //    List<Anomaly> anomalies = db.Anomalies.ToList();

        //    foreach (var anomaly in anomalies)
        //    {
        //        if (!string.IsNullOrEmpty(anomaly.ImageUrl))
        //        {
        //            AnomalyAttachment anomalyAttachment=new AnomalyAttachment()
        //            {
        //                ImageUrl = anomaly.ImageUrl,
        //                Id=Guid.NewGuid(),
        //                IsActive = anomaly.IsActive,
        //                IsDeleted = anomaly.IsDeleted,
        //                AnomalyId = anomaly.Id,
        //                CreationDate = DateTime.Now,
        //            };

        //            db.AnomalyAttachments.Add(anomalyAttachment);
        //        }
        //    }

        //    db.SaveChanges();
        //    return "";
        //}
    }
}
