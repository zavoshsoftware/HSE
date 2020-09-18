 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class UserRisk:BaseEntity
    {

         public Guid UserStageId { get; set; }
         public virtual UserStage UserStage { get; set; }

        public Guid RiskId { get; set; }
        public virtual Risk Risk { get; set; }


        [ForeignKey("RiskIntensity")]
        public Guid RiskIntensityId { get; set; }
        public virtual RiskIntensity RiskIntensity { get; set; }

        [ForeignKey("RiskProbability")]
        public Guid RiskProbabilityId { get; set; }
        public virtual RiskProbability RiskProbability { get; set; }

        public int  RiskNumber { get; set; }
        public string RiskDescription { get; set; }

        [ForeignKey("AfterControlRiskIntensity")]
        public Guid? AfterControlRiskIntensityId { get; set; }
        public virtual RiskIntensity AfterControlRiskIntensity { get; set; }


        [ForeignKey("AfterControlRiskProbability")]
        public Guid? AfterControlRiskProbabilityId { get; set; }
        public virtual RiskProbability AfterControlRiskProbability { get; set; }

        public int? AfterControlRiskNumber { get; set; }
        public string AfterControlRiskDescription { get; set; }


        public bool IsAcceptedBySupervisor { get; set; }
        
        internal class configuration : EntityTypeConfiguration<UserRisk>
        {
            public configuration()
            {
                HasRequired(p => p.UserStage).WithMany(j => j.UserRisks).HasForeignKey(p => p.UserStageId);
                HasRequired(p => p.Risk).WithMany(j => j.UserRisks).HasForeignKey(p => p.RiskId);
                HasRequired(p => p.RiskIntensity).WithMany(j => j.UserRisks).HasForeignKey(p => p.RiskIntensityId);
                HasRequired(p => p.RiskProbability).WithMany(j => j.UserRisks).HasForeignKey(p => p.RiskProbabilityId);
                HasOptional(p => p.AfterControlRiskIntensity).WithMany(j => j.AfterControlUserRisks).HasForeignKey(p => p.AfterControlRiskIntensityId);
                HasOptional(p => p.AfterControlRiskProbability).WithMany(j => j.AfterControlUserRisks).HasForeignKey(p => p.AfterControlRiskProbabilityId);

            }
        }
    }
}