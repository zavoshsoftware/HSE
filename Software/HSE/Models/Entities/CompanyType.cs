using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class CompanyType : BaseEntity
    {
        public CompanyType()
        {
            Companies = new List<Company>();
            PermitTypes = new List<PermitType>();
        }
        public string Title { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<PermitType> PermitTypes { get; set; }
    }
}