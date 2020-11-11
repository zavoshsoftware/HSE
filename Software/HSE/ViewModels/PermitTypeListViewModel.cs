using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class PermitTypeListViewModel
    {
        public PermitType PermitType { get; set; }
        [Display(Name="تعداد پرمیت ثبت شده")]
        public int Qty { get; set; }
    }
}