using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class AccidentReportRelAccident : BaseEntity
    {
        public Accident Accident { get; set; }
        public Guid AccidentId { get; set; }

        public Guid AccidentReportId { get; set; }
        public AccidentReport AccidentReport { get; set; }

        public string FileUrl { get; set; }
    }
}