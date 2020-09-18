using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Models
{
    public class RiskMatris:BaseEntity
    {
        public int RiskNumber { get; set; }
        public RiskProbability RiskProbability { get; set; }
        public Guid RiskProbabilityId { get; set; }
        public RiskIntensity RiskIntensity { get; set; }
        public Guid RiskIntensityId { get; set; }
    }
}