using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class AccidentInjury : BaseEntity
    {
        public string Title { get; set; }
        public virtual ICollection<AccidentInjuryRelAccident> AccidentInjuryRelAccidents { get; set; }

    }
}