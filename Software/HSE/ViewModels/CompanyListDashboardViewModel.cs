using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class CompanyListDashboardViewModel
    {
      
        public List<CompanyItemInDashboard> Companies { get; set; }

    
    }

    public class CompanyItemInDashboard
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsSelected { get; set; }
    }
    
}