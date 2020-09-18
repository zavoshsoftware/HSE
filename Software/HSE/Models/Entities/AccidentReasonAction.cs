using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class AccidentReasonAction : BaseEntity
    {
        public string Title { get; set; } 
        public virtual ICollection<AccidentReasonActionRelAccident> AccidentReasonActionRelAccidents { get; set; }

    }
}