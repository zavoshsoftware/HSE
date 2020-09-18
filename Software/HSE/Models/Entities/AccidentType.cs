using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class AccidentType : BaseEntity
    {
        public string Title { get; set; }
        public virtual ICollection<AccidentTypeRelAccident> AccidentTypeRelAccidents { get; set; }
    }
}