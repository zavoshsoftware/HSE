namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V07 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Anomalies", "StatusId", c => c.Guid(nullable: false));
            AddColumn("dbo.Companies", "OfficialEmployee", c => c.Int());
            AddColumn("dbo.Companies", "ContractEmployee", c => c.Int());
            CreateIndex("dbo.Anomalies", "StatusId");
            AddForeignKey("dbo.Anomalies", "StatusId", "dbo.Status", "Id", cascadeDelete: true);
            DropColumn("dbo.Anomalies", "IsCheckBySupervisor");
            DropColumn("dbo.Anomalies", "SupervisorConfirm");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Anomalies", "SupervisorConfirm", c => c.Boolean(nullable: false));
            AddColumn("dbo.Anomalies", "IsCheckBySupervisor", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Anomalies", "StatusId", "dbo.Status");
            DropIndex("dbo.Anomalies", new[] { "StatusId" });
            DropColumn("dbo.Companies", "ContractEmployee");
            DropColumn("dbo.Companies", "OfficialEmployee");
            DropColumn("dbo.Anomalies", "StatusId");
        }
    }
}
