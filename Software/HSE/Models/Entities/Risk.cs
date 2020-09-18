using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models 
{
    public class Risk : BaseEntity
    {
        [Display(Name="عنوان ریسک")]
        public string Title { get; set; }

        [Display(Name="کد")]
        public int Code { get; set; }

        [Display(Name="عنوان مرحله انجام کار")]
        public Guid StageId { get; set; }
        public virtual Stage Stage { get; set; }

        public virtual ICollection<UserRisk> UserRisks { get; set; }
        public virtual ICollection<RiskControlingWork> RiskControlingWorks { get; set; }
        public int? OldId { get; set; }

        internal class configuration : EntityTypeConfiguration<Risk>
        {
            public configuration()
            {
                HasRequired(p => p.Stage).WithMany(j => j.Risks).HasForeignKey(p => p.StageId);
            }
        }
    }
}