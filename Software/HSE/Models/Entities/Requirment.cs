using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class Requirment : BaseEntity
    {
        public Requirment()
        {
            RequirmentDetails=new List<RequirmentDetail>();
        }
        [Display(Name="اولویت")]
        public int Order { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name="عنوان الزام")]
        public string Title { get; set; }
        [Display(Name="وزن")]
        public decimal Weight { get; set; }
        

        public Guid RequirmentTypeId { get; set; }
        public virtual RequirmentType RequirmentType { get; set; }
        public virtual ICollection<RequirmentDetail> RequirmentDetails { get; set; }

    }
}