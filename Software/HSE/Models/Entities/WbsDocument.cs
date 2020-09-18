using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models 
{
    public class WbsDocument : BaseEntity
    {
        [Display(Name = "مدارک مورد نیاز")]
        public string Title { get; set; }

        public Guid WbsUserTypeId { get; set; }
        public virtual WbsUserType WbsUserType { get; set; }



    }
}