
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class DocumentType : BaseEntity
    {
        [Display(Name="نوع سند")]
        public string Title { get; set; }

        public virtual ICollection<Document> Documents { get; set; }

      
    }
}