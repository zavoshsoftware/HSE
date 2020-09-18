using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models 
{
    public class Operation : BaseEntity
    {
        [Display(Name="عنوان عملیات")]
        public string Title { get; set; }

        [Display(Name="کد")]
        public int Code { get; set; }

        [Display(Name="پروژه")]
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<Act> Acts { get; set; }

        public int? OldId { get; set; }

        internal class configuration : EntityTypeConfiguration<Operation>
        {
            public configuration()
            {
                HasRequired(p => p.Project).WithMany(j => j.Operations).HasForeignKey(p => p.ProjectId);
              
            }
        }
    }
}