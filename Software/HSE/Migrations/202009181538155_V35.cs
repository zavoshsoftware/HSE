namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V35 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Companies", "SupervisorUserId", "dbo.Users");
            AddColumn("dbo.Users", "CompanyId", c => c.Guid());
            AddColumn("dbo.Users", "Company_Id", c => c.Guid());
            AddColumn("dbo.Companies", "User_Id", c => c.Guid());
            CreateIndex("dbo.Users", "CompanyId");
            CreateIndex("dbo.Users", "Company_Id");
            CreateIndex("dbo.Companies", "User_Id");
            AddForeignKey("dbo.Users", "Company_Id", "dbo.Companies", "Id");
            AddForeignKey("dbo.Users", "CompanyId", "dbo.Companies", "Id");
            AddForeignKey("dbo.Companies", "User_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Companies", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Users", "Company_Id", "dbo.Companies");
            DropIndex("dbo.Companies", new[] { "User_Id" });
            DropIndex("dbo.Users", new[] { "Company_Id" });
            DropIndex("dbo.Users", new[] { "CompanyId" });
            DropColumn("dbo.Companies", "User_Id");
            DropColumn("dbo.Users", "Company_Id");
            DropColumn("dbo.Users", "CompanyId");
            AddForeignKey("dbo.Companies", "SupervisorUserId", "dbo.Users", "Id");
        }
    }
}
