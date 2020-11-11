using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Web.Mvc;

namespace Models
{
    public class RelationImage : BaseEntity
    {
        [Display(Name = "دسته بندی")]
        public Guid RelationId { get; set; }
        public virtual Relation Relation { get; set; }

        public string ImageUrl { get; set; }
        internal class configuration : EntityTypeConfiguration<RelationImage>
        {
            public configuration()
            {
                HasRequired(p => p.Relation).WithMany(j => j.RelationImages).HasForeignKey(p => p.RelationId);
            }
        }
    }
}