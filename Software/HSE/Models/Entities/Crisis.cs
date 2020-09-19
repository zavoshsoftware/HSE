using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace Models
{
    public class Crisis : BaseEntity
    {
        [Display(Name = "پیمانکار")]
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }
 
        [Display(Name = "دسته بندی")]
        public Guid CrisisTypeId { get; set; }
        public virtual CrisisType CrisisType { get; set; }

        [Display(Name = "فایل")]
        public string FileUrl { get; set; }


        [Display(Name = "وضعیت")]
        public Guid PermitStatusId { get; set; }
        public virtual PermitStatus PermitStatus { get; set; }

        [Display(Name = "نظر ناظر")]
        [DataType(DataType.MultilineText)]
        public string SuperVisorComment { get; set; }

        internal class configuration : EntityTypeConfiguration<Crisis>
        {
            public configuration()
            {
                HasRequired(p => p.CrisisType).WithMany(j => j.Crisises).HasForeignKey(p => p.CrisisTypeId);
                HasRequired(p => p.Company).WithMany(j => j.Crisises).HasForeignKey(p => p.CompanyId);
            }
        }
    }
}