using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class RiskItemTableViewModel
    {
        public int Index { get; set; }
        public string Title { get; set; }
        public int Code { get; set; }
        public List<DropDownItemViewModel> RiskIntensity { get; set; }
        public List<DropDownItemViewModel> RiskProbability { get; set; }
    }
}