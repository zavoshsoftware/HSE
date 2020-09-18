using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class UserStageDetailsViewModel
    {
        public UserStageListViewModel UserStageListViewModel { get; set; }

        public List<UserRisk> UserRisks { get; set; }

        public string ProbDesc { get; set; }
        public string IntenDesc { get; set; }

        public string SubmitDate { get; set; }
    }
}