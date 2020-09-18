using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class TempTable
    {
        [Key]
        public int OperationID { get; set; }

        public int OperationGroupID { get; set; }
        public int CodeID { get; set; }
        public string OperationTitle { get; set; }
        public bool IsAcceptedByAdmin { get; set; }
    }
    public class TempTable_Act
    {
        [Key]
        public int ActID { get; set; }

        public int OperationID { get; set; }
        public int CodeID { get; set; }
        public string ActTitle { get; set; }
        public bool IsAcceptedByAdmin { get; set; }
        public string ProtectionEQP { get; set; }
        public string Curses { get; set; }
    }
    public class TempTable_Stage
    {
        public int ActID { get; set; }

        [Key]
        public int StageID { get; set; }
        public int CodeID { get; set; }
        public string StageTitle { get; set; }
        public bool IsAcceptedByAdmin { get; set; }
    }
    public class TempTable_Risk
    {
        public int StageID { get; set; }

        [Key]
        public int RiskID { get; set; }
        public int CodeID { get; set; }
        public string RiskTitle { get; set; }
        public bool IsAcceptedByAdmin { get; set; }
        public bool IsNormal { get; set; }
        public int? RiskIntensityID { get; set; }
        public int? RiskProbabilityID { get; set; }
        public int UniqueId { get; set; }
    }
    public class TempTable_Control
    {
        [Key]
        public int ControlID { get; set; }

        public int RiskID { get; set; }
        public int CodeID { get; set; }
        public string ControlTitle { get; set; }
        public bool IsAcceptedByAdmin { get; set; }
   
    }
}