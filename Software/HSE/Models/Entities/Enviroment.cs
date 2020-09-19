using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace Models
{
    public class Enviroment : BaseEntity
    {
        [Display(Name = "پیمانکار")]
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }
 
        [Display(Name = "دسته بندی")]
        public Guid EnviromentTypeId { get; set; }
        public virtual EnviromentType EnviromentType { get; set; }

        [Display(Name = "فایل")]
        public string FileUrl { get; set; }


        [Display(Name = "وضعیت")]
        public Guid PermitStatusId { get; set; }
        public virtual PermitStatus PermitStatus { get; set; }

        [Display(Name = "نظر ناظر")]
        [DataType(DataType.MultilineText)]
        public string SuperVisorComment { get; set; }

        internal class configuration : EntityTypeConfiguration<Enviroment>
        {
            public configuration()
            {
                HasRequired(p => p.EnviromentType).WithMany(j => j.Enviroments).HasForeignKey(p => p.EnviromentTypeId);
                HasRequired(p => p.Company).WithMany(j => j.Enviroments).HasForeignKey(p => p.CompanyId);
            }
        }
    }
}