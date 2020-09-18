using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class UserStageListViewModel
    {
        [Display(Name="پروژه")]
        public string ProjectTitle { get; set; }
        [Display(Name="عملیات")]
        public string  OperationTitle { get; set; }
        [Display(Name="فعالیت")]
        public string  ActTitle { get; set; }
        [Display(Name="مرحله انجام کار")]
        public string  StageTitle { get; set; }
        public Guid  Id { get; set; }
        [Display(Name="وضعیت ثبت ریسک")]
        public string StatusTitle { get; set; }
    }
}