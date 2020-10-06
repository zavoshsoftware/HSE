using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class Safety:BaseEntity
    {
        [Display(Name="عنوان")]
        public string Title { get; set; }


        [Display(Name="نوع ایمنی")]
        public Guid SafetyTypeId { get; set; }
        public virtual SafetyType SafetyType { get; set; }

        [Display(Name="نوع فایل")]
        public Guid? SafetyFileTypeId { get; set; }
        public virtual SafetyFileType SafetyFileType { get; set; }

        [Display(Name="سند")]
        public string FileUrl { get; set; }
        [Display(Name="تایید ناظر؟")]
        public bool? IsAccepteBySupervisor { get; set; }
        [Display(Name="نظر ناظر")]
        [DataType(DataType.MultilineText)]
        public string SupervisorComment { get; set; }
        [Display(Name="شرکت پیمانکار")]
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}