using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models 
{
    public class Project : BaseEntity
    {
        public Project()
        {
            Operations=new List<Operation>();
        }
        [Display(Name="عنوان پروژه")]
        public string Title { get; set; }

        [Display(Name="نام مخفف پروژه")]
        public string Name { get; set; }

        public virtual ICollection<Operation> Operations { get; set; }
    }
}