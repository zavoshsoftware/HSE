namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V05 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ReportDate = c.DateTime(nullable: false),
                        CompanyId = c.Guid(nullable: false),
                        FileUrl = c.String(),
                        StatusId = c.Guid(nullable: false),
                        ReportTypeId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.ReportTypes", t => t.ReportTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.StatusId)
                .Index(t => t.ReportTypeId);
            
            CreateTable(
                "dbo.ReportTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Order = c.Int(nullable: false),
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
            DropForeignKey("dbo.Reports", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Reports", "ReportTypeId", "dbo.ReportTypes");
            DropForeignKey("dbo.Reports", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Reports", new[] { "ReportTypeId" });
            DropIndex("dbo.Reports", new[] { "StatusId" });
            DropIndex("dbo.Reports", new[] { "CompanyId" });
            DropTable("dbo.Status");
            DropTable("dbo.ReportTypes");
            DropTable("dbo.Reports");
        }
    }
}
