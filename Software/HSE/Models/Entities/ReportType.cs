using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class ReportType:BaseEntity
    {
        public ReportType()
        {
            Reports=new List<Report>();
            ReportTypes = new List<ReportType>();
        }
        [Display(Name="نوع گزارش")]
        public string Title { get; set; }
        public string Name { get; set; }
        public string SampleFile { get; set; }

        public Guid? ParentId { get; set; }
        public ReportType Parent { get; set; }
        public virtual ICollection<ReportType> ReportTypes { get; set; }

        public virtual ICollection<Report> Reports { get; set; }


        internal class configuration : EntityTypeConfiguration<ReportType>
        {
            public configuration()
            {
                HasRequired(p => p.Parent).WithMany(t => t.ReportTypes).HasForeignKey(p => p.ParentId);
            }
        }
    }
}