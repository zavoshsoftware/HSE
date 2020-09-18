using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class UserRiskFormViewModel
    {
        public List<Project> Projects { get; set; }
        public string ProbDesc { get; set; }
        public string IntenDesc { get; set; }
    }
     
}