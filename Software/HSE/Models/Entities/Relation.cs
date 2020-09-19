using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Web.Mvc;

namespace Models
{
    public class Relation : BaseEntity
    {
        [Display(Name = "دسته بندی")]
        public Guid RelationTypeId { get; set; }
        public virtual RelationType RelationType { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "متن")]
        [DataType(DataType.Html)]
        [AllowHtml]
        [Column(TypeName = "ntext")]
        [UIHint("RichText")]
        public string Body { get; set; }
         

        internal class configuration : EntityTypeConfiguration<Relation>
        {
            public configuration()
            {
                HasRequired(p => p.RelationType).WithMany(j => j.Relations).HasForeignKey(p => p.RelationTypeId);
            }
        }
    }
}