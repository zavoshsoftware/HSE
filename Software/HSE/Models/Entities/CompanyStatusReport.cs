using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class CompanyStatusReport:BaseEntity
    {
        [Display(Name="فایل صورت وضعیت")]
        public string FileUrl { get; set; }

        [Display(Name="عنوان صورت وضعیت")]
        public string Title { get; set; }

        [Display(Name="پیمانکار")]
        public virtual Company Company { get; set; }
        public Guid CompanyId { get; set; }
    }
}