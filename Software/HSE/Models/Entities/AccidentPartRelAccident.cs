using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class AccidentPartRelAccident : BaseEntity
    {
        public Accident Accident { get; set; }
        public Guid AccidentId { get; set; }

        public Guid AccidentPartId { get; set; }
        public AccidentPart AccidentPart { get; set; }
    }
}