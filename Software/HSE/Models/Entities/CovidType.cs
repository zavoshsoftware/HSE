using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class CovidType:BaseEntity
    {
        public CovidType()
        {
            Covids=new List<Covid>();
        }
        [Display(Name="مشکوک / مبتلا")]
        public string Title { get; set; }

        public virtual ICollection<Covid> Covids { get; set; }
    }
}