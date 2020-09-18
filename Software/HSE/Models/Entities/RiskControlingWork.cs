using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models 
{
    public class RiskControlingWork:BaseEntity
    {
        [Display(Name="عنوان اقدام کنترلی")]
        public string Title { get; set; }

        [Display(Name="کد")]
        public int Code { get; set; }

        [Display(Name="عنوان ریسک")]
        public Guid RiskId { get; set; }
        public virtual Risk Risk { get; set; }


        public int? OldId { get; set; }

        internal class configuration : EntityTypeConfiguration<RiskControlingWork>
        {
            public configuration()
            {
                HasRequired(p => p.Risk).WithMany(j => j.RiskControlingWorks).HasForeignKey(p => p.RiskId);
              
            }
        }
    }
}