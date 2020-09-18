namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V04 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Anomalies", "IsCheckBySupervisor", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Anomalies", "SupervisorConfirm", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Anomalies", "SupervisorConfirm", c => c.Boolean());
            DropColumn("dbo.Anomalies", "IsCheckBySupervisor");
        }
    }
}
