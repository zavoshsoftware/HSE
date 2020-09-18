using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class Status:BaseEntity
    {
        public Status()
        {
            Reports=new List<Report>();
            Anomalies = new List<Anomaly>();
        }
        public int Order { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Anomaly> Anomalies { get; set; }
    }
}