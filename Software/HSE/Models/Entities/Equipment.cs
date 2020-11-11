using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class Equipment : BaseEntity
    {
        [Display(Name="تجهیزات و ماشین آلات")]
        public string Title { get; set; }

        [Display(Name = "گواهینامه")]
        public string CertificateFileUrl { get; set; }

        [Display(Name = "پیمانکار")]
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [Display(Name = "دسته بندی")]
        public Guid EquipmentTypeId { get; set; }
        public virtual EquipmentType EquipmentType { get; set; }

        [Display(Name = "نظر ناظر")]
        [DataType(DataType.MultilineText)]
        public string SupervisorComment { get; set; }
        [Display(Name= "تاریخ خاتمه تاييد صلاحت ")]
        public DateTime FinishDate { get; set; }
        internal class configuration : EntityTypeConfiguration<Equipment>
        {
            public configuration()
            {
                HasRequired(p => p.Company).WithMany(j => j.Equipments).HasForeignKey(p => p.CompanyId);
                HasRequired(p => p.EquipmentType).WithMany(j => j.Equipments).HasForeignKey(p => p.EquipmentTypeId);
            }
        }

        [Display(Name = "تاریخ خاتمه تاييد صلاحت ")]
        [NotMapped]
        public string FinishDateStr
        {
            get
            {
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                string year = pc.GetYear(FinishDate).ToString().PadLeft(4, '0');
                string month = pc.GetMonth(FinishDate).ToString().PadLeft(2, '0');
                string day = pc.GetDayOfMonth(FinishDate).ToString().PadLeft(2, '0');
                return String.Format("{0}/{1}/{2}", year, month, day);
            }
        }
    }
}