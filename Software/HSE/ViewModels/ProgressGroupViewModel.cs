using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class ProgressGroupViewModel
    {
        public Guid Id { get; set; }
        [Display(Name="الزامات")]
        public string Title { get; set; }
        [Display(Name="درصد پیشرفت")]
        public string TotalPercent { get; set; }
    }
}