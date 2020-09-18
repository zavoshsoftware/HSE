namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V02 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "SupervisorUserId", c => c.Guid());
            CreateIndex("dbo.Companies", "SupervisorUserId");
            AddForeignKey("dbo.Companies", "SupervisorUserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Companies", "SupervisorUserId", "dbo.Users");
            DropIndex("dbo.Companies", new[] { "SupervisorUserId" });
            DropColumn("dbo.Companies", "SupervisorUserId");
        }
    }
}
