using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class ReportType:BaseEntity
    {
        public ReportType()
        {
            Reports=new List<Report>();
        }
        public string Title { get; set; }
        public string Name { get; set; }
        public string SampleFile { get; set; }

        public virtual ICollection<Report> Reports { get; set; }
    }
}