using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class PassiveDefense : BaseEntity
    {
        [Display(Name="عنوان پدافند غیرعامل")]
        public string Title { get; set; }

        [Display(Name="نوع فایل")]
        public Guid PassiveDefenseTypeId { get; set; }
        public virtual PassiveDefenseType PassiveDefenseType { get; set; }

        [Display(Name="فایل")]
        public string FileUrl { get; set; }

        [Display(Name="تایید توسط ناظر")]
        public bool IsAcceptBySup { get; set; }

        [Display(Name="نظر ناظر")]
        [DataType(DataType.MultilineText)]
        public string SupComment { get; set; }

        [Display(Name="شرکت پیمانکار")]
        public Guid? CompanyId { get; set; }
        public Company Company { get; set; }
    }
}