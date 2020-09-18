using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using ViewModels;
using System.Data.Entity;

namespace HSE.Controllers
{
    [Authorize(Roles = "Administrator,company,supervisor")]
    public class RequirmentDetailsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();
        // GET: RequirmentDetails
        public ActionResult Index(Guid id)
        {
          
            List<RequirmentDetailItemViewModel> requirmentDetailItems = new List<RequirmentDetailItemViewModel>();

            List<RequirmentType> requirmentTypes = db.RequirmentTypes.Where(c => c.IsDeleted == false && c.IsActive)
                .OrderBy(c => c.Order).ToList();

            foreach (RequirmentType requirmentType in requirmentTypes)
            {
                requirmentDetailItems.Add(new RequirmentDetailItemViewModel()
                {
                    RequirmentType = requirmentType,
                    Requirments = GetRequirments(requirmentType.Id, id)

                });
            }
            ViewBag.contractId = id;
            return View(requirmentDetailItems);
        }

        public List<RequirmentItemViewModel> GetRequirments(Guid rquirmentTypeId, Guid contractRequirmentId)
        {
            List<RequirmentItemViewModel> requirmentItems = new List<RequirmentItemViewModel>();

            List<Requirment> requirments = db.Requirments
                .Where(c => c.RequirmentTypeId == rquirmentTypeId && c.IsDeleted == false)
                .ToList();

            foreach (Requirment requirment in requirments)
            {
                RequirmentDetail rd = db.RequirmentDetails.Include(c=>c.ContractRquirment)
                    .Where(c => c.RequirmentId == requirment.Id&&c.ContractRquirmenttId== contractRequirmentId)
                    .OrderByDescending(c => c.CreationDate).FirstOrDefault();

                string progressWeight = "0";
                string progressAmount = "0";

                if (rd != null)
                {
                    progressAmount = rd.TotalProgressAmount.ToString();
                    progressWeight = rd.TotalProgressPercent.ToString();
                }
                requirmentItems.Add(new RequirmentItemViewModel()
                {
                    Id = requirment.Id,
                    Title = requirment.Title,
                    Weight = requirment.Weight.ToString(),
                    ProgressWeight = progressWeight,
                    ProgressAmount = progressAmount
                });
            }

            return requirmentItems;
        }

        public Guid GetCompanyId()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string id = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

            Guid userId = new Guid(id);

            User user = db.Users.FirstOrDefault(c => c.Id == userId);

            return user.CompanyId.Value;
        }

        [Route("create/{id:Guid}/{coId:Guid}")]
        public ActionResult Create(Guid id,Guid coId)
        {
            Requirment rd = db.Requirments.Find(id);

            ViewBag.title = rd?.Title;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create/{id:Guid}/{coId:Guid}")]
        public ActionResult Create(RequirmentDetail requirmentDetail, Guid id,Guid coId)
        {
            RequirmentDetail latest = db.RequirmentDetails
                .Where(c => c.RequirmentId == id && c.ContractRquirmenttId == coId)
                .OrderByDescending(c => c.CreationDate).FirstOrDefault();

            decimal totalProgress = requirmentDetail.ProgressPercent;
            decimal totalAmount = requirmentDetail.ProgressAmount;

            if (latest != null)
            {
                totalProgress = latest.TotalProgressPercent+ requirmentDetail.ProgressPercent;
                totalAmount = latest.TotalProgressAmount+ requirmentDetail.ProgressAmount;
            }


            Guid companyId = GetCompanyId();
            if (ModelState.IsValid)
            {
                requirmentDetail.IsDeleted = false;
                requirmentDetail.RequirmentId = id;


                requirmentDetail.TotalProgressAmount = totalAmount;

                requirmentDetail.TotalProgressPercent = totalProgress;

                requirmentDetail.ContractRquirmenttId =coId;

                requirmentDetail.CreationDate=DateTime.Now;
                requirmentDetail.Id = Guid.NewGuid();
                db.RequirmentDetails.Add(requirmentDetail);
                db.SaveChanges();
                return RedirectToAction("Index",new{id= coId });
            }

            return View();
        }
    }
}