using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class Covid:BaseEntity
    {
        [Display(Name="نام و نام خانوادگی")]
        public string FullName { get; set; }
        [Display(Name="تاریخ ابتلا")]
        [UIHint("PersianDatePicker")]
        public DateTime? SickDate { get; set; }
        [Display(Name="تاریخ بهبود")]
        [UIHint("PersianDatePicker")]
        public DateTime? SafeDate { get; set; }
        [Display(Name="مدت زمان قرنطینه (روز)")]
        public int QuarantineDays { get; set; }
        [Display(Name="مشکوک / مبتلا")]
        public Guid CovidTypeId { get; set; }
        public virtual CovidType CovidType { get; set; }

        [Display(Name="فرم")]
        public string ImageUrl { get; set; }
        [Display(Name="وضعیت")]
        public Guid CovidStatusId { get; set; }
        public virtual CovidStatus CovidStatus { get; set; }

        [Display(Name="پیمانکار")]
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}