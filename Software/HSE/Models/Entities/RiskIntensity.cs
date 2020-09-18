using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models 
{
    public class RiskIntensity : BaseEntity
    {
        public RiskIntensity()
        {
            RiskCalculators=new List<RiskCalculator>();
            UserRisks=new List<UserRisk>();
            AfterControlUserRisks = new List<UserRisk>();
        }
        [Display(Name="عنوان ")]
        public string Title { get; set; }

        [Display(Name="درجه")]
        public int Level { get; set; }

     

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Summery { get; set; }


        public virtual ICollection<RiskCalculator> RiskCalculators { get; set; }
        [InverseProperty("RiskIntensity")]
        public virtual ICollection<UserRisk> UserRisks { get; set; }

        [InverseProperty("AfterControlRiskIntensity")]
        public virtual ICollection<UserRisk> AfterControlUserRisks { get; set; }
 
    }
}

 