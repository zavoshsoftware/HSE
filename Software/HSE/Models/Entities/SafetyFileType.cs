
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class SafetyFileType : BaseEntity
    {

        public SafetyFileType()
        {
            Safeties = new List<Safety>();
        }
        public string Title { get; set; }

        public int Code { get; set; }
        public virtual ICollection<Safety> Safeties { get; set; }

    }
}