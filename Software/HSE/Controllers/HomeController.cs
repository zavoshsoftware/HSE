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
        public ActionResult Dashboard2()
        {
            return View();
        }
        [Authorize]
        public ActionResult Dashboard3()
        {

            ViewBag.DataPoints = JsonConvert.SerializeObject(GetAnomalyChart(), _jsonSetting);
            ViewBag.DataPointsAccident = JsonConvert.SerializeObject(GetAccidentChartByType(), _jsonSetting);
            ViewBag.DataPointsAccidentInjury = JsonConvert.SerializeObject(GetAccidentChartByInjuryType(), _jsonSetting);
            ViewBag.DataPointsAccidentComplication = JsonConvert.SerializeObject(GetAccidentChartByComplication(), _jsonSetting);

            return View();
        }
        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };


         
        public List<ChartViewModel> GetAnomalyChart()
        {
            Guid holdId = new Guid("37a92be4-045f-439f-9a0c-0b9d20f1a9aa");
            Guid openId = new Guid("0edebf8c-622b-4816-8f0b-ff06c676f37b");
            Guid completeId = new Guid("15af4452-e595-4128-9e2b-ba4ad9b3dacd");
            List<Anomaly> anomalies = db.Anomalies.Where(c => c.IsDeleted == false).ToList();
            int total = anomalies.Count();
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
                    Y =completeCount
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


        public List<ChartViewModel> GetAccidentChartByType()
        {
            int totalAccidents = db.AccidentTypes.Count(c => c.IsDeleted == false);


            List<AccidentType> accidentTypes = db.AccidentTypes.Where(c => c.IsDeleted == false && c.IsActive).ToList();

            List<ChartViewModel> charts = new List<ChartViewModel>();

            foreach (AccidentType accidentType in accidentTypes)
            {
                int itemCount =
                    db.AccidentTypeRelAccidents.Count(c => c.AccidentTypeId == accidentType.Id && c.IsDeleted == false);

                decimal count = (decimal)((decimal)((decimal)itemCount / (decimal)totalAccidents) * 100);

                charts.Add(new ChartViewModel()
                {
                    Label = accidentType.Title,
                    Y = count
                });
            }

            return charts;
        }

        public List<ChartViewModel> GetAccidentChartByInjuryType()
        {


            List<AccidentInjury> accidentInjuries = db.AccidentInjuries.Where(c => c.IsDeleted == false ).ToList();

            int totalAccidents = accidentInjuries.Count();

            List<ChartViewModel> charts = new List<ChartViewModel>();

            foreach (AccidentInjury accidentInjury in accidentInjuries)
            {
                int itemCount =
                    db.AccidentInjuryRelAccidents.Count(c => c.AccidentInjuryId == accidentInjury.Id && c.IsDeleted == false);

                decimal count = (decimal)((decimal)((decimal)itemCount / (decimal)totalAccidents) * 100);

                charts.Add(new ChartViewModel()
                {
                    Label = accidentInjury.Title,
                    Y = count
                });
            }

            return charts;
        }

        public List<ChartViewModel> GetAccidentChartByComplication()
        {


            List<Accident> accidents = db.Accidents.Where(c => c.IsDeleted == false &&c.AccidentComplication!=null).ToList();

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
                decimal firstDecimal = (decimal) (((decimal) (first / totalAccidents) * 100));
                decimal secondDecimal =
                    (decimal) ((decimal) ((decimal) ((decimal) second / (decimal) totalAccidents) * 100));
                decimal thirdDecimal =
                    (decimal) ((decimal) ((decimal) ((decimal) third / (decimal) totalAccidents) * 100));

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

    }
}