using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class RequirmentDetailItemViewModel
    {
        public RequirmentType RequirmentType { get; set; }
        public List<RequirmentItemViewModel> Requirments { get; set; }
    }

    public class RequirmentItemViewModel
    {
        public Guid Id { get; set; }
        [Display(Name="الزامات")]
        public string Title { get; set; }
        [Display(Name="وزن")]
        public string Weight { get; set; }
        [Display(Name="درصد پیشرفت تجمعی")]
        public string ProgressWeight { get; set; }
        [Display(Name="مبلغ پیشرفت تجمعی")]
        public string ProgressAmount { get; set; }
    }
}