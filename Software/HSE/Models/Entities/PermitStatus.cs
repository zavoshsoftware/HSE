using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace Models
{
    public class PermitStatus : BaseEntity
    {
        [Display(Name = "وضعیت")]
        public string Title { get; set; }
        public int Code { get; set; }

        public virtual ICollection<Permit> Permits { get; set; }
    }
}