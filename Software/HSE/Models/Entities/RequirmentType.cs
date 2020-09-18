using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class RequirmentType : BaseEntity
    {
        public RequirmentType()
        {
            Requirments = new List<Requirment>();
        }
        public int Order { get; set; }
        public string Title { get; set; }
        public decimal Weight { get; set; }

        public virtual ICollection<Requirment> Requirments { get; set; }
    }
}