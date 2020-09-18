using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class AccidentViewModel
    {
        public Guid Id { get; set; }
        public List<AccidentCheckbox> AccidentInjuries { get; set; }
        public List<AccidentCheckbox> AccidentReasonActions { get; set; }
        public List<AccidentCheckbox> AccidentReasonConditions { get; set; }
        public List<AccidentCheckbox> AccidentParts { get; set; }
        public List<AccidentCheckbox> AccidentTypes { get; set; }
        public List<AccidentCheckbox> AccidentResults { get; set; }
        public List<AccidentCheckboxSimple> Complication { get; set; }
        public List<AccidentCheckboxSimple> InitialComplication { get; set; }

        [Display(Name = "نام*")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی*")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string LastName { get; set; }

        [Display(Name = "شماره پرسنلی")]
        public string PersonalNumber { get; set; }

        [Display(Name = "واحد*")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string Unit { get; set; }

        [Display(Name = "تحصیلات")]
        public string Education { get; set; }

        [Display(Name = "سن*")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public int Age { get; set; }

        [Display(Name = "تجربه*")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string Experience { get; set; }

        [Display(Name = "تاریخ حادثه*")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public DateTime AccidentDate { get; set; }

        [Display(Name = "ساعت وقوع حادثه*")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string AccidentTime { get; set; }

        [Display(Name = "وضعیت تاهل")]
        public string MarriageStatusId { get; set; }

        [Display(Name = "روز حادثه*")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string WeekDay { get; set; }

        [Display(Name = "محل حادثه*")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string Place { get; set; }

        [Display(Name = "وضعیت استخدامی")]
        public Guid AccidentEmployeeTypeId { get; set; }

    

        [Display(Name = "تلفن")]
        public string Phone { get; set; }

        [Display(Name = "نام شرکت*")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string Company { get; set; }

        [Display(Name = "شغل*")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string Job { get; set; }

        [Display(Name = "نام سرپرست*")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string ManageName { get; set; }

        [Display(Name = "ساعت اعزام به درمانگاه*")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string HospitalTime { get; set; }

        [Display(Name = "نام درمانگاه یا بیمارستان محل بستری*")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string HospitalName { get; set; }

        [Display(Name = "کد ملی*")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string NationalCode { get; set; }

        [Display(Name = "شماره موبایل حادثه دیده*")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string CellNumber { get; set; }

        [Display(Name = "نشانی کامل وقوع حادثه*")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "شرح مختصر حادثه*")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string AccidentDesc { get; set; }

        [Display(Name = "آیا گفته مصدوم مورد تایید شما است؟")]
        public bool IsAcceptable { get; set; }

        [Display(Name = "نام و نام خانوادگی تکمیل کننده*")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string UserFullName { get; set; }


        [Display(Name = "سایر اعمال نا ایمن")]
        public string AccidentReasonConditionOther { get; set; }



        [Display(Name = "سایر شرایط نا ایمن")]
        public string AccidentReasonActionOther { get; set; }



        [Display(Name = "عضو حادثه دیده - سایر")]
        public string AccidentPartOther { get; set; }

        [Display(Name = "نوع صدمه - سایر")]
        public string AccidentInjuryOther { get; set; }



        [Display(Name = "نوع حادثه - سایر")]
        public string AccidentTypeOther { get; set; }
         

        [Display(Name = "میزان تخمینی هزینه خسارات*")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public decimal AccidentAmount { get; set; }

        [Display(Name = "عوارض ناشی از حادثه*")]
        public string AccidentComplication { get; set; }

        [Display(Name = "عوارض اوليه حادثه *")]
        public string AccidentInitialComplication { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "اقدامات انجام شده")]
        public string Actions { get; set; }
        [Display(Name = "اقدامات اصلاحي جهت جلوگيري از تكرار حادثه")]
        [DataType(DataType.MultilineText)]
        public string ReapeatAction { get; set; }

        public List<AccidentReport> AccidentReports { get; set; }


        [Display(Name = "اقدامات اصلاحي جهت جلوگيري از تكرار حادثه*")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string WasteDays { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "كارشناس مسئول ايمني شركت*")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string CompanyUser { get; set; }
    }

    public class AccidentCheckbox
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsSelected { get; set; }
    }

    public class AccidentCheckboxSimple
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsSelected { get; set; }
    }

    public class MarriageStatus
    {
        public int Id { get; set; }
        public string Title{ get; set; }
    }
}