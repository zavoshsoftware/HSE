using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class HseDocument : BaseEntity
    {
        [Display(Name = "کاربر")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        
        [Display(Name = "شرکت پیمانکار")]
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [Display(Name = "بارگزاری سند ")]
        public string FileUrl { get; set; }

        [Display(Name = "نظر ناظر")]
        [DataType(DataType.MultilineText)]
        public string SupervisorComment { get; set; }

        [Display(Name = "نوع سند")]
        public Guid HseDocumentTypeId { get; set; }
        public virtual HseDocumentType HseDocumentType { get; set; }
    }
}