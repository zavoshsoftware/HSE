namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V47 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Safeties",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        SafetyTypeId = c.Guid(nullable: false),
                        SafetyFileTypeId = c.Guid(),
                        FileUrl = c.String(),
                        IsAccepteBySupervisor = c.Boolean(nullable: false),
                        SupervisorComment = c.String(),
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
                .ForeignKey("dbo.SafetyFileTypes", t => t.SafetyFileTypeId)
                .ForeignKey("dbo.SafetyTypes", t => t.SafetyTypeId, cascadeDelete: true)
                .Index(t => t.SafetyTypeId)
                .Index(t => t.SafetyFileTypeId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.SafetyFileTypes",
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
            
            CreateTable(
                "dbo.SafetyTypes",
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
            DropForeignKey("dbo.Safeties", "SafetyTypeId", "dbo.SafetyTypes");
            DropForeignKey("dbo.Safeties", "SafetyFileTypeId", "dbo.SafetyFileTypes");
            DropForeignKey("dbo.Safeties", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Safeties", new[] { "CompanyId" });
            DropIndex("dbo.Safeties", new[] { "SafetyFileTypeId" });
            DropIndex("dbo.Safeties", new[] { "SafetyTypeId" });
            DropTable("dbo.SafetyTypes");
            DropTable("dbo.SafetyFileTypes");
            DropTable("dbo.Safeties");
        }
    }
}
