using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class Accident : BaseEntity
    {
        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Display(Name = "شماره پرسنلی")]
        public string PersonalNumber { get; set; }

        [Display(Name = "واحد")]
        public string Unit { get; set; }

        [Display(Name = "تحصیلات")]
        public string Education { get; set; }

        [Display(Name = "سن")]
        public int Age { get; set; }

        [Display(Name = "تجربه")]
        public string Experience { get; set; }

        [Display(Name = "تاریخ حادثه")]
        public DateTime AccidentDate { get; set; }

        [Display(Name = "ساعت وقوع حادثه")]
        public string AccidentTime { get; set; }

        [Display(Name = "وضعیت تاهل")]
        public bool IsMaried { get; set; }

        [Display(Name = "روز حادثه")]
        public string WeekDay { get; set; }

        [Display(Name = "محل حادثه")]
        public string Place { get; set; }

        [Display(Name = "وضعیت استخدامی")]
        public Guid AccidentEmployeeTypeId { get; set; }

        [Display(Name = "")]
        public AccidentEmployeeType AccidentEmployeeType { get; set; }

        [Display(Name = "تلفن")]
        public string Phone { get; set; }

        [Display(Name = "نام شرکت")]
        public string Company { get; set; }

        [Display(Name = "شغل")]
        public string Job { get; set; }

        [Display(Name = "نام سرپرست")]
        public string ManageName { get; set; }

        [Display(Name = "ساعت اعزام به درمانگاه")]
        public string HospitalTime { get; set; }

        [Display(Name = "نام درمانگاه یا بیمارستان محل بستری")]
        public string HospitalName { get; set; }

        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; }

        [Display(Name = "شماره موبایل حادثه دیده")]
        public string CellNumber { get; set; }

        [Display(Name = "نشانی کامل وقوع حادثه")]
        public string Address { get; set; }

        [Display(Name = "شرح مختصر حادثه")]
        public string AccidentDesc { get; set; }

        [Display(Name = "آیا گفته مصدوم مورد تایید شما است؟")]
        public bool IsAcceptable { get; set; }

        [Display(Name = "نام و نام خانوادگی تکمیل کننده")]
        public string UserFullName { get; set; }


        public virtual ICollection<AccidentReasonConditionRelAccident> AccidentReasonConditionRelAccidents { get; set; }
        [Display(Name = "سایر اعمال نا ایمن")]
        public string AccidentReasonConditionOther { get; set; }


        public virtual ICollection<AccidentReasonActionRelAccident> AccidentReasonActionRelAccidents { get; set; }

        [Display(Name = "سایر شرایط نا ایمن")]
        public string AccidentReasonActionOther { get; set; }


        public virtual ICollection<AccidentPartRelAccident> AccidentPartRelAccidents { get; set; }

        [Display(Name = "عضو حادثه دیده - سایر")]
        public string AccidentPartOther { get; set; }

        public virtual ICollection<AccidentInjuryRelAccident> AccidentInjuryRelAccidents { get; set; }
        [Display(Name = "نوع صدمه - سایر")]
        public string AccidentInjuryOther { get; set; }


        public virtual ICollection<AccidentTypeRelAccident> AccidentTypeRelAccidents { get; set; }

        [Display(Name = "نوع حادثه - سایر")]
        public string AccidentTypeOther { get; set; }


        public virtual ICollection<AccidentResultRelAccident> AccidentResultRelAccidents { get; set; }

        [Display(Name = "میزان تخمینی هزینه خسارات")]

        public decimal AccidentAmount { get; set; }

        [Display(Name = "عوارض ناشی از حادثه")]
        public string AccidentComplication { get; set; }
        [Display(Name = "عوارض اوليه حادثه ")]
        public string AccidentInitialComplication { get; set; }

        [Display(Name = "اقدامات انجام شده")]
        public string Actions { get; set; }
        [Display(Name = "اقدامات اصلاحي جهت جلوگيري از تكرار حادثه")]
        public string ReapeatAction { get; set; }
        [Display(Name = "اقدامات اصلاحي جهت جلوگيري از تكرار حادثه")]
        public string WasteDays { get; set; }
        [Display(Name = "كارشناس مسئول ايمني شركت")]
        public string CompanyUser { get; set; }

        public virtual ICollection<AccidentReportRelAccident> AccidentReportRelAccidents { get; set; }

        [Display(Name = "کاربر")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}