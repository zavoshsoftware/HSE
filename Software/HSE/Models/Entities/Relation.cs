using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Web.Mvc;

namespace Models
{
    public class Relation : BaseEntity
    {
        public Relation()
        {
            RelationImages=new List<RelationImage>();
        }
        [Display(Name = "دسته بندی")]
        public Guid RelationTypeId { get; set; }
        public virtual RelationType RelationType { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "فایل صورتجلسه")]
        public string FileUrl { get; set; }

        [Display(Name = "ActionPlan")]
        public string ActionPlanFileUrl { get; set; }

        [Display(Name = "نام مدرس")]
        public string TeacherName { get; set; }

        [Display(Name = "شرکت پیمانکار")]
        public Guid? CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<RelationImage> RelationImages { get; set; }
        internal class configuration : EntityTypeConfiguration<Relation>
        {
            public configuration()
            {
                HasRequired(p => p.RelationType).WithMany(j => j.Relations).HasForeignKey(p => p.RelationTypeId);
                HasOptional(p => p.Company).WithMany(j => j.Relations).HasForeignKey(p => p.CompanyId);
            }
        }
    }
}