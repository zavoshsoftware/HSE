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
    public class UserStagesController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

    [Authorize(Roles = "company")]
        public ActionResult Index()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

            string id = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

            Guid userId = new Guid(id);

            List<UserStage> userStages = db.UserStages.Include(u => u.RiskStatus)
                .Where(u => u.UserId == userId && u.IsDeleted == false).OrderByDescending(u => u.CreationDate)
                .Include(u => u.Stage).Include(u => u.User).ToList();

           List<UserStageListViewModel> stages=new List<UserStageListViewModel>();

            foreach (UserStage userStage in userStages)
            {
                stages.Add(new UserStageListViewModel()
                {
                    StageTitle = userStage.Stage.Title,
                    ActTitle = userStage.Stage.Act.Title,
                    OperationTitle = userStage.Stage.Act.Operation.Title,
                    ProjectTitle = userStage.Stage.Act.Operation.Project.Title,
                    Id = userStage.Id,
                    StatusTitle = userStage.RiskStatus.Title
                });
            }

            return View(stages);
        }

    [Authorize(Roles = "company")]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserStage userStage = db.UserStages.Where(c=>c.Id== id).Include(c => c.Stage).FirstOrDefault();
            if (userStage == null)
            {
                return HttpNotFound();
            }

            UserStageDetailsViewModel userStageDetails=new UserStageDetailsViewModel();

            UserStageListViewModel list=new UserStageListViewModel()
            {
                StageTitle = userStage.Stage.Title,
                ActTitle = userStage.Stage.Act.Title,
                OperationTitle = userStage.Stage.Act.Operation.Title,
                ProjectTitle = userStage.Stage.Act.Operation.Project.Title,
                Id = userStage.Id,
                StatusTitle = userStage.RiskStatus.Title
            };


            userStageDetails.UserStageListViewModel = list;
                                         
            userStageDetails.UserStageListViewModel.StatusTitle = userStage.RiskStatus.Title;
            userStageDetails.SubmitDate = userStage.Stage.CreationDateStr;
            userStageDetails.ProbDesc = GetProbDesc();
            userStageDetails.IntenDesc = GetIntenDesc();
            userStageDetails.UserRisks = db.UserRisks.Include(c=>c.Risk).Where(c => c.UserStageId == id && c.IsDeleted == false).ToList();
            return View(userStageDetails);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }








    [Authorize(Roles = "supervisor")]
        public ActionResult IndexForSupervisor(Guid companyId)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

            string id = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

            Guid userId = new Guid(id);

          

        
            List<UserStage> userStages = db.UserStages.Include(u => u.RiskStatus)
                .Where(u =>  u.IsDeleted == false).OrderByDescending(u => u.CreationDate)
                .Include(u => u.Stage).Include(u => u.User).ToList();

            List<UserStageListViewModel> stages = new List<UserStageListViewModel>();

            foreach (UserStage userStage in userStages)
            {
                CompanyUser companyUser = db.CompanyUsers.FirstOrDefault(c =>
                    c.CompanyId == companyId && c.UserId == userStage.UserId && c.IsDeleted == false);


                if (companyUser != null)
                {
                    ViewBag.CompanyTitle = companyUser.Company.Title;
                    stages.Add(new UserStageListViewModel()
                    {
                        StageTitle = userStage.Stage.Title,
                        ActTitle = userStage.Stage.Act.Title,
                        OperationTitle = userStage.Stage.Act.Operation.Title,
                        ProjectTitle = userStage.Stage.Act.Operation.Project.Title,
                        Id = userStage.Id,
                        StatusTitle = userStage.RiskStatus.Title
                    });
                }
            }

            return View(stages);
        }

    [Authorize(Roles = "supervisor")]
        public ActionResult DetailsForSupervisor(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserStage userStage = db.UserStages.Where(c => c.Id == id).Include(c => c.Stage).FirstOrDefault();
            if (userStage == null)
            {
                return HttpNotFound();
            }

            UserStageDetailsViewModel userStageDetails = new UserStageDetailsViewModel();

            UserStageListViewModel list = new UserStageListViewModel()
            {
                StageTitle = userStage.Stage.Title,
                ActTitle = userStage.Stage.Act.Title,
                OperationTitle = userStage.Stage.Act.Operation.Title,
                ProjectTitle = userStage.Stage.Act.Operation.Project.Title,
                Id = userStage.Id,
                StatusTitle = userStage.RiskStatus.Title
            };


            userStageDetails.UserStageListViewModel = list;

            userStageDetails.UserStageListViewModel.StatusTitle = userStage.RiskStatus.Title;
            userStageDetails.SubmitDate = userStage.Stage.CreationDateStr;
            userStageDetails.ProbDesc = GetProbDesc();
            userStageDetails.IntenDesc = GetIntenDesc();
            userStageDetails.UserRisks = db.UserRisks.Include(c => c.Risk).Where(c => c.UserStageId == id && c.IsDeleted == false).ToList();
            return View(userStageDetails);
        }

        [HttpPost, ActionName("DetailsForSupervisor")]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsForSupervisor(Guid id, string submitButton)
        {
            UserStage userStage = db.UserStages.Find(id);
            if(submitButton== "accept")
            {
                userStage.RiskStatusId = db.RiskStatuses.FirstOrDefault(c => c.Code == 2).Id;
                userStage.LastModifiedDate = DateTime.Now;


                List<UserRisk> userRisks = db.UserRisks.Where(c => c.UserStageId == id).ToList();

                foreach (UserRisk userRisk in userRisks)
                {
                    userRisk.IsAcceptedBySupervisor = true;
                    userRisk.LastModifiedDate=DateTime.Now;
                }

                db.SaveChanges();
            }

           else if(submitButton== "reject")
            {
                userStage.RiskStatusId = db.RiskStatuses.FirstOrDefault(c => c.Code == 3).Id;
                userStage.LastModifiedDate = DateTime.Now;


                List<UserRisk> userRisks = db.UserRisks.Where(c => c.UserStageId == id).ToList();

                foreach (UserRisk userRisk in userRisks)
                {
                    userRisk.IsAcceptedBySupervisor = false;
                    userRisk.LastModifiedDate=DateTime.Now;
                }

                db.SaveChanges();
            }

            return RedirectToAction("CompanyList");
        }


    [Authorize(Roles = "supervisor")]
        public ActionResult CompanyList()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

            string id = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

            Guid userId = new Guid(id);


            var companies = db.Companies.Include(c => c.SupervisorUser).Where(c =>c.SupervisorUserId==userId&& c.IsDeleted == false).OrderByDescending(c => c.CreationDate);

            return View(companies.ToList());
        }

    }
}
