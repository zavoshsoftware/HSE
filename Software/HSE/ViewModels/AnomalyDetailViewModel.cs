using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class AnomalyDetailViewModel
    {
     

        [Display(Name = "شماره")]
        public string Code { get; set; }

        [Display(Name = "شرح مختصری از گزارش عامل بالقوه آسیب رسان Anomaly")]
        [DataType(DataType.MultilineText)]
        public string Summery { get; set; }

        [Display(Name = "مکان")]
        public string Place { get; set; }


        [Display(Name = "مسئول پیگیری")]
        public string ResponseUserFullName { get; set; }



        [Display(Name = "گزارش دهنده")]
        public string CreatorUserFullName { get; set; }



        [Display(Name = "HSE")]
        public string AnomalyHseTitle { get; set; }



        [Display(Name = "سطح گزارش")]
        public string AnomalyLevelTitle { get; set; }

                [Display(Name = "مهلت اقدام")]
        public string DeadlineStr { get; set; }
        
        [Display(Name = "تاریخ")]
        public string EventDateStr { get; set; }

        [Display(Name = "نتیجه")]
        [ForeignKey("AnomalyResult")]
        public string AnomalyResultTitle { get; set; }


        [Display(Name = "تاریخ احراز اثربخشی")]
        [UIHint("PersianDatePicker")]
        public DateTime? EffectivnessDate { get; set; }
        [Display(Name = "تاریخ احراز اثربخشی")]
        public string EffectivnessDateStr { get; set; }

        [Display(Name = "تصاویر")]
        public string ImageUrl { get; set; }

        [Display(Name = "توضیحات ناظر")]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [Display(Name = "وضعیت")]
        public string StatusTitle { get; set; }

        [Display(Name = "شرکت پیمانکار")]
        public string CompanyTitle { get; set; }

        


        [Display(Name = "توضیحات شرکت پیمانکار")]
        [DataType(DataType.MultilineText)]
        public string CompanyNotes { get; set; }
        [Display(Name = "تاریخ ثبت")]
        public string CreationDateStr { get; set; }
        [Display(Name = "یادداشت")]
        public string Description { get; set; }

        public virtual List<AnomalyAttachment> AnomalyAttachments { get; set; }

        public int StatusCode { get; set; }
        public Guid Id { get; set; }
        public Guid AnomalyResultId { get; set; }
    
    }
}