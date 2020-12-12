using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class ProgressGroup:BaseEntity
    {
        public ProgressGroup()
        {
            Progresses=new List<Progress>();
        }

        [Display(Name = "الزامات")]
        public string Title { get; set; }


        public decimal MaxAmount { get; set; }

        public virtual ICollection<Progress> Progresses { get; set; }
    }
}