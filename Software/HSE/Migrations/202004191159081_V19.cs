namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V19 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TempTable_Risk", "RiskIntensityID", c => c.Int());
            AlterColumn("dbo.TempTable_Risk", "RiskProbabilityID", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TempTable_Risk", "RiskProbabilityID", c => c.Int(nullable: false));
            AlterColumn("dbo.TempTable_Risk", "RiskIntensityID", c => c.Int(nullable: false));
        }
    }
}
