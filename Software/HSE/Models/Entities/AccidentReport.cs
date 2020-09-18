using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class AccidentReport : BaseEntity
    {
        [Display(Name = "نوع گزارش")]
        public string Title { get; set; }

        [Display(Name = "آخرین مهلت")]
        public string Deadline { get; set; }

        [Display(Name = "فرمت ارسالی")]
        public string DeliveryFormat { get; set; }

        [Display(Name = "مسئول  گزارش دهی")]
        public string ResponsibleReporter { get; set; }

        [Display(Name = "شماره فرم ")]
        public int No { get; set; }

        [Display(Name = "محل ارسال ")]
        public string SendPlace { get; set; }

        [Display(Name = "ضمائم")]
        public string Attachments { get; set; }

        public string BaseFileUrl { get; set; }

        public virtual ICollection<AccidentReportRelAccident> AccidentReportRelAccidents { get; set; }
    }
}