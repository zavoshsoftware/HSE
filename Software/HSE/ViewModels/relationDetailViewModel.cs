using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class relationDetailViewModel
    {
        public Relation Relation { get; set; }
        public List<RelationImage> RelationImages { get; set; }
    }
}