using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class UserStage : BaseEntity
    {

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public Guid?  StageId { get; set; }
        public virtual Stage Stage { get; set; }

        public Guid RiskStatusId { get; set; }
        public virtual RiskStatus RiskStatus { get; set; }

        public virtual ICollection<UserRisk> UserRisks { get; set; }
        internal class configuration : EntityTypeConfiguration<UserStage>
        {
            public configuration()
            {
                HasRequired(p => p.User).WithMany(j => j.UserStages).HasForeignKey(p => p.UserId);
                HasOptional(p => p.Stage).WithMany(j => j.UserStages).HasForeignKey(p => p.StageId);
              HasRequired(p => p.RiskStatus).WithMany(j => j.UserStages).HasForeignKey(p => p.RiskStatusId);
            }
        }
    }
}