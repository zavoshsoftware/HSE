using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models 
{
    public class Stage : BaseEntity
    {
        [Display(Name="عنوان مرحله انجام کار")]
        public string Title { get; set; }

        [Display(Name="کد")]
        public int Code { get; set; }

        [Display(Name="عنوان عملیات")]
        public Guid ActId { get; set; }
        public virtual Act Act { get; set; }
        public virtual ICollection<Risk> Risks { get; set; }

        public int? OldId { get; set; }

        public virtual ICollection<UserStage> UserStages { get; set; }

        internal class configuration : EntityTypeConfiguration<Stage>
        {
            public configuration()
            {
                HasRequired(p => p.Act).WithMany(j => j.Stages).HasForeignKey(p => p.ActId);
              
            }
        }
    }
}