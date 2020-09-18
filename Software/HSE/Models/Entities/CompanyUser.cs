using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class CompanyUser : BaseEntity
    {
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [Display(Name="نام کاربر")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        [Display(Name="سمت")]
        public string PositionTitle { get; set; }

        internal class configuration : EntityTypeConfiguration<CompanyUser>
        {
            public configuration()
            {
                HasRequired(p => p.Company).WithMany(j => j.CompanyUsers).HasForeignKey(p => p.CompanyId);
                HasRequired(p => p.User).WithMany(j => j.CompanyUsers).HasForeignKey(p => p.UserId);
            }
        }
    }
}