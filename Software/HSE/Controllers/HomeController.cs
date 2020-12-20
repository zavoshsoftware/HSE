using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Newtonsoft.Json;
using ViewModels;

namespace HSE.Controllers
{
    public class HomeController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            return Redirect("/login");
        }

        [Authorize]
        public ActionResult Dashboard()
        {
            return View();
        }

        [Authorize]
        public ActionResult Dashboard3()
        {

            //ViewBag.DataPoints = JsonConvert.SerializeObject(GetAnomalyChart(), _jsonSetting);
            //  ViewBag.DataPointsAccident = JsonConvert.SerializeObject(GetAccidentChartByType(), _jsonSetting);
            //     ViewBag.DataPointsAccidentInjury = JsonConvert.SerializeObject(GetAccidentChartByInjuryType(), _jsonSetting);
            ViewBag.DataPointsAccidentComplication = JsonConvert.SerializeObject(GetAccidentChartByComplication(), _jsonSetting);

            return View();
        }

        [Authorize]
        public ActionResult AccidentDashboard(string[] companyId, string companyTypeId)
        {
            List<Company> companies=new List<Company>();

            Guid coTypeId = new Guid(companyTypeId);
            companies = db.Companies.Where(c => c.CompanyTypeId == coTypeId && c.IsDeleted == false)
                .OrderBy(c => c.Title).ToList();

            List<CompanyItemInDashboard> result = new List<CompanyItemInDashboard>();

            foreach (var company in companies)
            {
                result.Add(new CompanyItemInDashboard()
                {
                    Id = company.Id,
                    Title = company.Title,
                    IsSelected = false
                });

                if (companyId != null)
                {
                    if (companyId.Any(c => c == company.Id.ToString()))
                    {
                        result.LastOrDefault().IsSelected = true;
                    }
                }
                else
                {
                    result.LastOrDefault().IsSelected = true;
                }
            }

            List<CompanyType> companyTypes = db.CompanyTypes.Where(c => c.IsDeleted == false && c.IsActive).ToList();
            Guid selected = companyTypes.FirstOrDefault().Id;

            if (companyTypeId != null)
                selected = coTypeId;

            CompanyListDashboardViewModel res = new CompanyListDashboardViewModel()
            {
                Companies = result,
                CompanyTypes = new SelectList(companyTypes, "Id", "Title", selected),
                SelectedCompanyTypeId = selected
            };

            ViewBag.DataPointsAccident = JsonConvert.SerializeObject(GetAccidentChartByType(companyId), _jsonSetting);
            ViewBag.DataPointsAccidentInjury = JsonConvert.SerializeObject(GetAccidentChartByInjuryType(companyId), _jsonSetting);
            ViewBag.DataPointsAccidentReasonAction = JsonConvert.SerializeObject(GetAccidentChartByReasonAction(companyId), _jsonSetting);
            ViewBag.DataPointsAccidentReasonCondition = JsonConvert.SerializeObject(GetAccidentChartByReasonCondition(companyId), _jsonSetting);

            return View(res);
        }
        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };





        public List<ChartViewModel> GetAccidentChartByType(string[] companies)
        {
            if (companies == null)
            {
                int totalAccidents = db.AccidentTypeRelAccidents.Count(c => c.IsDeleted == false);

                List<AccidentType> accidentTypes =
                    db.AccidentTypes.Where(c => c.IsDeleted == false && c.IsActive).ToList();

                List<ChartViewModel> charts = new List<ChartViewModel>();

                foreach (AccidentType accidentType in accidentTypes)
                {

                    int itemCount =
                        db.AccidentTypeRelAccidents.Count(c =>
                            c.AccidentTypeId == accidentType.Id && c.IsDeleted == false);

                    decimal count = (decimal)((decimal)((decimal)itemCount / (decimal)totalAccidents) * 100);

                    if (count > 0)
                    {
                        charts.Add(new ChartViewModel()
                        {
                            Label = accidentType.Title,
                            Y = Math.Round(count, 2)
                        });
                    }
                }

                return charts;
            }
            else
            {
                int tot = 0;

                foreach (string companyId in companies)
                {
                    Guid id = new Guid(companyId);

                    int totalAccidents =
                        db.AccidentTypeRelAccidents.Count(c => c.Accident.User.CompanyId == id && c.IsDeleted == false);

                    tot += totalAccidents;
                }

                List<ChartViewModel> charts = new List<ChartViewModel>();
                List<AccidentType> accidentTypes =
                    db.AccidentTypes.Where(c => c.IsDeleted == false && c.IsActive).ToList();
                if (tot == 0)
                {
                    foreach (AccidentType accidentType in accidentTypes)
                    {
                        charts.Add(new ChartViewModel()
                        {
                            Label = accidentType.Title,
                            Y = 0
                        });
                    }

                    return charts;

                }



                foreach (AccidentType accidentType in accidentTypes)
                {
                    int typeTot = 0;
                    foreach (string companyId in companies)
                    {
                        Guid id = new Guid(companyId);

                        int itemCount =
                            db.AccidentTypeRelAccidents.Count(c =>
                                c.AccidentTypeId == accidentType.Id && c.Accident.User.CompanyId == id && c.IsDeleted == false);

                        typeTot += itemCount;
                    }


                    decimal count = (decimal)((decimal)((decimal)typeTot / (decimal)tot) * 100);

                    if (count > 0)
                    {
                        charts.Add(new ChartViewModel()
                        {
                            Label = accidentType.Title,
                            Y = Math.Round(count, 2)
                        });
                    }
                }

                return charts;
            }
        }

        public List<ChartViewModel> GetAccidentChartByInjuryType(string[] companies)
        {
            if (companies == null)
            {
                List<AccidentInjuryRelAccident> accidentInjuryRelAccidents =
                    db.AccidentInjuryRelAccidents.Where(c => c.IsDeleted == false).ToList();

                int totalAccidents = accidentInjuryRelAccidents.Count();

                List<ChartViewModel> charts = new List<ChartViewModel>();
                List<AccidentInjury> accidentInjuries = db.AccidentInjuries.Where(c => c.IsDeleted == false).ToList();

                foreach (AccidentInjury accidentInjury in accidentInjuries)
                {
                    int itemCount =
                        accidentInjuryRelAccidents.Count(c =>
                            c.AccidentInjuryId == accidentInjury.Id && c.IsDeleted == false);

                    decimal count = (decimal)((decimal)((decimal)itemCount / (decimal)totalAccidents) * 100);

                    if (count > 0)
                    {
                        charts.Add(new ChartViewModel()
                        {
                            Label = accidentInjury.Title,
                            Y = Math.Round(count, 2)
                        });
                    }
                }

                return charts;
            }
            else
            {
                int tot = 0;

                foreach (string companyId in companies)
                {
                    Guid id = new Guid(companyId);

                    int totalAccidents =
                        db.AccidentInjuryRelAccidents.Count(c => c.Accident.User.CompanyId == id && c.IsDeleted == false);

                    tot += totalAccidents;
                }

                List<ChartViewModel> charts = new List<ChartViewModel>();
                List<AccidentInjury> accidentInjuries =
                    db.AccidentInjuries.Where(c => c.IsDeleted == false && c.IsActive).ToList();

                if (tot == 0)
                {
                    foreach (AccidentInjury accidentInjury in accidentInjuries)
                    {
                        charts.Add(new ChartViewModel()
                        {
                            Label = accidentInjury.Title,
                            Y = 0
                        });
                    }

                    return charts;

                }



                foreach (AccidentInjury accidentInjury in accidentInjuries)
                {
                    int typeTot = 0;
                    foreach (string companyId in companies)
                    {
                        Guid id = new Guid(companyId);

                        int itemCount =
                            db.AccidentInjuryRelAccidents.Count(c =>
                                c.AccidentInjuryId == accidentInjury.Id && c.Accident.User.CompanyId == id && c.IsDeleted == false);

                        typeTot += itemCount;
                    }


                    decimal count = (decimal)((decimal)((decimal)typeTot / (decimal)tot) * 100);

                    if (count > 0)
                    {
                        charts.Add(new ChartViewModel()
                        {
                            Label = accidentInjury.Title,
                            Y = Math.Round(count, 2)
                        });
                    }
                }

                return charts;
            }
        }

        public List<ChartViewModel> GetAccidentChartByReasonAction(string[] companies)
        {
            if (companies == null)
            {
                List<AccidentReasonActionRelAccident> accidentReasonActionRelAccidents =
                    db.AccidentReasonActionRelAccidents.Where(c => c.IsDeleted == false).ToList();

                int totalAccidents = accidentReasonActionRelAccidents.Count();

                List<ChartViewModel> charts = new List<ChartViewModel>();
                List<AccidentReasonAction> accidentReasonActions = db.AccidentReasonActions.Where(c => c.IsDeleted == false).ToList();

                foreach (AccidentReasonAction accidentReasonAction in accidentReasonActions)
                {
                    int itemCount =
                        accidentReasonActionRelAccidents.Count(c =>
                            c.AccidentReasonActionId == accidentReasonAction.Id && c.IsDeleted == false);

                    decimal count = (decimal)((decimal)((decimal)itemCount / (decimal)totalAccidents) * 100);
                    if (count > 0)
                    {
                        charts.Add(new ChartViewModel()
                        {
                            Label = accidentReasonAction.Title,
                            Y = Math.Round(count, 2)
                        });
                    }
                }

                return charts;
            }
            else
            {
                int tot = 0;

                foreach (string companyId in companies)
                {
                    Guid id = new Guid(companyId);

                    int totalAccidents =
                        db.AccidentReasonActionRelAccidents.Count(c => c.Accident.User.CompanyId == id && c.IsDeleted == false);

                    tot += totalAccidents;
                }

                List<ChartViewModel> charts = new List<ChartViewModel>();
                List<AccidentReasonAction> accidentReasonActions =
                    db.AccidentReasonActions.Where(c => c.IsDeleted == false && c.IsActive).ToList();

                if (tot == 0)
                {
                    foreach (AccidentReasonAction accidentReasonAction in accidentReasonActions)
                    {
                        charts.Add(new ChartViewModel()
                        {
                            Label = accidentReasonAction.Title,
                            Y = 0
                        });
                    }

                    return charts;

                }



                foreach (AccidentReasonAction accidentReasonAction in accidentReasonActions)
                {
                    int typeTot = 0;
                    foreach (string companyId in companies)
                    {
                        Guid id = new Guid(companyId);

                        int itemCount =
                            db.AccidentReasonActionRelAccidents.Count(c =>
                                c.AccidentReasonActionId == accidentReasonAction.Id && c.Accident.User.CompanyId == id && c.IsDeleted == false);

                        typeTot += itemCount;
                    }


                    decimal count = (decimal)((decimal)((decimal)typeTot / (decimal)tot) * 100);

                    if (count > 0)
                    {
                        charts.Add(new ChartViewModel()
                        {
                            Label = accidentReasonAction.Title,
                            Y = Math.Round(count, 2)
                        });
                    }
                }

                return charts;
            }
        }

        public List<ChartViewModel> GetAccidentChartByReasonCondition(string[] companies)
        {
            if (companies == null)
            {
                List<AccidentReasonConditionRelAccident> accidentReasonConditionRelAccidents =
                    db.AccidentReasonConditionRelAccidents.Where(c => c.IsDeleted == false).ToList();

                int totalAccidents = accidentReasonConditionRelAccidents.Count();

                List<ChartViewModel> charts = new List<ChartViewModel>();
                List<AccidentReasonCondition> accidentReasonConditions = db.AccidentReasonConditions.Where(c => c.IsDeleted == false).ToList();

                foreach (AccidentReasonCondition accidentReasonCondition in accidentReasonConditions)
                {
                    int itemCount =
                        accidentReasonConditionRelAccidents.Count(c =>
                            c.AccidentReasonConditionId == accidentReasonCondition.Id && c.IsDeleted == false);

                    decimal count = (decimal)((decimal)((decimal)itemCount / (decimal)totalAccidents) * 100);
                    if (count > 0)
                    {
                        charts.Add(new ChartViewModel()
                        {
                            Label = accidentReasonCondition.Title,
                            Y = Math.Round(count, 2)
                        });
                    }
                }

                return charts;
            }
            else
            {
                int tot = 0;

                foreach (string companyId in companies)
                {
                    Guid id = new Guid(companyId);

                    int totalAccidents =
                        db.AccidentReasonConditionRelAccidents.Count(c => c.Accident.User.CompanyId == id && c.IsDeleted == false);

                    tot += totalAccidents;
                }

                List<ChartViewModel> charts = new List<ChartViewModel>();
                List<AccidentReasonCondition> accidentReasonConditions =
                    db.AccidentReasonConditions.Where(c => c.IsDeleted == false && c.IsActive).ToList();

                if (tot == 0)
                {
                    foreach (AccidentReasonCondition accidentReasonCondition in accidentReasonConditions)
                    {
                        charts.Add(new ChartViewModel()
                        {
                            Label = accidentReasonCondition.Title,
                            Y = 0
                        });
                    }

                    return charts;

                }



                foreach (AccidentReasonCondition accidentReasonCondition in accidentReasonConditions)
                {
                    int typeTot = 0;
                    foreach (string companyId in companies)
                    {
                        Guid id = new Guid(companyId);

                        int itemCount =
                            db.AccidentReasonConditionRelAccidents.Count(c =>
                                c.AccidentReasonConditionId == accidentReasonCondition.Id && c.Accident.User.CompanyId == id && c.IsDeleted == false);

                        typeTot += itemCount;
                    }


                    decimal count = (decimal)((decimal)((decimal)typeTot / (decimal)tot) * 100);

                    if (count > 0)
                    {
                        charts.Add(new ChartViewModel()
                        {
                            Label = accidentReasonCondition.Title,
                            Y = Math.Round(count, 2)
                        });
                    }
                }

                return charts;
            }
        }

        public List<ChartViewModel> GetAccidentChartByComplication()
        {


            List<Accident> accidents = db.Accidents.Where(c => c.IsDeleted == false && c.AccidentComplication != null).ToList();

            int first = 0;
            int second = 0;
            int third = 0;

            foreach (Accident accident in accidents)
            {
                if (accident.AccidentComplication.Contains("1"))
                    first++;
                if (accident.AccidentComplication.Contains("2"))
                    second++;
                if (accident.AccidentComplication.Contains("3"))
                    third++;
            }

            int totalAccidents = first + second + third;

            if (totalAccidents > 0)
            {
                decimal firstDecimal = (decimal)(((decimal)(first / totalAccidents) * 100));
                decimal secondDecimal =
                    (decimal)((decimal)((decimal)((decimal)second / (decimal)totalAccidents) * 100));
                decimal thirdDecimal =
                    (decimal)((decimal)((decimal)((decimal)third / (decimal)totalAccidents) * 100));

                List<ChartViewModel> charts = new List<ChartViewModel>
                {
                    new ChartViewModel()
                    {
                        Label = "ناشي از كار آماري ",
                        Y = firstDecimal
                    },
                    new ChartViewModel()
                    {
                        Label = "ناشي از كار غیرآماري ",
                        Y = secondDecimal
                    },
                    new ChartViewModel()
                    {
                        Label = "ناشي از كار ",
                        Y = thirdDecimal
                    },

                };

                return charts;
            }

            else
            {
                List<ChartViewModel> charts = new List<ChartViewModel>
                {
                    new ChartViewModel()
                    {
                        Label = "ناشي از كار آماري ",
                        Y = 0
                    },
                    new ChartViewModel()
                    {
                        Label = "ناشي از كار غیرآماري ",
                        Y = 0
                    },
                    new ChartViewModel()
                    {
                        Label = "ناشي از كار ",
                        Y = 0
                    },

                };
                return charts;

            }
        }

        public ActionResult MdlDashboard(Guid? companyTypeId)
        {
            List<CompanyType> companyTypes = db.CompanyTypes.Where(c => c.IsDeleted == false && c.IsActive).ToList();

            Guid selected = companyTypes.FirstOrDefault().Id;

            if (companyTypeId != null)
                selected = companyTypeId.Value;

            MdlDashboardViewModel result = new MdlDashboardViewModel()
            {
                CompanyTypes = new SelectList(companyTypes, "Id", "Title", selected),
                MdrItems = GetMdlItems(selected),
                SelectedCompanyTypeId = selected
            };
            return View(result);
        }

        public List<MdlTableViewModel> GetMdlItems(Guid companyTypeId)
        {
            List<MdlTableViewModel> result = new List<MdlTableViewModel>();

            List<Company> companies = db.Companies.Where(c => c.IsDeleted == false && c.CompanyTypeId == companyTypeId)
                .ToList();

            foreach (Company company in companies)
            {
                result.Add(new MdlTableViewModel()
                {
                    CompanyId = company.Id,
                    CompanyTitle = company.Title,
                    HsePlan = GetHseDocIsUploaded(company.Id, "HSE plan"),
                    Erp = GetHseDocIsUploaded(company.Id, "erp"),
                    RiskAssessment = GetHseDocIsUploaded(company.Id, "risk assessment"),
                    Esr = GetHseDocIsUploaded(company.Id, "esr"),

                });
            }

            return result;
        }

        public bool GetHseDocIsUploaded(Guid companyId, string hseDocumentType)
        {
            if (db.HseDocuments.Any(c =>
                c.CompanyId == companyId && c.HseDocumentType.Title.ToLower() == hseDocumentType))
                return true;

            return false;
        }


        public ActionResult AnomalyDashboard(string[] companyId, string companyTypeId)
        {
            List<Company> companies = new List<Company>();

           
                Guid coTypeId = new Guid(companyTypeId);
                companies = db.Companies.Where(c => c.CompanyTypeId == coTypeId && c.IsDeleted == false)
                    .OrderBy(c => c.Title).ToList();
           

            List<CompanyItemInDashboard> result = new List<CompanyItemInDashboard>();

            foreach (var company in companies)
            {
                result.Add(new CompanyItemInDashboard()
                {
                    Id = company.Id,
                    Title = company.Title,
                    IsSelected = false
                });

                if (companyId != null)
                {
                    if (companyId.Any(c => c == company.Id.ToString()))
                    {
                        result.LastOrDefault().IsSelected = true;
                    }
                }
                else
                {
                    result.LastOrDefault().IsSelected = true;
                }
            }

            List<CompanyType> companyTypes = db.CompanyTypes.Where(c => c.IsDeleted == false && c.IsActive).ToList();
            Guid selected = companyTypes.FirstOrDefault().Id;

            if (companyTypeId != null)
                selected = coTypeId;

            CompanyListDashboardViewModel res = new CompanyListDashboardViewModel()
            {
                Companies = result,
                CompanyTypes = new SelectList(companyTypes, "Id", "Title", selected),
                SelectedCompanyTypeId = selected
            };

            ViewBag.DataPointsAnomaly = JsonConvert.SerializeObject(GetAnomalyChart(companyId), _jsonSetting);
            ViewBag.DataPointsAnomalyByCompany = JsonConvert.SerializeObject(GetAnomalySubmitChart(companyId), _jsonSetting);

            return View(res);
        }


        public List<ChartViewModel> GetAnomalyChart(string[] companies)
        {
            Guid holdId = new Guid("37a92be4-045f-439f-9a0c-0b9d20f1a9aa");
            Guid openId = new Guid("0edebf8c-622b-4816-8f0b-ff06c676f37b");
            Guid completeId = new Guid("15af4452-e595-4128-9e2b-ba4ad9b3dacd");
            if (companies == null)
            {

                List<Anomaly> anomalies = db.Anomalies.Where(c => c.IsDeleted == false).ToList();
                int total = anomalies.Count();

                if (total == 0)
                {
                    List<ChartViewModel> chartsZero = new List<ChartViewModel>
                    {
                        new ChartViewModel()
                        {
                            Label = "Complete",
                            Y = 0
                        },
                        new ChartViewModel()
                        {
                            Label = "Open",
                            Y = 0
                        },
                        new ChartViewModel()
                        {
                            Label = "Hold",
                            Y = 0
                        },

                    };
                    return chartsZero;
                }

                int a = anomalies.Count(c => c.AnomalyResultId == holdId);
                int b = anomalies.Count(c => c.AnomalyResultId == openId);
                int dc = anomalies.Count(c => c.AnomalyResultId == completeId);
                decimal holdCount = (decimal)((decimal)((decimal)a / (decimal)total) * 100);
                decimal openCount = (decimal)((decimal)((decimal)b / (decimal)total) * 100);
                decimal completeCount = (decimal)((decimal)((decimal)dc / (decimal)total) * 100);


                List<ChartViewModel> charts = new List<ChartViewModel>
                {
                    new ChartViewModel()
                    {
                        Label = "Complete",
                        Y = completeCount
                    },
                    new ChartViewModel()
                    {
                        Label = "Open",
                        Y = openCount
                    },
                    new ChartViewModel()
                    {
                        Label = "Hold",
                        Y = holdCount
                    },

                };
                return charts;
            }
            else
            {

                int total = 0;
                foreach (string company in companies)
                {
                    Guid id = new Guid(company);

                    total += db.Anomalies.Count(c => c.CompanyId == id && c.IsDeleted == false);

                }

                if (total == 0)
                {
                    List<ChartViewModel> chartsZero = new List<ChartViewModel>
                    {
                        new ChartViewModel()
                        {
                            Label = "Complete",
                            Y = 0
                        },
                        new ChartViewModel()
                        {
                            Label = "Open",
                            Y = 0
                        },
                        new ChartViewModel()
                        {
                            Label = "Hold",
                            Y = 0
                        },

                    };
                    return chartsZero;
                }

                int aTotal = 0;
                foreach (string company in companies)
                {
                    Guid id = new Guid(company);

                    aTotal += db.Anomalies.Count(c => c.CompanyId == id && c.AnomalyResultId == holdId && c.IsDeleted == false);

                }

                int bTotal = 0;
                foreach (string company in companies)
                {
                    Guid id = new Guid(company);

                    bTotal += db.Anomalies.Count(c => c.CompanyId == id && c.AnomalyResultId == openId && c.IsDeleted == false);

                }

                int cTotal = 0;
                foreach (string company in companies)
                {
                    Guid id = new Guid(company);

                    cTotal += db.Anomalies.Count(c => c.CompanyId == id && c.AnomalyResultId == completeId && c.IsDeleted == false);

                }


                decimal holdCount = (decimal)((decimal)((decimal)aTotal / (decimal)total) * 100);
                decimal openCount = (decimal)((decimal)((decimal)bTotal / (decimal)total) * 100);
                decimal completeCount = (decimal)((decimal)((decimal)cTotal / (decimal)total) * 100);


                List<ChartViewModel> charts = new List<ChartViewModel>
                {
                    new ChartViewModel()
                    {
                        Label = "Complete",
                        Y = completeCount
                    },
                    new ChartViewModel()
                    {
                        Label = "Open",
                        Y = openCount
                    },
                    new ChartViewModel()
                    {
                        Label = "Hold",
                        Y = holdCount
                    },

                };
                return charts;
            }
        }


        public List<ChartViewModel> GetAnomalySubmitChart(string[] companyItems)
        {
            List<ChartViewModel> charts = new List<ChartViewModel>();

            List<Company> companies = new List<Company>();

            if (companyItems != null)
            {
                foreach (string item in companyItems)
                {
                    Guid companyId = new Guid(item);

                    companies.Add(db.Companies.Find(companyId));
                }
            }
            else
            {
                companies = db.Companies.Where(c => c.IsDeleted == false).ToList();
            }

            foreach (Company company in companies)
            {
                charts.Add(new ChartViewModel()
                {
                    Label = company.Title,
                    Y = db.Anomalies.Count(c => c.CompanyId == company.Id && c.IsDeleted == false)
                });
            }
            return charts;
        }



        public ActionResult ProgressDashboard(string[] companyId)
        {
            var companies = db.Companies.Where(c => c.IsDeleted == false).OrderBy(c => c.Title).ToList();

            List<CompanyType> companyTypes = db.CompanyTypes.Where(c => c.IsDeleted == false)
                .OrderBy(c => c.CreationDate).ToList();


            List<CompanyItemInDashboard> result = new List<CompanyItemInDashboard>();

            foreach (var company in companies)
            {
                result.Add(new CompanyItemInDashboard()
                {
                    Id = company.Id,
                    Title = company.Title,
                    IsSelected = false
                });

                if (companyId != null)
                {
                    if (companyId.Any(c => c == company.Id.ToString()))
                    {
                        result.LastOrDefault().IsSelected = true;
                    }
                }
                else
                {
                    result.LastOrDefault().IsSelected = true;
                }
            }


            CompanyListDashboardViewModel res = new CompanyListDashboardViewModel()
            {
                Companies = result
            };

            ViewBag.DataPointsAnomaly = JsonConvert.SerializeObject(GetAnomalyChart(companyId), _jsonSetting);
            ViewBag.DataPointsAnomalyByCompany = JsonConvert.SerializeObject(GetAnomalySubmitChart(companyId), _jsonSetting);

            return View(res);
        }


        public ActionResult CovidDashboard(Guid companyTypeId)
        {
            List<Company> companies = db.Companies.Where(c => c.CompanyTypeId == companyTypeId && c.IsDeleted == false).OrderBy(c => c.Title).ToList();

            List<CompanyType> companyTypes = db.CompanyTypes.Where(c => c.IsDeleted == false && c.IsActive).ToList();

            Guid selected = companyTypes.FirstOrDefault().Id;

            if (companyTypeId != null)
                selected = companyTypeId;

            MdlDashboardViewModel result = new MdlDashboardViewModel()
            {
                CompanyTypes = new SelectList(companyTypes, "Id", "Title", selected),
                MdrItems = GetMdlItems(selected),
                SelectedCompanyTypeId = selected
            };

            ViewBag.DataPointsCovidByCompany = JsonConvert.SerializeObject(GetCovidSubmitChart(companies), _jsonSetting);

            return View(result);
        }


        public List<ChartViewModel> GetCovidSubmitChart(List<Company> companies)
        {
            List<ChartViewModel> charts = new List<ChartViewModel>();


            foreach (Company company in companies)
            {
                charts.Add(new ChartViewModel()
                {
                    Label = company.Title,
                    Y = db.Covids.Count(c => c.CompanyId == company.Id && c.IsDeleted == false)
                });
            }
            return charts;
        }
    }
}