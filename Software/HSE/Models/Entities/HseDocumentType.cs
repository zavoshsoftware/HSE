using System;
using System.Collections.Generic;
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
        public string Title { get; set; }

        public virtual ICollection<HseDocument> HseDocuments { get; set; }
    }
}