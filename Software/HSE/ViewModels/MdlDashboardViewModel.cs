using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace ViewModels
{
    public class MdlDashboardViewModel
    {
      
        public SelectList CompanyTypes { get; set; }

        public List<MdlTableViewModel> MdrItems { get; set; }
        public Guid SelectedCompanyTypeId { get; set; }
    }

    public class MdlTableViewModel
    {
        public Guid CompanyId { get; set; }
        public string CompanyTitle { get; set; }
        public bool HsePlan { get; set; }
        public bool RiskAssessment { get; set; }
        public bool Erp { get; set; }
        public bool Esr { get; set; }
    }
    
}