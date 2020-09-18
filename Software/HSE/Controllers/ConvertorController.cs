using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace HSE.Controllers
{
    public class ConvertorController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        public string Index()
        {
            //List<TempTable> temps = db.TempTables.ToList();

            //foreach (TempTable temp in temps)
            //{
            //    Guid projId=new Guid("68ABCCFE-2D12-4FD7-9B8B-90292CFD5714");

            //    if(temp.OperationGroupID==1)
            //        projId= new Guid("084C0AFC-9A3D-4243-A636-AB746B5EF045");


            //    Operation op =new Operation()
            //    {
            //        Id = Guid.NewGuid(),
            //        Title = temp.OperationTitle,
            //        CreationDate = DateTime.Now,
            //        Code = temp.CodeID,
            //        IsActive = true,
            //        IsDeleted = false,
            //        OldId = temp.OperationID,
            //        ProjectId = projId
            //    };
            //    db.Operations.Add(op);
            //}

            //db.SaveChanges();


            //List<TempTable_Act> temps = db.TempTable_Acts.ToList();

            //foreach (TempTable_Act temp in temps)
            //{

            //    Operation operation = db.Operations.FirstOrDefault(c => c.OldId == temp.OperationID);


            //    Act act = new Act()
            //    {
            //        Id = Guid.NewGuid(),
            //        Title = temp.ActTitle,
            //        CreationDate = DateTime.Now,
            //        Code = temp.CodeID,
            //        IsActive = true,
            //        IsDeleted = false,
            //        OldId = temp.ActID,
            //        OperationId = operation.Id,
            //        Courses = temp.Curses,
            //        ProtectionEquipment = temp.ProtectionEQP
            //    };
            //    db.Acts.Add(act);
            //}


            //List<TempTable_Stage> temps = db.TempTable_Stages.ToList();

            //foreach (TempTable_Stage temp in temps)
            //{

            //    Act act = db.Acts.FirstOrDefault(c => c.OldId == temp.ActID);


            //    Stage stage = new Stage()
            //    {
            //        Id = Guid.NewGuid(),
            //        Title = temp.StageTitle,
            //        CreationDate = DateTime.Now,
            //        Code = temp.CodeID,
            //        IsActive = true,
            //        IsDeleted = false,
            //        OldId = temp.StageID,
            //        ActId = act.Id,

            //    };
            //    db.Stages.Add(stage);
            //}



            //List<TempTable_Risk> temps = db.TempTable_Risks.ToList();

            //foreach (TempTable_Risk temp in temps)
            //{

            //    Stage stage = db.Stages.FirstOrDefault(c => c.OldId == temp.StageID);


            //    Risk risk = new Risk()
            //    {
            //        Id = Guid.NewGuid(),
            //        Title = temp.RiskTitle,
            //        CreationDate = DateTime.Now,
            //        Code = temp.CodeID,
            //        IsActive = true,
            //        IsDeleted = false,
            //        OldId = temp.RiskID,
            //        StageId = stage.Id,


            //    };
            //    db.Risks.Add(risk);
            //}


            //List<TempTable_Control> temps = db.TempTable_Controls.ToList();

            //foreach (TempTable_Control temp in temps)
            //{

            //    Risk risk = db.Risks.FirstOrDefault(c => c.OldId == temp.RiskID);


            //    RiskControlingWork riskCw = new RiskControlingWork()
            //    {
            //        Id = Guid.NewGuid(),
            //        Title = temp.ControlTitle,
            //        CreationDate = DateTime.Now,
            //        Code = temp.CodeID,
            //        IsActive = true,
            //        IsDeleted = false,
            //        OldId = temp.RiskID,
            //        RiskId = risk.Id,


            //    };
            //    db.RiskControlingWorks.Add(riskCw);
            //}


            List<TempTable_Risk> temps = db.TempTable_Risks.ToList();
            foreach (TempTable_Risk temp in temps)
            {
                Risk risk = db.Risks.FirstOrDefault(c => c.OldId == temp.RiskID);
                if (risk != null)
                    risk.Code = temp.UniqueId;
            }



            db.SaveChanges();
            return "ok";
        }
    }
}