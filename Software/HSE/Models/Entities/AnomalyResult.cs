using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class AnomalyResult : BaseEntity
    {
        public AnomalyResult()
        {
            Anomalies=new List<Anomaly>();
        }
        public string Title { get; set; }
        [InverseProperty("AnomalyResult")]
        public virtual ICollection<Anomaly> Anomalies { get; set; }
    }
}