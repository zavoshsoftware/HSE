using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models 
{
    public class RiskProbability : BaseEntity
    {
        public RiskProbability()
        {
            RiskCalculators = new List<RiskCalculator>();
            UserRisks = new List<UserRisk>();
            AfterControlUserRisks = new List<UserRisk>();
        }
        [Display(Name="عنوان ")]
        public string Title { get; set; }

        [Display(Name="درجه")]
        public int Level { get; set; }

     

        [Display(Name = "توضیحات کیفی")]
        [DataType(DataType.MultilineText)]
        public string Summery { get; set; }

        [Display(Name = "توضیحات کمی")]
        [DataType(DataType.MultilineText)]
        public string Summery2 { get; set; }

        public virtual ICollection<RiskCalculator> RiskCalculators { get; set; }
        [InverseProperty("RiskProbability")]
        public virtual ICollection<UserRisk> UserRisks { get; set; }
        [InverseProperty("AfterControlRiskProbability")]
        public virtual ICollection<UserRisk> AfterControlUserRisks { get; set; }

    }
}