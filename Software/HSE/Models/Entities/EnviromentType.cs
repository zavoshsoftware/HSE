
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class EnviromentType : BaseEntity
    {
        [Display(Name="دسته بندی")]
        public string Title { get; set; }

        [Display(Name="نمونه فرم")]
        public string FileUrl { get; set; }

        public virtual ICollection<Enviroment> Enviroments { get; set; }

      
    }
}