namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V55 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PassiveDefenses", "CompanyId", "dbo.Companies");
            DropIndex("dbo.PassiveDefenses", new[] { "CompanyId" });
            AlterColumn("dbo.PassiveDefenses", "CompanyId", c => c.Guid());
            CreateIndex("dbo.PassiveDefenses", "CompanyId");
            AddForeignKey("dbo.PassiveDefenses", "CompanyId", "dbo.Companies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PassiveDefenses", "CompanyId", "dbo.Companies");
            DropIndex("dbo.PassiveDefenses", new[] { "CompanyId" });
            AlterColumn("dbo.PassiveDefenses", "CompanyId", c => c.Guid(nullable: false));
            CreateIndex("dbo.PassiveDefenses", "CompanyId");
            AddForeignKey("dbo.PassiveDefenses", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
    }
}
