using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class CovidStatus:BaseEntity
    {
        public CovidStatus()
        {
            Covids = new List<Covid>();
        }
        [Display(Name="وضعیت")]
        public string Title { get; set; }
        public virtual ICollection<Covid> Covids { get; set; }

    }
}