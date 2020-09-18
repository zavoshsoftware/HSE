using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models 
{
    public class Act : BaseEntity
    {
        [Display(Name="عنوان فعالیت")]
        public string Title { get; set; }

        [Display(Name="کد")]
        public int Code { get; set; }

        [Display(Name="عنوان عملیات")]
        public Guid OperationId { get; set; }
        public virtual Operation Operation { get; set; }
        public virtual ICollection<Stage> Stages { get; set; }

        [Display(Name = "تجهیزات حفاظت فردی")]
        public string ProtectionEquipment { get; set; }

        [Display(Name = "دوره های آموزشی")]
        public string Courses { get; set; }

        public int? OldId { get; set; }

        internal class configuration : EntityTypeConfiguration<Act>
        {
            public configuration()
            {
                HasRequired(p => p.Operation).WithMany(j => j.Acts).HasForeignKey(p => p.OperationId);
              
            }
        }
    }
}