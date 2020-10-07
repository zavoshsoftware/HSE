using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class HseDocumentType:BaseEntity
    {
        public HseDocumentType()
        {
            HseDocuments=new List<HseDocument>();
        }
        [Display(Name = "نوع سند")]
        public string Title { get; set; }

        public virtual ICollection<HseDocument> HseDocuments { get; set; }
    }
}