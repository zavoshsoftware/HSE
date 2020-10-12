using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class PassiveDefenseType:BaseEntity
    {
        public PassiveDefenseType()
        {
            PassiveDefenses=new List<PassiveDefense>();
        }

        [Display(Name="نوع فایل")]
        public string Title { get; set; }

        public virtual ICollection<PassiveDefense> PassiveDefenses { get; set; }
    }
}