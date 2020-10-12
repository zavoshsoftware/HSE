using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class ReportIndexViewModel
    {
        public List<Report> Reports { get; set; }
        public List<ReportType> ReportTypes { get; set; }
    }
     
}