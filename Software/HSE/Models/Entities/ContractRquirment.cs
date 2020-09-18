using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Models
{
    public class ContractRquirment:BaseEntity
    {
        public ContractRquirment()
        {
            RequirmentDetails=new List<RequirmentDetail>();
        }
        [Display(Name="نام پروژه")]
        public string ProjectTitle { get; set; }
        [Display(Name="شماره پیمان")]
        public string Code { get; set; }
        [Display(Name="تاریخ قرارداد")]
        public DateTime ContractDate { get; set; }
        [NotMapped]
        [Display(Name = "تاریخ قرارداد")]
        public string ContractDateStr
        {
            get
            {
                //  return "hi";
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                string year = pc.GetYear(ContractDate).ToString().PadLeft(4, '0');
                string month = pc.GetMonth(ContractDate).ToString().PadLeft(2, '0');
                string day = pc.GetDayOfMonth(ContractDate).ToString().PadLeft(2, '0');
                return String.Format("{0}/{1}/{2}", year, month, day);
            }
        }

        [NotMapped]
        [Display(Name = "تاریخ شروع به کار")]
        public string StartDateStr
        {
            get
            {
                //  return "hi";
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                string year = pc.GetYear(StartDate).ToString().PadLeft(4, '0');
                string month = pc.GetMonth(StartDate).ToString().PadLeft(2, '0');
                string day = pc.GetDayOfMonth(StartDate).ToString().PadLeft(2, '0');
                return String.Format("{0}/{1}/{2}", year, month, day);
            }
        }
        [Display(Name="تاریخ شروع به کار")]
        public DateTime StartDate { get; set; }
        [Display(Name="پیمانکار")]
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [Display(Name="مبلغ کل")]
        public decimal TotalAmount { get; set; }

        [NotMapped]
        [Display(Name="مبلغ کل")]
        public string TotalAmountStr
        {
            get { return TotalAmount.ToString("N0"); }
        }

        public virtual ICollection<RequirmentDetail> RequirmentDetails { get; set; }
    }
}