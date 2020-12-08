namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V62 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reports", "ReportDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reports", "ReportDate", c => c.DateTime(nullable: false));
        }
    }
}
