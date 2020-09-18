namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V23 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserRisks", "RiskNumber", c => c.Int(nullable: false));
            AddColumn("dbo.UserRisks", "RiskDescription", c => c.String());
            AddColumn("dbo.UserRisks", "AfterControlRiskNumber", c => c.Int());
            AddColumn("dbo.UserRisks", "AfterControlRiskDescription", c => c.String());
            AddColumn("dbo.RiskStatus", "Code", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RiskStatus", "Code");
            DropColumn("dbo.UserRisks", "AfterControlRiskDescription");
            DropColumn("dbo.UserRisks", "AfterControlRiskNumber");
            DropColumn("dbo.UserRisks", "RiskDescription");
            DropColumn("dbo.UserRisks", "RiskNumber");
        }
    }
}
