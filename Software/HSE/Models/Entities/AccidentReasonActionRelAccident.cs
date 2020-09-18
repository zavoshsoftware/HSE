using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class AccidentReasonActionRelAccident:BaseEntity
    {
        public Accident Accident { get; set; }
        public Guid AccidentId { get; set; }

        public Guid AccidentReasonActionId { get; set; }
        public AccidentReasonAction AccidentReasonAction { get; set; }
    }
}