using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class UserRiskReportViewModel
    {
        public List<UserRiskItem> UserRiskItems { get; set; }
        public string ProbDesc { get; set; }
        public string IntenDesc { get; set; }
    }

    public class UserRiskItem
    {
        public string ProjectTitle { get; set; }
        public string OperationtTitle { get; set; }
        public string ActTitle { get; set; }
        public string StageTitle { get; set; }
        public List<UserRisk> UserRisks { get; set; }
    }
}