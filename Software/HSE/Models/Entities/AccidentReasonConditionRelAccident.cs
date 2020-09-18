using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class AccidentReasonConditionRelAccident : BaseEntity
    {
        public Accident Accident { get; set; }
        public Guid AccidentId { get; set; }

        public Guid AccidentReasonConditionId { get; set; }
        public AccidentReasonCondition AccidentReasonCondition { get; set; }
    }
}