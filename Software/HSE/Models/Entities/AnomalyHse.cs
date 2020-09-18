using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class AnomalyHse : BaseEntity
    {
        public AnomalyHse()
        {
            Anomalies=new List<Anomaly>();
        }
        public string Title { get; set; }
        public virtual ICollection<Anomaly> Anomalies { get; set; }
    }
}