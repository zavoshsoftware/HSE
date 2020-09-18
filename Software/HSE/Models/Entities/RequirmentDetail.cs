using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class RequirmentDetail : BaseEntity
    {
        [Display(Name="درصد پیشرفت طی دوره")]
        public decimal ProgressPercent { get; set; }
        [Display(Name="مبلغ پیشرفت طی دوره")]
        public decimal ProgressAmount { get; set; }
        public decimal TotalProgressPercent { get; set; }
        public decimal TotalProgressAmount { get; set; }
   
        public Guid ContractRquirmenttId { get; set; }
        public virtual ContractRquirment ContractRquirment { get; set; }

        public Guid RequirmentId { get; set; }
        public virtual Requirment Requirment { get; set; }
    }
}