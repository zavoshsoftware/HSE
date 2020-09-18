using System;
using System.Data.Entity.ModelConfiguration;

namespace Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CompanyHumanResource : BaseEntity
    {
        [Display(Name="نام")]
        public string FirstName { get; set; }
        [Display(Name="نام خانوادگی")]
        public string LastName { get; set; }
        [Display(Name = "رده شغلی")]
        public Guid UserJobRateId { get; set; }
        public virtual UserJobRate UserJobRate { get; set; }

        [Display(Name = "مدرک تحصیلی")]
        public string Degree { get; set; }

        [Display(Name = "رزومه")]
        public string ResumeFileUrl { get; set; }

        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }

        internal class configuration : EntityTypeConfiguration<CompanyHumanResource>
        {
            public configuration()
            {
                HasRequired(p => p.UserJobRate).WithMany(j => j.CompanyHumanResources).HasForeignKey(p => p.UserJobRateId);
            }
        }
    }
}
