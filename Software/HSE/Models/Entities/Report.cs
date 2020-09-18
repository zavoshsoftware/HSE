using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class Report : BaseEntity
    {
        [Display(Name="تاریخ گزارش")]
        public DateTime ReportDate { get; set; }

        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public string FileUrl { get; set; }

        public Guid StatusId { get; set; }
        public virtual Status Status { get; set; }

        public Guid ReportTypeId { get; set; }
        public virtual ReportType ReportType { get; set; }


        internal class configuration : EntityTypeConfiguration<Report>
        {
            public configuration()
            {
                HasRequired(p => p.Company).WithMany(j => j.Reports).HasForeignKey(p => p.CompanyId);
                HasRequired(p => p.Status).WithMany(j => j.Reports).HasForeignKey(p => p.StatusId);
                HasRequired(p => p.ReportType).WithMany(j => j.Reports).HasForeignKey(p => p.ReportTypeId);
            }
        }
    }
}