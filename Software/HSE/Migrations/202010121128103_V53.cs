namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V53 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportTypes", "ParentId", c => c.Guid());
            CreateIndex("dbo.ReportTypes", "ParentId");
            AddForeignKey("dbo.ReportTypes", "ParentId", "dbo.ReportTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReportTypes", "ParentId", "dbo.ReportTypes");
            DropIndex("dbo.ReportTypes", new[] { "ParentId" });
            DropColumn("dbo.ReportTypes", "ParentId");
        }
    }
}
