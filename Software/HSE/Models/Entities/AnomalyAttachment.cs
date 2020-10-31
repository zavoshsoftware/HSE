using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class AnomalyAttachment : BaseEntity
    {
        [Display(Name = "پیوست")]
        public string ImageUrl { get; set; }
        public Guid AnomalyId { get; set; }
        public virtual Anomaly Anomaly { get; set; }

        internal class configuration : EntityTypeConfiguration<AnomalyAttachment>
        {
            public configuration()
            {
                HasRequired(p => p.Anomaly).WithMany(j => j.AnomalyAttachments).HasForeignKey(p => p.AnomalyId);
            }
        }
    }
}