namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V25 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportTypes", "SampleFile", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReportTypes", "SampleFile");
        }
    }
}
