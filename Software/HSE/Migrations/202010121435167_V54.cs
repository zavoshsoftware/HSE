namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V54 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PassiveDefenses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        PassiveDefenseTypeId = c.Guid(nullable: false),
                        FileUrl = c.String(),
                        IsAcceptBySup = c.Boolean(nullable: false),
                        SupComment = c.String(),
                        CompanyId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.PassiveDefenseTypes", t => t.PassiveDefenseTypeId, cascadeDelete: true)
                .Index(t => t.PassiveDefenseTypeId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.PassiveDefenseTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PassiveDefenses", "PassiveDefenseTypeId", "dbo.PassiveDefenseTypes");
            DropForeignKey("dbo.PassiveDefenses", "CompanyId", "dbo.Companies");
            DropIndex("dbo.PassiveDefenses", new[] { "CompanyId" });
            DropIndex("dbo.PassiveDefenses", new[] { "PassiveDefenseTypeId" });
            DropTable("dbo.PassiveDefenseTypes");
            DropTable("dbo.PassiveDefenses");
        }
    }
}
