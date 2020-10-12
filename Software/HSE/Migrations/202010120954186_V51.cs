namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V51 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Relations", "CompanyId", c => c.Guid());
            CreateIndex("dbo.Relations", "CompanyId");
            AddForeignKey("dbo.Relations", "CompanyId", "dbo.Companies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Relations", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Relations", new[] { "CompanyId" });
            DropColumn("dbo.Relations", "CompanyId");
        }
    }
}
