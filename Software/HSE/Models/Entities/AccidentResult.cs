using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class AccidentResult : BaseEntity
    {
        public string Title { get; set; }
        public virtual ICollection<AccidentResultRelAccident> AccidentResultRelAccidents { get; set; }
    }
}