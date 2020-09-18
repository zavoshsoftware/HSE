
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class PermitType : BaseEntity
    {
        [Display(Name="نوع مجوز")]
        public string Title { get; set; }

        [Display(Name="نمونه فرم")]
        public string FileUrl { get; set; }

        public virtual ICollection<Permit> Permits { get; set; }

      
    }
}