using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using ViewModels;

namespace HSE.Controllers
{
    public class UserRisksController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        //public ActionResult Index()
        //{
        //    var userRisks = db.UserRisks.Include(u => u.AfterControlRiskIntensity).Where(u => u.IsDeleted == false).OrderByDescending(u => u.CreationDate).Include(u => u.AfterControlRiskProbability).Where(u => u.IsDeleted == false).OrderByDescending(u => u.CreationDate).Include(u => u.Risk).Where(u => u.IsDeleted == false).OrderByDescending(u => u.CreationDate).Include(u => u.RiskIntensity).Where(u => u.IsDeleted == false).OrderByDescending(u => u.CreationDate).Include(u => u.RiskProbability).Where(u => u.IsDeleted == false).OrderByDescending(u => u.CreationDate).Include(u => u.RiskStatus).Where(u => u.IsDeleted == false).OrderByDescending(u => u.CreationDate).Include(u => u.User).Where(u => u.IsDeleted == false).OrderByDescending(u => u.CreationDate);
        //    return View(userRisks.ToList());
        //}

        //public ActionResult Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserRisk userRisk = db.UserRisks.Find(id);
        //    if (userRisk == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userRisk);
        //}

        //public ActionResult Create()
        //{
        //    ViewBag.AfterControlRiskIntensityId = new SelectList(db.RiskIntensities, "Id", "Title");
        //    ViewBag.AfterControlRiskProbabilityId = new SelectList(db.RiskProbabilities, "Id", "Title");
        //    ViewBag.RiskId = new SelectList(db.Risks, "Id", "Title");
        //    ViewBag.RiskIntensityId = new SelectList(db.RiskIntensities, "Id", "Title");
        //    ViewBag.RiskProbabilityId = new SelectList(db.RiskProbabilities, "Id", "Title");
        //    ViewBag.RiskStatusId = new SelectList(db.RiskStatuses, "Id", "Title");
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "Password");
        //    return View();
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,UserId,RiskId,RiskIntensityId,RiskProbabilityId,AfterControlRiskIntensityId,AfterControlRiskProbabilityId,RiskStatusId,IsAcceptedBySupervisor,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] UserRisk userRisk)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        userRisk.IsDeleted = false;
        //        userRisk.CreationDate = DateTime.Now;
        //        userRisk.Id = Guid.NewGuid();
        //        db.UserRisks.Add(userRisk);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.AfterControlRiskIntensityId = new SelectList(db.RiskIntensities, "Id", "Title", userRisk.AfterControlRiskIntensityId);
        //    ViewBag.AfterControlRiskProbabilityId = new SelectList(db.RiskProbabilities, "Id", "Title", userRisk.AfterControlRiskProbabilityId);
        //    ViewBag.RiskId = new SelectList(db.Risks, "Id", "Title", userRisk.RiskId);
        //    ViewBag.RiskIntensityId = new SelectList(db.RiskIntensities, "Id", "Title", userRisk.RiskIntensityId);
        //    ViewBag.RiskProbabilityId = new SelectList(db.RiskProbabilities, "Id", "Title", userRisk.RiskProbabilityId);
        //    ViewBag.RiskStatusId = new SelectList(db.RiskStatuses, "Id", "Title", userRisk.RiskStatusId);
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "Password", userRisk.UserId);
        //    return View(userRisk);
        //}

        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserRisk userRisk = db.UserRisks.Find(id);
        //    if (userRisk == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.AfterControlRiskIntensityId = new SelectList(db.RiskIntensities, "Id", "Title", userRisk.AfterControlRiskIntensityId);
        //    ViewBag.AfterControlRiskProbabilityId = new SelectList(db.RiskProbabilities, "Id", "Title", userRisk.AfterControlRiskProbabilityId);
        //    ViewBag.RiskId = new SelectList(db.Risks, "Id", "Title", userRisk.RiskId);
        //    ViewBag.RiskIntensityId = new SelectList(db.RiskIntensities, "Id", "Title", userRisk.RiskIntensityId);
        //    ViewBag.RiskProbabilityId = new SelectList(db.RiskProbabilities, "Id", "Title", userRisk.RiskProbabilityId);
        //    ViewBag.RiskStatusId = new SelectList(db.RiskStatuses, "Id", "Title", userRisk.RiskStatusId);
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "Password", userRisk.UserId);
        //    return View(userRisk);
        //}

        //// POST: UserRisks/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,UserId,RiskId,RiskIntensityId,RiskProbabilityId,AfterControlRiskIntensityId,AfterControlRiskProbabilityId,RiskStatusId,IsAcceptedBySupervisor,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] UserRisk userRisk)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        userRisk.IsDeleted = false;
        //        userRisk.LastModifiedDate = DateTime.Now;
        //        db.Entry(userRisk).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.AfterControlRiskIntensityId = new SelectList(db.RiskIntensities, "Id", "Title", userRisk.AfterControlRiskIntensityId);
        //    ViewBag.AfterControlRiskProbabilityId = new SelectList(db.RiskProbabilities, "Id", "Title", userRisk.AfterControlRiskProbabilityId);
        //    ViewBag.RiskId = new SelectList(db.Risks, "Id", "Title", userRisk.RiskId);
        //    ViewBag.RiskIntensityId = new SelectList(db.RiskIntensities, "Id", "Title", userRisk.RiskIntensityId);
        //    ViewBag.RiskProbabilityId = new SelectList(db.RiskProbabilities, "Id", "Title", userRisk.RiskProbabilityId);
        //    ViewBag.RiskStatusId = new SelectList(db.RiskStatuses, "Id", "Title", userRisk.RiskStatusId);
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "Password", userRisk.UserId);
        //    return View(userRisk);
        //}

        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserRisk userRisk = db.UserRisks.Find(id);
        //    if (userRisk == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userRisk);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    UserRisk userRisk = db.UserRisks.Find(id);
        //    userRisk.IsDeleted = true;
        //    userRisk.DeletionDate = DateTime.Now;

        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        [Authorize(Roles = "company")]
        public ActionResult Form()
        {
            List<Project> projects = db.Projects.Where(c => c.IsDeleted == false && c.IsActive).ToList();

            UserRiskFormViewModel userRisk = new UserRiskFormViewModel()
            {
                Projects = projects,
                ProbDesc = GetProbDesc(),
                IntenDesc = GetIntenDesc()
            };
            return View(userRisk);
        }

        public string GetProbDesc()
        {
            List<RiskProbability> riskProbabilities =
                db.RiskProbabilities.Where(c => c.IsDeleted == false).OrderBy(c => c.Level).ToList();

            string val = "";

            foreach (RiskProbability probability in riskProbabilities)
            {
                val += "<span>" + probability.Title + ":</span> " + probability.Summery2 + "<br />";
            }

            return val;
        }


        public string GetIntenDesc()
        {
            List<RiskIntensity> riskIntensities =
                db.RiskIntensities.Where(c => c.IsDeleted == false).OrderBy(c => c.Level).ToList();

            string val = "";


            foreach (RiskIntensity intensity in riskIntensities)
            {
                val += "<span>" + intensity.Title + ":</span> " + intensity.Summery + "<br />";
            }

            return val;
        }



        public ActionResult GetOperation(string id)
        {
            Guid idGuid = new Guid(id);

            List<Operation> operations =
                db.Operations.Where(c => c.ProjectId == idGuid && c.IsDeleted == false && c.IsActive).ToList();

            List<DropDownItemViewModel> items = new List<DropDownItemViewModel>();

            foreach (Operation op in operations)
            {
                items.Add(new DropDownItemViewModel()
                {
                    Title = op.Title,
                    Value = op.Id.ToString()
                });
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAct(string id)
        {
            Guid idGuid = new Guid(id);

            List<Act> acts =
                db.Acts.Where(c => c.OperationId == idGuid && c.IsDeleted == false && c.IsActive).ToList();

            List<DropDownItemViewModel> items = new List<DropDownItemViewModel>();

            foreach (Act ac in acts)
            {
                items.Add(new DropDownItemViewModel()
                {
                    Title = ac.Title,
                    Value = ac.Id.ToString()
                });
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStage(string id)
        {
            Guid idGuid = new Guid(id);

            List<Stage> stages =
                db.Stages.Where(c => c.ActId == idGuid && c.IsDeleted == false && c.IsActive).ToList();

            List<DropDownItemViewModel> items = new List<DropDownItemViewModel>();

            foreach (Stage st in stages)
            {
                items.Add(new DropDownItemViewModel()
                {
                    Title = st.Title,
                    Value = st.Id.ToString()
                });
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetRisk(string id)
        {
            Guid idGuid = new Guid(id);

            List<Risk> Risks =
                db.Risks.Where(c => c.StageId == idGuid && c.IsDeleted == false && c.IsActive).ToList();

            List<RiskItemTableViewModel> riskItems = new List<RiskItemTableViewModel>();


            int i = 1;
            foreach (Risk risk in Risks)
            {
                riskItems.Add(new RiskItemTableViewModel()
                {
                    Title = risk.Title,
                    Code = risk.Code,
                    Index = i,
                    RiskProbability = GetRiskParam("probability"),
                    RiskIntensity = GetRiskParam("intensity"),
                });

                i++;
            }
            return Json(riskItems, JsonRequestBehavior.AllowGet);
        }


        public List<DropDownItemViewModel> GetRiskParam(string typeTitle)
        {
            List<DropDownItemViewModel> list = new List<DropDownItemViewModel>();
            if (typeTitle == "probability")
            {
                List<RiskProbability> riskProbabilities =
                    db.RiskProbabilities.Where(c => c.IsDeleted == false).OrderBy(c => c.Level).ToList();

                foreach (RiskProbability riskProbability in riskProbabilities)
                {
                    list.Add(new DropDownItemViewModel()
                    {
                        Title = riskProbability.Title,
                        Value = riskProbability.Level.ToString()
                    });
                }
            }

            else if (typeTitle == "intensity")
            {
                List<RiskIntensity> riskIntensities =
                    db.RiskIntensities.Where(c => c.IsDeleted == false).OrderBy(c => c.Level).ToList();

                foreach (RiskIntensity riskIntensity in riskIntensities)
                {
                    list.Add(new DropDownItemViewModel()
                    {
                        Title = riskIntensity.Title,
                        Value = riskIntensity.Level.ToString()
                    });
                }
            }
            return list;
        }



        public ActionResult CalculateRisk(List<CalculateRiskInputViewModel> input)
        {
            List<CalculateRiskOutputViewModel> result = new List<CalculateRiskOutputViewModel>();

            foreach (CalculateRiskInputViewModel riskInput in input)
            {
                int riskProbabilityLevel = Convert.ToInt32(riskInput.Prob);
                int riskIntensityLevel = Convert.ToInt32(riskInput.Intent);

                RiskMatris riskMatris = db.RiskMatris.FirstOrDefault(c =>
                    c.RiskIntensity.Level == riskIntensityLevel && c.RiskProbability.Level == riskProbabilityLevel);

                if (riskMatris != null)
                {
                    result.Add(new CalculateRiskOutputViewModel()
                    {
                        Result = riskMatris.RiskNumber + " - " + GetTextRuslt(riskMatris),
                        Code = riskInput.Code
                    });
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public string GetTextRuslt(RiskMatris riskMatris)
        {
            string res = "";
            if (riskMatris.RiskNumber >= 1 &&
                riskMatris.RiskNumber <= 3)
            {

                res = "قابل قبول بدون نیاز به بازنگری";
            }
            else if (riskMatris.RiskNumber >= 4 &&
                     riskMatris.RiskNumber <= 11)
            {
                res = "قابل قبول با نیاز به بازنگری";
            }
            else if (riskMatris.RiskNumber >= 12 &&
                     riskMatris.RiskNumber <= 15)
            {
                res = "نامطلوب ، نیاز به تصمیم گیری";
            }
            else if (riskMatris.RiskNumber >= 16 &&
                     riskMatris.RiskNumber <= 20)
            {
                res = "غیر قابل قبول";
            }

            return res;
        }



        public ActionResult SubmitRisk(List<CalculateRiskInputViewModel> input)
        {
            try
            {


                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                string id = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

                Guid userId = new Guid(id);



                Guid? stageId = GetStageIdByRiskCode(input.FirstOrDefault().Code);

                if (stageId != null)
                {
                    UserStage userStage = new UserStage()
                    {
                        UserId = userId,
                        StageId = stageId,
                        RiskStatusId = db.RiskStatuses.FirstOrDefault(c => c.Code == 1).Id,
                        CreationDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false
                    };

                    db.UserStages.Add(userStage);


                    foreach (CalculateRiskInputViewModel riskInput in input)
                    {
                        int riskProbabilityLevel = Convert.ToInt32(riskInput.Prob);
                        int riskIntensityLevel = Convert.ToInt32(riskInput.Intent);
                        int riskCode = Convert.ToInt32(riskInput.Code);

                        RiskProbability riskProbability =
                            db.RiskProbabilities.FirstOrDefault(c => c.Level == riskProbabilityLevel);

                        RiskIntensity riskIntensity =
                            db.RiskIntensities.FirstOrDefault(c => c.Level == riskIntensityLevel);

                        RiskMatris riskMatris = db.RiskMatris.FirstOrDefault(c =>
                            c.RiskIntensity.Level == riskIntensityLevel && c.RiskProbability.Level == riskProbabilityLevel);

                        Risk risk = db.Risks.FirstOrDefault(c => c.Code == riskCode);

                        UserRisk userRisk = new UserRisk()
                        {
                            UserStageId = userStage.Id,
                            RiskIntensityId = riskIntensity.Id,
                            RiskProbabilityId = riskProbability.Id,
                            IsAcceptedBySupervisor = false,
                            RiskNumber = riskMatris.RiskNumber,
                            RiskDescription = GetTextRuslt(riskMatris),
                            RiskId = risk.Id,
                            CreationDate = DateTime.Now,
                            IsActive = true,
                            IsDeleted = false
                        };


                        db.UserRisks.Add(userRisk);



                    }

                    db.SaveChanges();
                    return Json("true", JsonRequestBehavior.AllowGet);

                }
                return Json("false", JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);

            }
        }

        public Guid? GetStageIdByRiskCode(string code)
        {
            int riskCode = Convert.ToInt32(code);
            Risk risk = db.Risks.FirstOrDefault(c => c.Code == riskCode);

            if (risk != null)
                return risk.StageId;

            return null;
        }

    }
}
