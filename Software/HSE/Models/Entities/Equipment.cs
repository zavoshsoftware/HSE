using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        internal class configuration : EntityTypeConfiguration<Equipment>
        {
            public configuration()
            {
                HasRequired(p => p.Company).WithMany(j => j.Equipments).HasForeignKey(p => p.CompanyId);
                HasRequired(p => p.EquipmentType).WithMany(j => j.Equipments).HasForeignKey(p => p.EquipmentTypeId);
            }
        }
    }
}