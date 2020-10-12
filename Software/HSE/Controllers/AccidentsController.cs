using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using ViewModels;

namespace HSE.Controllers
{
    [Authorize(Roles = "Administrator,company,supervisor")]
    public class AccidentsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Accidents
        public ActionResult Index()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string id = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
            string roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            Guid userId = new Guid(id);

            ViewBag.roleName = roleName;

            List<Accident> accidents = new List<Accident>();

            if (roleName == "Administrator")
            {
                accidents = db.Accidents.Where(a => a.IsDeleted == false)
                    .OrderByDescending(a => a.CreationDate).ToList();

                return View(accidents);

            }

            else if (roleName == "company")
            {
                accidents = db.Accidents.Where(a => a.IsDeleted == false && a.UserId == userId)
                    .OrderByDescending(a => a.CreationDate).ToList();

                return View(accidents);

            }
            else if (roleName == "supervisor")
            {

                accidents = db.Accidents.Where(a => a.IsDeleted == false)
                    .OrderByDescending(a => a.CreationDate).ToList();

                return View(accidents);

            }
            return View(accidents);


        }

        // GET: Accidents/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accident accident = db.Accidents.Find(id);

            if (accident == null)
            {
                return HttpNotFound();
            }

            string marriageStatus = "0";

            if (accident.IsMaried)
                marriageStatus = "1";

            ViewBag.AccidentEmployeeTypeId = new SelectList(db.AccidentEmployeeTypes, "Id", "Title", accident.AccidentEmployeeTypeId);
            ViewBag.MarriageStatusId = new SelectList(GetMarriageStatuses(), "Id", "Title", marriageStatus);

            AccidentViewModel accidentViewModel = new AccidentViewModel()
            {
                Id = accident.Id,
                AccidentInjuries = GetInjuries(accident.Id),
                AccidentReasonActions = GetReasonAction(accident.Id),
                AccidentReasonConditions = GetReasonCondition(accident.Id),
                AccidentParts = GetParts(accident.Id),
                AccidentTypes = GetTypes(accident.Id),
                AccidentResults = GetResults(accident.Id),
                AccidentDate = accident.AccidentDate,
                AccidentReports = db.AccidentReports.Where(c => c.IsDeleted == false && c.IsActive).ToList(),
                AccidentAmount = accident.AccidentAmount,
                AccidentComplication = accident.AccidentComplication,
                AccidentTypeOther = accident.AccidentTypeOther,
                AccidentDesc = accident.AccidentDesc,
                AccidentInitialComplication = accident.AccidentInitialComplication,
                AccidentInjuryOther = accident.AccidentInjuryOther,
                AccidentPartOther = accident.AccidentPartOther,
                AccidentReasonActionOther = accident.AccidentReasonActionOther,
                AccidentReasonConditionOther = accident.AccidentReasonConditionOther,
                AccidentTime = accident.AccidentTime,
                Actions = accident.Actions,
                Address = accident.Address,
                Age = accident.Age,
                CellNumber = accident.CellNumber,
                Company = accident.Company,
                Education = accident.Education,
                IsAcceptable = accident.IsAcceptable,
                Experience = accident.Experience,
                FirstName = accident.FirstName,
                HospitalName = accident.HospitalName,
                HospitalTime = accident.HospitalTime,
                Job = accident.Job,
                LastName = accident.LastName,
                ManageName = accident.ManageName,
                NationalCode = accident.NationalCode,
                PersonalNumber = accident.PersonalNumber,
                Phone = accident.Phone,
                Place = accident.Place,
                ReapeatAction = accident.ReapeatAction,
                Unit = accident.Unit,
                UserFullName = accident.UserFullName,
                WeekDay = accident.WeekDay

            };



            return View(accidentViewModel);
        }


        #region InsertMethods
        public List<MarriageStatus> GetMarriageStatuses()
        {
            List<MarriageStatus> marriage = new List<MarriageStatus>();

            marriage.Add(new MarriageStatus()
            {
                Id = 1,
                Title = "مجرد"
            });

            marriage.Add(new MarriageStatus()
            {
                Id = 2,
                Title = "متاهل"
            });

            return marriage;
        }
        public List<AccidentCheckbox> GetInjuries(Guid? accidentId)
        {
            List<AccidentCheckbox> accidentCheckboxitems = new List<AccidentCheckbox>();

            List<AccidentInjury> accidentInjuries =
                db.AccidentInjuries.Where(c => c.IsActive && c.IsDeleted == false).ToList();

            foreach (AccidentInjury accidentInjury in accidentInjuries)
            {
                bool isSelected = false;
                if (accidentId != null)
                {
                    if (db.AccidentInjuryRelAccidents.Any(c =>
                        c.AccidentId == accidentId && c.AccidentInjuryId == accidentInjury.Id))
                        isSelected = true;
                }
                accidentCheckboxitems.Add(new AccidentCheckbox()
                {
                    Id = accidentInjury.Id,
                    Title = accidentInjury.Title,
                    IsSelected = isSelected
                });
            }

            return accidentCheckboxitems;
        }
        public List<AccidentCheckboxSimple> GetComplication(Guid? accidentId)
        {
            List<AccidentCheckboxSimple> accidentCheckboxitems = new List<AccidentCheckboxSimple>();

            if (accidentId != null)
            {
                Accident accident = db.Accidents.Find(accidentId);

                if (accident != null)
                {
                    if (accident.AccidentComplication != null)
                    {
                        string[] accidentComplicationItems = accident.AccidentComplication.Split('|');

                        if (accidentComplicationItems.Any(c => c == "1"))
                        {
                            accidentCheckboxitems.Add(new AccidentCheckboxSimple()
                            {
                                Id = 1,
                                Title = "ناشي از كار آماري ",
                                IsSelected = true
                            });
                        }
                        else
                        {
                            accidentCheckboxitems.Add(new AccidentCheckboxSimple()
                            {
                                Id = 1,
                                Title = "ناشي از كار آماري ",
                                IsSelected = false
                            });
                        }

                        if (accidentComplicationItems.Any(c => c == "2"))
                        {
                            accidentCheckboxitems.Add(new AccidentCheckboxSimple()
                            {
                                Id = 2,
                                Title = "ناشي از كار غیرآماري ",
                                IsSelected = true
                            });
                        }
                        else
                        {
                            accidentCheckboxitems.Add(new AccidentCheckboxSimple()
                            {
                                Id = 2,
                                Title = "ناشي از كار غیرآماري ",
                                IsSelected = false
                            });
                        }

                        if (accidentComplicationItems.Any(c => c == "3"))
                        {
                            accidentCheckboxitems.Add(new AccidentCheckboxSimple()
                            {
                                Id = 3,
                                Title = "ناشي از كار ",
                                IsSelected = true
                            });
                        }
                        else
                        {
                            accidentCheckboxitems.Add(new AccidentCheckboxSimple()
                            {
                                Id = 3,
                                Title = "ناشي از كار ",
                                IsSelected = false
                            });
                        }
                    }
                }
            }
            else
            {
                accidentCheckboxitems.Add(new AccidentCheckboxSimple()
                {
                    Id = 1,
                    Title = "ناشي از كار آماري ",
                    IsSelected = false
                });
                accidentCheckboxitems.Add(new AccidentCheckboxSimple()
                {
                    Id = 2,
                    Title = "ناشي از كار غیرآماري ",
                    IsSelected = false
                });
                accidentCheckboxitems.Add(new AccidentCheckboxSimple()
                {
                    Id = 3,
                    Title = "ناشي از كار ",
                    IsSelected = false
                });
            }



            return accidentCheckboxitems;
        }

        public List<AccidentCheckboxSimple> GetInitialComplication(Guid? accidentId)
        {
            List<AccidentCheckboxSimple> accidentCheckboxitems = new List<AccidentCheckboxSimple>();

            if (accidentId != null)
            {
                Accident accident = db.Accidents.Find(accidentId);

                if (accident != null)
                {
                    if (accident.AccidentInitialComplication != null)
                    {
                        string[] accidentComplicationItems = accident.AccidentInitialComplication.Split('|');

                        if (accidentComplicationItems.Any(c => c == "1"))
                        {
                            accidentCheckboxitems.Add(new AccidentCheckboxSimple()
                            {
                                Id = 1,
                                Title = "مصدوم",
                                IsSelected = true
                            });
                        }
                        else
                        {
                            accidentCheckboxitems.Add(new AccidentCheckboxSimple()
                            {
                                Id = 1,
                                Title = "مصدوم",
                                IsSelected = false
                            });
                        }

                        if (accidentComplicationItems.Any(c => c == "2"))
                        {
                            accidentCheckboxitems.Add(new AccidentCheckboxSimple()
                            {
                                Id = 2,
                                Title = "مجروح",
                                IsSelected = true
                            });
                        }
                        else
                        {
                            accidentCheckboxitems.Add(new AccidentCheckboxSimple()
                            {
                                Id = 2,
                                Title = "مجروح",
                                IsSelected = false
                            });
                        }

                        if (accidentComplicationItems.Any(c => c == "3"))
                        {
                            accidentCheckboxitems.Add(new AccidentCheckboxSimple()
                            {
                                Id = 3,
                                Title = "قطع عضو",
                                IsSelected = true
                            });
                        }
                        else
                        {
                            accidentCheckboxitems.Add(new AccidentCheckboxSimple()
                            {
                                Id = 3,
                                Title = "قطع عضو",
                                IsSelected = false
                            });
                        }
                        if (accidentComplicationItems.Any(c => c == "4"))
                        {
                            accidentCheckboxitems.Add(new AccidentCheckboxSimple()
                            {
                                Id = 4,
                                Title = "فوت ",
                                IsSelected = true
                            });
                        }
                        else
                        {
                            accidentCheckboxitems.Add(new AccidentCheckboxSimple()
                            {
                                Id = 4,
                                Title = "فوت ",
                                IsSelected = false
                            });
                        }
                    }
                }
            }
            else
            {
                accidentCheckboxitems.Add(new AccidentCheckboxSimple()
                {
                    Id = 1,
                    Title = "مصدوم",
                    IsSelected = false
                });
                accidentCheckboxitems.Add(new AccidentCheckboxSimple()
                {
                    Id = 2,
                    Title = "مجروح",
                    IsSelected = false
                });
                accidentCheckboxitems.Add(new AccidentCheckboxSimple()
                {
                    Id = 3,
                    Title = "قطع عضو",
                    IsSelected = false
                });
                accidentCheckboxitems.Add(new AccidentCheckboxSimple()
                {
                    Id = 4,
                    Title = "فوت ",
                    IsSelected = false
                });
            }



            return accidentCheckboxitems;
        }

        public List<AccidentCheckbox> GetReasonAction(Guid? accidentId)
        {
            List<AccidentCheckbox> accidentCheckboxitems = new List<AccidentCheckbox>();

            List<AccidentReasonAction> accidentReasonActions =
                db.AccidentReasonActions.Where(c => c.IsActive && c.IsDeleted == false).ToList();

            foreach (AccidentReasonAction accidentReasonAction in accidentReasonActions)
            {
                bool isSelected = false;
                if (accidentId != null)
                {
                    if (db.AccidentReasonActionRelAccidents.Any(c =>
                        c.AccidentId == accidentId && c.AccidentReasonActionId == accidentReasonAction.Id))
                        isSelected = true;
                }

                accidentCheckboxitems.Add(new AccidentCheckbox()
                {
                    Id = accidentReasonAction.Id,
                    Title = accidentReasonAction.Title,
                    IsSelected = isSelected
                });
            }

            return accidentCheckboxitems;
        }

        public List<AccidentCheckbox> GetReasonCondition(Guid? accidentId)
        {
            List<AccidentCheckbox> accidentCheckboxitems = new List<AccidentCheckbox>();

            List<AccidentReasonCondition> accidentReasonConditions =
                db.AccidentReasonConditions.Where(c => c.IsActive && c.IsDeleted == false).ToList();

            foreach (AccidentReasonCondition accidentReasonCondition in accidentReasonConditions)
            {
                bool isSelected = false;

                if (accidentId != null)
                {
                    if (db.AccidentReasonConditionRelAccidents.Any(c =>
                        c.AccidentId == accidentId && c.AccidentReasonConditionId == accidentReasonCondition.Id))
                        isSelected = true;
                }

                accidentCheckboxitems.Add(new AccidentCheckbox()
                {
                    Id = accidentReasonCondition.Id,
                    Title = accidentReasonCondition.Title,
                    IsSelected = isSelected
                });
            }

            return accidentCheckboxitems;
        }

        public List<AccidentCheckbox> GetParts(Guid? accidentId)
        {
            List<AccidentCheckbox> accidentCheckboxitems = new List<AccidentCheckbox>();

            List<AccidentPart> accidentParts =
                db.AccidentParts.Where(c => c.IsActive && c.IsDeleted == false).ToList();

            foreach (AccidentPart accidentPart in accidentParts)
            {
                bool isSelected = false;

                if (accidentId != null)
                {
                    if (db.AccidentPartRelAccidents.Any(c =>
                        c.AccidentId == accidentId && c.AccidentPartId == accidentPart.Id))
                        isSelected = true;
                }

                accidentCheckboxitems.Add(new AccidentCheckbox()
                {
                    Id = accidentPart.Id,
                    Title = accidentPart.Title,
                    IsSelected = isSelected
                });
            }

            return accidentCheckboxitems;
        }

        public List<AccidentCheckbox> GetTypes(Guid? accidentId)
        {
            List<AccidentCheckbox> accidentCheckboxitems = new List<AccidentCheckbox>();

            List<AccidentType> accidentTypes =
                db.AccidentTypes.Where(c => c.IsActive && c.IsDeleted == false).ToList();

            foreach (AccidentType accidentType in accidentTypes)
            {
                bool isSelected = false;

                if (accidentId != null)
                {
                    if (db.AccidentTypeRelAccidents.Any(c =>
                        c.AccidentId == accidentId && c.AccidentTypeId == accidentType.Id))
                        isSelected = true;
                }

                accidentCheckboxitems.Add(new AccidentCheckbox()
                {
                    Id = accidentType.Id,
                    Title = accidentType.Title,
                    IsSelected = isSelected
                });
            }

            return accidentCheckboxitems;
        }

        public List<AccidentCheckbox> GetResults(Guid? accidentId)
        {
            List<AccidentCheckbox> accidentCheckboxitems = new List<AccidentCheckbox>();

            List<AccidentResult> accidentResults =
                db.AccidentResults.Where(c => c.IsActive && c.IsDeleted == false).ToList();

            foreach (AccidentResult accidentResult in accidentResults)
            {
                bool isSelected = false;

                if (accidentId != null)
                {
                    if (db.AccidentResultRelAccidents.Any(c =>
                        c.AccidentId == accidentId && c.AccidentResultId == accidentResult.Id))
                        isSelected = true;
                }

                accidentCheckboxitems.Add(new AccidentCheckbox()
                {
                    Id = accidentResult.Id,
                    Title = accidentResult.Title,
                    IsSelected = isSelected
                });
            }

            return accidentCheckboxitems;
        }



        public void InsertAccidentInjuryRelAccident(List<AccidentCheckbox> accidentInjuries, Guid accidentId)
        {
            try
            {
                foreach (AccidentCheckbox accidentInjury in accidentInjuries.Where(x => x.IsSelected))
                {
                    AccidentInjuryRelAccident injury = new AccidentInjuryRelAccident()
                    {
                        AccidentId = accidentId,
                        AccidentInjuryId = accidentInjury.Id,
                        IsDeleted = false,
                        IsActive = true,
                        CreationDate = DateTime.Now
                    };
                    db.AccidentInjuryRelAccidents.Add(injury);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        public void InsertAccidentReasonActionRelAccident(List<AccidentCheckbox> accidentReasonActions, Guid accidentId)
        {
            try
            {
                foreach (AccidentCheckbox accidentReasonAction in accidentReasonActions.Where(x => x.IsSelected))
                {
                    AccidentReasonActionRelAccident accidentReasonActionRelAccident = new AccidentReasonActionRelAccident()
                    {
                        AccidentId = accidentId,
                        AccidentReasonActionId = accidentReasonAction.Id,
                        IsDeleted = false,
                        IsActive = true,
                        CreationDate = DateTime.Now
                    };
                    db.AccidentReasonActionRelAccidents.Add(accidentReasonActionRelAccident);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        public void InsertAccidentReasonConditionRelAccident(List<AccidentCheckbox> accidentReasonConditions, Guid accidentId)
        {
            try
            {
                foreach (AccidentCheckbox accidentReasonConditionAction in accidentReasonConditions.Where(x => x.IsSelected))
                {
                    AccidentReasonConditionRelAccident accidentReasonConditionRelAccident = new AccidentReasonConditionRelAccident()
                    {
                        AccidentId = accidentId,
                        AccidentReasonConditionId = accidentReasonConditionAction.Id,
                        IsDeleted = false,
                        IsActive = true,
                        CreationDate = DateTime.Now
                    };
                    db.AccidentReasonConditionRelAccidents.Add(accidentReasonConditionRelAccident);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        public void InsertAccidentPartRelAccident(List<AccidentCheckbox> accidentParts, Guid accidentId)
        {
            try
            {
                foreach (AccidentCheckbox accidentPart in accidentParts.Where(x => x.IsSelected))
                {
                    AccidentPartRelAccident accidentPartRelAccident = new AccidentPartRelAccident()
                    {
                        AccidentId = accidentId,
                        AccidentPartId = accidentPart.Id,
                        IsDeleted = false,
                        IsActive = true,
                        CreationDate = DateTime.Now
                    };
                    db.AccidentPartRelAccidents.Add(accidentPartRelAccident);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        public void InsertAccidentTypeRelAccident(List<AccidentCheckbox> accidentTypes, Guid accidentId)
        {
            try
            {
                foreach (AccidentCheckbox accidentType in accidentTypes.Where(x => x.IsSelected))
                {
                    AccidentTypeRelAccident accidentTypeRelAccident = new AccidentTypeRelAccident()
                    {
                        AccidentId = accidentId,
                        AccidentTypeId = accidentType.Id,
                        IsDeleted = false,
                        IsActive = true,
                        CreationDate = DateTime.Now
                    };
                    db.AccidentTypeRelAccidents.Add(accidentTypeRelAccident);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        public void InsertAccidentResultRelAccident(List<AccidentCheckbox> accidentResults, Guid accidentId)
        {
            try
            {
                foreach (AccidentCheckbox accidentResult in accidentResults.Where(x => x.IsSelected))
                {
                    AccidentResultRelAccident accidentResultRelAccident = new AccidentResultRelAccident()
                    {
                        AccidentId = accidentId,
                        AccidentResultId = accidentResult.Id,
                        IsDeleted = false,
                        IsActive = true,
                        CreationDate = DateTime.Now
                    };
                    db.AccidentResultRelAccidents.Add(accidentResultRelAccident);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        public string InsertComplication(List<AccidentCheckboxSimple> complications)
        {
            try
            {
                string comp = "";
                foreach (AccidentCheckboxSimple complication in complications)
                {
                    if (complication.IsSelected)
                        comp = comp + complication.Id + "|";
                }

                return comp;
            }
            catch (Exception e)
            {
                string comp = "";
                return comp;
            }

        }

        public bool ReturnIsMariage(string mariageId)
        {
            if (mariageId == "0")
                return false;
            return true;
        }

        public string UploadFile(HttpPostedFileBase fileupload)
        {
            if (fileupload != null)
            {
                string filename = Path.GetFileName(fileupload.FileName);
                string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                     + Path.GetExtension(filename);

                string newFilenameUrl = "/Uploads/reports/" + newFilename;
                string physicalFilename = Server.MapPath(newFilenameUrl);

                fileupload.SaveAs(physicalFilename);

                return newFilenameUrl;
            }

            return string.Empty;
        }

        public void InsertAccidentReportRelAccident(Guid reportId, Guid accidentId, string fileUrl)
        {
            AccidentReportRelAccident accidentReport = new AccidentReportRelAccident()
            {
                AccidentId = accidentId,
                AccidentReportId = reportId,
                FileUrl = fileUrl,
                IsDeleted = false,
                IsActive = true,
                Id = Guid.NewGuid(),
                CreationDate = DateTime.Now
            };
        }


        #endregion

        public ActionResult Create()
        {
            ViewBag.AccidentEmployeeTypeId = new SelectList(db.AccidentEmployeeTypes, "Id", "Title");
            ViewBag.MarriageStatusId = new SelectList(GetMarriageStatuses(), "Id", "Title");



            AccidentViewModel accidentViewModel = new AccidentViewModel()
            {

                AccidentInjuries = GetInjuries(null),
                AccidentReasonActions = GetReasonAction(null),
                AccidentReasonConditions = GetReasonCondition(null),
                AccidentParts = GetParts(null),
                AccidentTypes = GetTypes(null),
                AccidentResults = GetResults(null),
                AccidentDate = DateTime.Today,
                AccidentReports = db.AccidentReports.Where(c => c.IsDeleted == false && c.IsActive).ToList(),
                Complication = GetComplication(null),
                InitialComplication = GetInitialComplication(null)
            };
            return View(accidentViewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccidentViewModel accidentViewModel, HttpPostedFileBase fileUpload1, HttpPostedFileBase fileUpload2, HttpPostedFileBase fileUpload3, HttpPostedFileBase fileUpload4, HttpPostedFileBase fileUpload5, HttpPostedFileBase fileUpload6, HttpPostedFileBase fileUpload7, HttpPostedFileBase fileUpload8, HttpPostedFileBase fileUpload9, HttpPostedFileBase fileUpload10)
        {
            if (ModelState.IsValid)
            {
                if (!(accidentViewModel.AccidentInjuries.Any(c => c.IsSelected) && accidentViewModel.AccidentReasonActions.Any(c => c.IsSelected) &&
                     accidentViewModel.AccidentReasonConditions.Any(c => c.IsSelected) &&  accidentViewModel.AccidentParts.Any(c => c.IsSelected) &&
                     accidentViewModel.AccidentResults.Any(c => c.IsSelected) &&  accidentViewModel.AccidentTypes.Any(c => c.IsSelected)))
                {
                    ModelState.AddModelError("checkboxes", "کلیه فیلد های ستاره دار را تکمیل نمایید");
                }

                else
                {


                    var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                    string uId = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
                    Guid userId = new Guid(uId);


                    Accident accident = new Accident()
                    {
                        UserId = userId,
                        FirstName = accidentViewModel.FirstName,
                        LastName = accidentViewModel.LastName,
                        PersonalNumber = accidentViewModel.PersonalNumber,
                        Unit = accidentViewModel.Unit,
                        Education = accidentViewModel.Education,
                        Age = accidentViewModel.Age,
                        Experience = accidentViewModel.Experience,
                        AccidentDate = accidentViewModel.AccidentDate,
                        AccidentTime = accidentViewModel.AccidentTime,
                        IsMaried = ReturnIsMariage(accidentViewModel.MarriageStatusId),
                        WeekDay = accidentViewModel.WeekDay,
                        Place = accidentViewModel.Place,
                        AccidentEmployeeTypeId = accidentViewModel.AccidentEmployeeTypeId,
                        Phone = accidentViewModel.Phone,
                        Company = accidentViewModel.Company,
                        Job = accidentViewModel.Job,
                        ManageName = accidentViewModel.ManageName,
                        HospitalTime = accidentViewModel.HospitalTime,
                        HospitalName = accidentViewModel.HospitalName,
                        NationalCode = accidentViewModel.NationalCode,
                        CellNumber = accidentViewModel.CellNumber,
                        Address = accidentViewModel.Address,
                        AccidentDesc = accidentViewModel.AccidentDesc,
                        UserFullName = accidentViewModel.UserFullName,
                        IsAcceptable = accidentViewModel.IsAcceptable,
                        AccidentAmount = accidentViewModel.AccidentAmount,
                        AccidentComplication = InsertComplication(accidentViewModel.Complication),
                        AccidentInitialComplication = InsertComplication(accidentViewModel.InitialComplication),
                        Actions = accidentViewModel.Actions,
                        ReapeatAction = accidentViewModel.ReapeatAction,

                        AccidentTypeOther = accidentViewModel.AccidentTypeOther,
                        AccidentReasonActionOther = accidentViewModel.AccidentReasonActionOther,
                        AccidentPartOther = accidentViewModel.AccidentPartOther,
                        AccidentInjuryOther = accidentViewModel.AccidentInjuryOther,
                        AccidentReasonConditionOther = accidentViewModel.AccidentReasonConditionOther, 
                    };

                    accident.IsDeleted = false;
                    accident.CreationDate = DateTime.Now;
                    db.Accidents.Add(accident);
                    db.SaveChanges();

                    InsertAccidentInjuryRelAccident(accidentViewModel.AccidentInjuries, accident.Id);
                    InsertAccidentReasonActionRelAccident(accidentViewModel.AccidentReasonActions, accident.Id);
                    InsertAccidentReasonConditionRelAccident(accidentViewModel.AccidentReasonConditions, accident.Id);
                    InsertAccidentPartRelAccident(accidentViewModel.AccidentParts, accident.Id);
                    InsertAccidentTypeRelAccident(accidentViewModel.AccidentTypes, accident.Id);
                    InsertAccidentResultRelAccident(accidentViewModel.AccidentResults, accident.Id);


                    InsertAccidentReportRelAccident(new Guid("B2D00079-A144-4991-A439-19A479096E2F"), accident.Id, UploadFile(fileUpload1));
                    InsertAccidentReportRelAccident(new Guid("0630884E-769D-41D8-84DC-49F9393B23EF"), accident.Id, UploadFile(fileUpload2));
                    InsertAccidentReportRelAccident(new Guid("4864B35E-2A47-4470-8088-4AFF7DD5C1FF"), accident.Id, UploadFile(fileUpload3));
                    InsertAccidentReportRelAccident(new Guid("CD529926-F1BC-4A6A-8C01-59F076CD45FD"), accident.Id, UploadFile(fileUpload4));
                    InsertAccidentReportRelAccident(new Guid("06FBF6A7-B20D-4CEE-A69F-63AA38618C06"), accident.Id, UploadFile(fileUpload5));
                    InsertAccidentReportRelAccident(new Guid("305538E2-AE72-4579-8FAD-6AEC9D835813"), accident.Id, UploadFile(fileUpload6));
                    InsertAccidentReportRelAccident(new Guid("053E7D95-E97C-4447-AE8E-A4D1F5B00B52"), accident.Id, UploadFile(fileUpload7));
                    InsertAccidentReportRelAccident(new Guid("9403E98D-ABD2-4843-8C23-DE9E81B2C6D6"), accident.Id, UploadFile(fileUpload8));
                    InsertAccidentReportRelAccident(new Guid("E68DFA7A-7946-437F-A2F6-E6A881A0CCA1"), accident.Id, UploadFile(fileUpload9));
                    InsertAccidentReportRelAccident(new Guid("14E8912A-08B4-4CD6-8F27-F7DFDD86465E"), accident.Id, UploadFile(fileUpload10));


                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }



            ViewBag.AccidentEmployeeTypeId = new SelectList(db.AccidentEmployeeTypes, "Id", "Title");
            ViewBag.MarriageStatusId = new SelectList(GetMarriageStatuses(), "Id", "Title");



            accidentViewModel = new AccidentViewModel()
            {
                AccidentInjuries = GetInjuries(null),
                AccidentReasonActions = GetReasonAction(null),
                AccidentReasonConditions = GetReasonCondition(null),
                AccidentParts = GetParts(null),
                AccidentTypes = GetTypes(null),
                AccidentResults = GetResults(null),
                AccidentDate = DateTime.Today,
                AccidentReports = db.AccidentReports.Where(c => c.IsDeleted == false && c.IsActive).ToList(),
                Complication = GetComplication(null),
                InitialComplication = GetInitialComplication(null)
            };
            return View(accidentViewModel);
        }



        // GET: Accidents/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accident accident = db.Accidents.Find(id);

            if (accident == null)
            {
                return HttpNotFound();
            }

            string marriageStatus = "0";

            if (accident.IsMaried)
                marriageStatus = "1";

            ViewBag.AccidentEmployeeTypeId = new SelectList(db.AccidentEmployeeTypes, "Id", "Title", accident.AccidentEmployeeTypeId);
            ViewBag.MarriageStatusId = new SelectList(GetMarriageStatuses(), "Id", "Title", marriageStatus);



            AccidentViewModel accidentViewModel = new AccidentViewModel()
            {
                Complication = GetComplication(accident.Id),
                InitialComplication = GetInitialComplication(accident.Id),
                Id = accident.Id,
                AccidentInjuries = GetInjuries(accident.Id),
                AccidentReasonActions = GetReasonAction(accident.Id),
                AccidentReasonConditions = GetReasonCondition(accident.Id),
                AccidentParts = GetParts(accident.Id),
                AccidentTypes = GetTypes(accident.Id),
                AccidentResults = GetResults(accident.Id),
                AccidentDate = accident.AccidentDate,
                AccidentReports = db.AccidentReports.Where(c => c.IsDeleted == false && c.IsActive).ToList(),
                AccidentAmount = accident.AccidentAmount,
                AccidentComplication = accident.AccidentComplication,
                AccidentTypeOther = accident.AccidentTypeOther,
                AccidentDesc = accident.AccidentDesc,
                AccidentInitialComplication = accident.AccidentInitialComplication,
                AccidentInjuryOther = accident.AccidentInjuryOther,
                AccidentPartOther = accident.AccidentPartOther,
                AccidentReasonActionOther = accident.AccidentReasonActionOther,
                AccidentReasonConditionOther = accident.AccidentReasonConditionOther,
                AccidentTime = accident.AccidentTime,
                Actions = accident.Actions,
                Address = accident.Address,
                Age = accident.Age,
                CellNumber = accident.CellNumber,
                Company = accident.Company,
                Education = accident.Education,
                IsAcceptable = accident.IsAcceptable,
                Experience = accident.Experience,
                FirstName = accident.FirstName,
                HospitalName = accident.HospitalName,
                HospitalTime = accident.HospitalTime,
                Job = accident.Job,
                LastName = accident.LastName,
                ManageName = accident.ManageName,
                NationalCode = accident.NationalCode,
                PersonalNumber = accident.PersonalNumber,
                Phone = accident.Phone,
                Place = accident.Place,
                ReapeatAction = accident.ReapeatAction,
                Unit = accident.Unit,
                UserFullName = accident.UserFullName,
                WeekDay = accident.WeekDay

            };



            return View(accidentViewModel);
        }

        // POST: Accidents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccidentViewModel accidentViewModel, HttpPostedFileBase fileUpload1, HttpPostedFileBase fileUpload2, HttpPostedFileBase fileUpload3, HttpPostedFileBase fileUpload4, HttpPostedFileBase fileUpload5, HttpPostedFileBase fileUpload6, HttpPostedFileBase fileUpload7, HttpPostedFileBase fileUpload8, HttpPostedFileBase fileUpload9, HttpPostedFileBase fileUpload10)
        {
            if (ModelState.IsValid)
            {
                Accident accident = new Accident()
                {
                    FirstName = accidentViewModel.FirstName,
                    LastName = accidentViewModel.LastName,
                    PersonalNumber = accidentViewModel.PersonalNumber,
                    Unit = accidentViewModel.Unit,
                    Education = accidentViewModel.Education,
                    Age = accidentViewModel.Age,
                    Experience = accidentViewModel.Experience,
                    AccidentDate = accidentViewModel.AccidentDate,
                    AccidentTime = accidentViewModel.AccidentTime,
                    IsMaried = ReturnIsMariage(accidentViewModel.MarriageStatusId),
                    WeekDay = accidentViewModel.WeekDay,
                    Place = accidentViewModel.Place,
                    AccidentEmployeeTypeId = accidentViewModel.AccidentEmployeeTypeId,
                    Phone = accidentViewModel.Phone,
                    Company = accidentViewModel.Company,
                    Job = accidentViewModel.Job,
                    ManageName = accidentViewModel.ManageName,
                    HospitalTime = accidentViewModel.HospitalTime,
                    HospitalName = accidentViewModel.HospitalName,
                    NationalCode = accidentViewModel.NationalCode,
                    CellNumber = accidentViewModel.CellNumber,
                    Address = accidentViewModel.Address,
                    AccidentDesc = accidentViewModel.AccidentDesc,
                    UserFullName = accidentViewModel.UserFullName,
                    IsAcceptable = accidentViewModel.IsAcceptable,
                    AccidentAmount = accidentViewModel.AccidentAmount,
                    AccidentComplication = accidentViewModel.AccidentComplication,
                    AccidentInitialComplication = accidentViewModel.AccidentInitialComplication,
                    Actions = accidentViewModel.Actions,
                    ReapeatAction = accidentViewModel.ReapeatAction,

                    AccidentTypeOther = accidentViewModel.AccidentTypeOther,
                    AccidentReasonActionOther = accidentViewModel.AccidentReasonActionOther,
                    AccidentPartOther = accidentViewModel.AccidentPartOther,
                    AccidentInjuryOther = accidentViewModel.AccidentInjuryOther,
                    AccidentReasonConditionOther = accidentViewModel.AccidentReasonConditionOther


                };

                accident.IsDeleted = false;
                accident.CreationDate = DateTime.Now;
                accident.LastModifiedDate = DateTime.Now;


                db.SaveChanges();

                InsertAccidentInjuryRelAccident(accidentViewModel.AccidentInjuries, accident.Id);
                InsertAccidentReasonActionRelAccident(accidentViewModel.AccidentReasonActions, accident.Id);
                InsertAccidentReasonConditionRelAccident(accidentViewModel.AccidentReasonConditions, accident.Id);
                InsertAccidentPartRelAccident(accidentViewModel.AccidentParts, accident.Id);
                InsertAccidentTypeRelAccident(accidentViewModel.AccidentTypes, accident.Id);
                InsertAccidentResultRelAccident(accidentViewModel.AccidentResults, accident.Id);


                InsertAccidentReportRelAccident(new Guid("B2D00079-A144-4991-A439-19A479096E2F"), accident.Id, UploadFile(fileUpload1));
                InsertAccidentReportRelAccident(new Guid("0630884E-769D-41D8-84DC-49F9393B23EF"), accident.Id, UploadFile(fileUpload2));
                InsertAccidentReportRelAccident(new Guid("4864B35E-2A47-4470-8088-4AFF7DD5C1FF"), accident.Id, UploadFile(fileUpload3));
                InsertAccidentReportRelAccident(new Guid("CD529926-F1BC-4A6A-8C01-59F076CD45FD"), accident.Id, UploadFile(fileUpload4));
                InsertAccidentReportRelAccident(new Guid("06FBF6A7-B20D-4CEE-A69F-63AA38618C06"), accident.Id, UploadFile(fileUpload5));
                InsertAccidentReportRelAccident(new Guid("305538E2-AE72-4579-8FAD-6AEC9D835813"), accident.Id, UploadFile(fileUpload6));
                InsertAccidentReportRelAccident(new Guid("053E7D95-E97C-4447-AE8E-A4D1F5B00B52"), accident.Id, UploadFile(fileUpload7));
                InsertAccidentReportRelAccident(new Guid("9403E98D-ABD2-4843-8C23-DE9E81B2C6D6"), accident.Id, UploadFile(fileUpload8));
                InsertAccidentReportRelAccident(new Guid("E68DFA7A-7946-437F-A2F6-E6A881A0CCA1"), accident.Id, UploadFile(fileUpload9));
                InsertAccidentReportRelAccident(new Guid("14E8912A-08B4-4CD6-8F27-F7DFDD86465E"), accident.Id, UploadFile(fileUpload10));


                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.AccidentEmployeeTypeId = new SelectList(db.AccidentEmployeeTypes, "Id", "Title");
            ViewBag.MarriageStatusId = new SelectList(GetMarriageStatuses(), "Id", "Title");



            accidentViewModel = new AccidentViewModel()
            {
                AccidentInjuries = GetInjuries(accidentViewModel.Id),
                AccidentReasonActions = GetReasonAction(accidentViewModel.Id),
                AccidentReasonConditions = GetReasonCondition(accidentViewModel.Id),
                AccidentParts = GetParts(accidentViewModel.Id),
                AccidentTypes = GetTypes(accidentViewModel.Id),
                AccidentResults = GetResults(accidentViewModel.Id),
                AccidentDate = DateTime.Today,
                AccidentReports = db.AccidentReports.Where(c => c.IsDeleted == false && c.IsActive).ToList(),
                Complication = GetComplication(accidentViewModel.Id),
                InitialComplication = GetInitialComplication(accidentViewModel.Id),
            };
            return View(accidentViewModel);
        }

        // GET: Accidents/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accident accident = db.Accidents.Find(id);
            if (accident == null)
            {
                return HttpNotFound();
            }
            return View(accident);
        }

        // POST: Accidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Accident accident = db.Accidents.Find(id);
            accident.IsDeleted = true;
            accident.DeletionDate = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
