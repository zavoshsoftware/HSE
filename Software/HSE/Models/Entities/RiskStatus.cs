using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class RiskStatus : BaseEntity
    {
        public int Order { get; set; }
        public string Title { get; set; }
        public int Code { get; set; }
        public virtual ICollection<UserStage> UserStages { get; set; }

    }
}