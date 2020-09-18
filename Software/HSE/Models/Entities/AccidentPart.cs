using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class AccidentPart : BaseEntity
    { 
        public string Title { get; set; }
        public virtual ICollection<AccidentPartRelAccident> AccidentPartRelAccidents { get; set; }

    }
}