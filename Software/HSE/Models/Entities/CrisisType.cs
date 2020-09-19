
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class CrisisType : BaseEntity
    {
        [Display(Name="دسته بندی")]
        public string Title { get; set; }
 

        public virtual ICollection<Crisis> Crisises { get; set; }

      
    }
}