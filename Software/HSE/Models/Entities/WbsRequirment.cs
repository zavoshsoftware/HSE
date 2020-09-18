using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class WbsRequirment : BaseEntity
    {
        public WbsRequirment()
        {
            WbsUserTypes=new List<WbsUserType>();
        }
        [Display(Name="نوع الزامات")]
        public string Title { get; set; }

        public virtual ICollection<WbsUserType> WbsUserTypes { get; set; }
    }
}