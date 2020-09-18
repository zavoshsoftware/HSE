using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models 
{
    public class WbsUserType:BaseEntity
    {
        public WbsUserType()
        {
            WbsDocuments=new List<WbsDocument>();
        }

        [Display(Name = "ساختار شکست")]
        public string Title { get; set; }

        public Guid WbsRequirmentId { get; set; }
        public virtual WbsRequirment WbsRequirment { get; set; }

        public virtual ICollection<WbsDocument> WbsDocuments { get; set; }

    }
}