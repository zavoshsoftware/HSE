using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class AccidentReasonCondition : BaseEntity
    {
        public string Title { get; set; }
        public virtual ICollection<AccidentReasonConditionRelAccident> AccidentReasonConditionRelAccidents { get; set; }

    }
}