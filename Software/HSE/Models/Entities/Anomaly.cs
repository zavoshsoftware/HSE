using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class Anomaly : BaseEntity
    {
        public Anomaly()
        {
            AnomalyAttachments = new List<AnomalyAttachment>();
        }
        [Display(Name = "تاریخ")]
        public DateTime EventDate { get; set; }

        [Display(Name = "شماره")]
        public string Code { get; set; }

        [Display(Name = "شرح مختصری از گزارش عامل بالقوه آسیب رسان Anomaly")]
        [DataType(DataType.MultilineText)]
        public string Summery { get; set; }

        [Display(Name = "مکان")]
        public string Place { get; set; }


        [Display(Name = "مسئول پیگیری")]
        [ForeignKey("ResponseUser")]
        public Guid? ResponseUserId { get; set; }

        public virtual User ResponseUser { get; set; }


        [Display(Name = "گزارش دهنده")]
        [ForeignKey("CreatorUser")]
        public Guid? CreatorUserId { get; set; }

        public virtual User CreatorUser { get; set; }


        [Display(Name = "HSE")]
        public Guid AnomalyHseId { get; set; }

        public virtual AnomalyHse AnomalyHse { get; set; }


        [Display(Name = "سطح گزارش")]
        [ForeignKey("AnomalyLevel")]
        public Guid AnomalyLevelId { get; set; }

        [Display(Name = "مهلت اقدام")]
        public DateTime Deadline { get; set; }

        [NotMapped]
        [Display(Name = "مهلت اقدام")]
        public string DeadlineStr
        {
            get
            {
                //  return "hi";
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                string year = pc.GetYear(Deadline).ToString().PadLeft(4, '0');
                string month = pc.GetMonth(Deadline).ToString().PadLeft(2, '0');
                string day = pc.GetDayOfMonth(Deadline).ToString().PadLeft(2, '0');
                return String.Format("{0}/{1}/{2}", year, month, day);
            }
        }

        [NotMapped]
        [Display(Name = "تاریخ")]
        public string EventDateStr
        {
            get
            {
                //  return "hi";
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                string year = pc.GetYear(EventDate).ToString().PadLeft(4, '0');
                string month = pc.GetMonth(EventDate).ToString().PadLeft(2, '0');
                string day = pc.GetDayOfMonth(EventDate).ToString().PadLeft(2, '0');
                return String.Format("{0}/{1}/{2}", year, month, day);
            }
        }

        [Display(Name = "نتیجه")]
        [ForeignKey("AnomalyResult")]
        public Guid AnomalyResultId { get; set; }

        [Display(Name = "تاریخ احراز اثربخشی")]
        public DateTime? EffectivnessDate { get; set; }



        [NotMapped]
        [Display(Name = "تاریخ احراز اثربخشی")]
        public string EffectivnessDateStr
        {
            get
            {
                //  return "hi";
                if (EffectivnessDate != null)
                {
                    System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                    string year = pc.GetYear(EffectivnessDate.Value).ToString().PadLeft(4, '0');
                    string month = pc.GetMonth(EffectivnessDate.Value).ToString().PadLeft(2, '0');
                    string day = pc.GetDayOfMonth(EffectivnessDate.Value).ToString().PadLeft(2, '0');
                    return String.Format("{0}/{1}/{2}", year, month, day);
                }

                return String.Empty;
            }
        }



        [Display(Name = "تصاویر")]
        public string ImageUrl { get; set; }

        [Display(Name = "توضیحات ناظر")]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [Display(Name = "وضعیت")]
        public Guid StatusId { get; set; }
        public virtual Status Status { get; set; }

        [Display(Name = "شرکت پیمانکار")]
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public virtual AnomalyLevel AnomalyLevel { get; set; }
        public virtual AnomalyResult AnomalyResult { get; set; }


        [Display(Name = "توضیحات شرکت پیمانکار")]
        [DataType(DataType.MultilineText)]
        public string CompanyNotes { get; set; }

        public virtual ICollection<AnomalyAttachment> AnomalyAttachments { get; set; }
        internal class configuration : EntityTypeConfiguration<Anomaly>
        {
            public configuration()
            {
                HasRequired(p => p.Company).WithMany(j => j.Anomalies).HasForeignKey(p => p.CompanyId);
                HasRequired(p => p.Status).WithMany(j => j.Anomalies).HasForeignKey(p => p.StatusId);
                HasRequired(p => p.AnomalyLevel).WithMany(j => j.Anomalies).HasForeignKey(p => p.AnomalyLevelId);
                HasRequired(p => p.AnomalyHse).WithMany(j => j.Anomalies).HasForeignKey(p => p.AnomalyHseId);
                HasRequired(p => p.AnomalyResult).WithMany(j => j.Anomalies).HasForeignKey(p => p.AnomalyResultId);
                HasOptional(p => p.ResponseUser).WithMany(j => j.Anomalies).HasForeignKey(p => p.ResponseUserId);
                HasOptional(p => p.CreatorUser).WithMany(j => j.CreatorAnomalies).HasForeignKey(p => p.CreatorUserId);
            }
        }
    }
}