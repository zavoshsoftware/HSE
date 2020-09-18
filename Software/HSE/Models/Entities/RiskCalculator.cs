using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class RiskCalculator : BaseEntity
    {

        public Guid RiskIntensityId { get; set; }
        public virtual RiskIntensity RiskIntensity { get; set; }

        public Guid RiskProbabilityId { get; set; }
        public virtual RiskProbability RiskProbability { get; set; }

        public int RiskNumber { get; set; }
        public int RiskNumberDescription { get; set; }

        internal class configuration : EntityTypeConfiguration<RiskCalculator>
        {
            public configuration()
            {
                HasRequired(p => p.RiskIntensity).WithMany(j => j.RiskCalculators).HasForeignKey(p => p.RiskIntensityId);
                HasRequired(p => p.RiskProbability).WithMany(j => j.RiskCalculators).HasForeignKey(p => p.RiskProbabilityId);

            }
        }
    }
}