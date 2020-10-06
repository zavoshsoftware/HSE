using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class Company : BaseEntity
    {
        public Company()
        {
            Anomalies = new List<Anomaly>();
            Reports = new List<Report>();
            ContractRquirments = new List<ContractRquirment>();
            CompanyStatusReports = new List<CompanyStatusReport>();
            Users = new List<User>();
            Equipments = new List<Equipment>();
            CompanyHumanResources = new List<CompanyHumanResource>();
            Permits = new List<Permit>();
        }
        [Display(Name = "نام شرکت پیمانکار")]
        public string Title { get; set; }

        [Display(Name = "تعداد پرسونل رسمی")]
        public int? OfficialEmployee { get; set; }
        [Display(Name = "تعداد پرسونل قراردادی")]
        public int? ContractEmployee { get; set; }


        [Display(Name = "نام ناظر")]
        public Guid? SupervisorUserId { get; set; }
        public virtual User SupervisorUser { get; set; }

        //  public virtual ICollection<CompanyUser> CompanyUsers { get; set; }
        public virtual ICollection<Anomaly> Anomalies { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<ContractRquirment> ContractRquirments { get; set; }
        public virtual ICollection<CompanyStatusReport> CompanyStatusReports { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<CompanyHumanResource> CompanyHumanResources { get; set; }
        public virtual ICollection<Equipment> Equipments { get; set; }
        public virtual ICollection<Permit> Permits { get; set; }
        public virtual ICollection<Enviroment> Enviroments { get; set; }
        public virtual ICollection<Crisis> Crisises { get; set; }

        [Display(Name = "فایل مفاد پیمان")]
        public string ContractItemFileUrl { get; set; }

        [Display(Name = "چارت سازمانی")]
        public string ChartFileUrl { get; set; }
        internal class configuration : EntityTypeConfiguration<Company>
        {
            public configuration()
            {
                HasOptional(p => p.SupervisorUser).WithMany(j => j.Companies).HasForeignKey(p => p.SupervisorUserId);
            }
        }
    }
}