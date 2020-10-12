namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V50 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PermitTypes", "CompanyTypeId", c => c.Guid());
            CreateIndex("dbo.PermitTypes", "CompanyTypeId");
            AddForeignKey("dbo.PermitTypes", "CompanyTypeId", "dbo.CompanyTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PermitTypes", "CompanyTypeId", "dbo.CompanyTypes");
            DropIndex("dbo.PermitTypes", new[] { "CompanyTypeId" });
            DropColumn("dbo.PermitTypes", "CompanyTypeId");
        }
    }
}
