namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V63 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reports", "ReportDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reports", "ReportDate", c => c.DateTime());
        }
    }
}
