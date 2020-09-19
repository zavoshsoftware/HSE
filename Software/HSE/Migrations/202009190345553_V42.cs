namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V42 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Crises",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CompanyId = c.Guid(nullable: false),
                        CrisisTypeId = c.Guid(nullable: false),
                        FileUrl = c.String(),
                        PermitStatusId = c.Guid(nullable: false),
                        SuperVisorComment = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.CrisisTypes", t => t.CrisisTypeId, cascadeDelete: true)
                .ForeignKey("dbo.PermitStatus", t => t.PermitStatusId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.CrisisTypeId)
                .Index(t => t.PermitStatusId);
            
            CreateTable(
                "dbo.CrisisTypes",
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
            DropForeignKey("dbo.Crises", "PermitStatusId", "dbo.PermitStatus");
            DropForeignKey("dbo.Crises", "CrisisTypeId", "dbo.CrisisTypes");
            DropForeignKey("dbo.Crises", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Crises", new[] { "PermitStatusId" });
            DropIndex("dbo.Crises", new[] { "CrisisTypeId" });
            DropIndex("dbo.Crises", new[] { "CompanyId" });
            DropTable("dbo.CrisisTypes");
            DropTable("dbo.Crises");
        }
    }
}
