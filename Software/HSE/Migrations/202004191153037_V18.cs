namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V18 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempTable_Control",
                c => new
                    {
                        ControlID = c.Int(nullable: false, identity: true),
                        RiskID = c.Int(nullable: false),
                        CodeID = c.Int(nullable: false),
                        ControlTitle = c.String(),
                        IsAcceptedByAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ControlID);
            
            CreateTable(
                "dbo.TempTable_Risk",
                c => new
                    {
                        RiskID = c.Int(nullable: false, identity: true),
                        StageID = c.Int(nullable: false),
                        CodeID = c.Int(nullable: false),
                        RiskTitle = c.String(),
                        IsAcceptedByAdmin = c.Boolean(nullable: false),
                        IsNormal = c.Boolean(nullable: false),
                        RiskIntensityID = c.Int(nullable: false),
                        RiskProbabilityID = c.Int(nullable: false),
                        UniqueId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RiskID);
            
            CreateTable(
                "dbo.TempTable_Stage",
                c => new
                    {
                        StageID = c.Int(nullable: false, identity: true),
                        ActID = c.Int(nullable: false),
                        CodeID = c.Int(nullable: false),
                        StageTitle = c.String(),
                        IsAcceptedByAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StageID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TempTable_Stage");
            DropTable("dbo.TempTable_Risk");
            DropTable("dbo.TempTable_Control");
        }
    }
}
