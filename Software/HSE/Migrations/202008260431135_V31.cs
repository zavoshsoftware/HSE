namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V31 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accidents", "UserId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Accidents", "UserId");
            AddForeignKey("dbo.Accidents", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accidents", "UserId", "dbo.Users");
            DropIndex("dbo.Accidents", new[] { "UserId" });
            DropColumn("dbo.Accidents", "UserId");
        }
    }
}
