using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace Models
{
    public class Permit : BaseEntity
    {
        [Display(Name = "پیمانکار")]
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }
 
        [Display(Name = "دسته بندی")]
        public Guid PermitTypeId { get; set; }
        public virtual PermitType PermitType { get; set; }

        [Display(Name = "مجوز")]
        public string FileUrl { get; set; }


        [Display(Name = "وضعیت")]
        public Guid PermitStatusId { get; set; }
        public virtual PermitStatus PermitStatus { get; set; }

        [Display(Name = "نظر ناظر")]
        [DataType(DataType.MultilineText)]
        public string SuperVisorComment { get; set; }

        internal class configuration : EntityTypeConfiguration<Permit>
        {
            public configuration()
            {
                HasRequired(p => p.PermitType).WithMany(j => j.Permits).HasForeignKey(p => p.PermitTypeId);
                HasRequired(p => p.Company).WithMany(j => j.Permits).HasForeignKey(p => p.CompanyId);
            }
        }
    }
}