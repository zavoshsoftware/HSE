using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Display(Name = "تاریخ ابتلا")]
        [NotMapped]
        public string SickDateStr
        {
            get
            {
                if (SickDate == null)
                    return string.Empty;

                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                string year = pc.GetYear(SickDate.Value).ToString().PadLeft(4, '0');
                string month = pc.GetMonth(SickDate.Value).ToString().PadLeft(2, '0');
                string day = pc.GetDayOfMonth(SickDate.Value).ToString().PadLeft(2, '0');
                return String.Format("{0}/{1}/{2}", year, month, day) ;
            }
        }
        [Display(Name = "تاریخ بهبود")]
        [NotMapped]
        public string SafeDateStr
        {
            get
            {
                if (SafeDate == null)
                    return string.Empty;

                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                string year = pc.GetYear(SafeDate.Value).ToString().PadLeft(4, '0');
                string month = pc.GetMonth(SafeDate.Value).ToString().PadLeft(2, '0');
                string day = pc.GetDayOfMonth(SafeDate.Value).ToString().PadLeft(2, '0');
                return String.Format("{0}/{1}/{2}", year, month, day) ;
            }
        }
    }
}