using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class Document : BaseEntity
    {
        [Display(Name="عنوان سند")]
        public string Title { get; set; }
 
        [Display(Name = "دسته بندی")]
        public Guid DocumentTypeId { get; set; }
        public virtual DocumentType DocumentType { get; set; }

        [Display(Name = "سند")]
        public string FileUrl { get; set; }
        internal class configuration : EntityTypeConfiguration<Document>
        {
            public configuration()
            {
                HasRequired(p => p.DocumentType).WithMany(j => j.Documents).HasForeignKey(p => p.DocumentTypeId);
            }
        }
    }
}