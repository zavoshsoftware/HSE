using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class ChangePasswordViewModel
    {
        [Display(Name="کلمه عبور قدیمی")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string OldPassword { get; set; }

        [Display(Name = "کلمه عبور جدید")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string NewPassword { get; set; }

        [Display(Name = "تکرار کلمه عبور جدید")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string RepeatNewPassword { get; set; }
    }
     
}