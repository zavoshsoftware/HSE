using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class SafetyType:BaseEntity
    {
        [Display(Name="نوع ایمنی")]
        public string Title { get; set; }

        public virtual ICollection<Safety> Safeties { get; set; }

        public SafetyType()
        {
            Safeties=new List<Safety>();
        }
    }
}