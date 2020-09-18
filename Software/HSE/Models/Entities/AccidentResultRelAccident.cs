using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class AccidentResultRelAccident : BaseEntity
    {
        public Accident Accident { get; set; }
        public Guid AccidentId { get; set; }

        public Guid AccidentResultId { get; set; }
        public AccidentResult AccidentResult { get; set; }
    }
}