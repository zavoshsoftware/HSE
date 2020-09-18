 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class UserJobRate : BaseEntity
    {
        public UserJobRate()
        {
            CompanyHumanResources = new List<CompanyHumanResource>();
        }

        [Display(Name="رده شغلی")]
        public string Title { get; set; }

        public virtual ICollection<CompanyHumanResource> CompanyHumanResources { get; set; }
    }
}