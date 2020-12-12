using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class Progress:BaseEntity
    {
        [Display(Name="شروع دوره")]
        [UIHint("PersianDatePicker")]
        public DateTime Start { get; set; }

        [Display(Name="پایان دوره")]
        [UIHint("PersianDatePicker")]
        public DateTime Finish { get; set; }
       
        [Display(Name="درصد پیشرفت پیمانکار")]
        public decimal CompanyPercent { get; set; }

        [Display(Name="درصد پیشرفت تایید شده ناظر")]
        public decimal? SupPercent { get; set; }

        [Display(Name="درصد پیشرفت تایید شده کارفرما")]
        public decimal? AdminPercent { get; set; }

        [Display(Name="جمع کل درصد پیشرقت")]
        public decimal Total { get; set; }

        [Display(Name = "پیمانکار")]
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [Display(Name = "الزامات")]
        public Guid ProgressGroupId { get; set; }
        public virtual ProgressGroup ProgressGroup { get; set; }

        [Display(Name = "ضمیمه")]
        public string ImageUrl { get; set; }
    }
}