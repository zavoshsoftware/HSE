using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class AccidentTypeRelAccident : BaseEntity
    {
        public Accident Accident { get; set; }
        public Guid AccidentId { get; set; }

        public Guid AccidentTypeId { get; set; }
        public AccidentType AccidentType { get; set; }
    }
}