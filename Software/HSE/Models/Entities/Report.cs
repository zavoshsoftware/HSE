using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class Report : BaseEntity
    {
        [Display(Name="تاریخ گزارش")]
        public DateTime ReportDate { get; set; }

        [Display(Name = "تاریخ گزارش")]
        [NotMapped]
        public string ReportDateStr
        {
            get
            {
                //  return "hi";
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                string year = pc.GetYear(ReportDate).ToString().PadLeft(4, '0');
                string month = pc.GetMonth(ReportDate).ToString().PadLeft(2, '0');
                string day = pc.GetDayOfMonth(ReportDate).ToString().PadLeft(2, '0');
                return String.Format("{0}/{1}/{2}", year, month, day) ;
            }
        }

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