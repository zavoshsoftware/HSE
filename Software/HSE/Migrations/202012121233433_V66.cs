namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V66 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Covids",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FullName = c.String(),
                        SickDate = c.DateTime(nullable: false),
                        SafeDate = c.DateTime(nullable: false),
                        QuarantineDays = c.Int(nullable: false),
                        CovidTypeId = c.Guid(nullable: false),
                        ImageUrl = c.String(),
                        CovidStatusId = c.Guid(nullable: false),
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
                .ForeignKey("dbo.CovidStatus", t => t.CovidStatusId, cascadeDelete: true)
                .ForeignKey("dbo.CovidTypes", t => t.CovidTypeId, cascadeDelete: true)
                .Index(t => t.CovidTypeId)
                .Index(t => t.CovidStatusId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.CovidStatus",
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
                "dbo.CovidTypes",
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
            DropForeignKey("dbo.Covids", "CovidTypeId", "dbo.CovidTypes");
            DropForeignKey("dbo.Covids", "CovidStatusId", "dbo.CovidStatus");
            DropForeignKey("dbo.Covids", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Covids", new[] { "CompanyId" });
            DropIndex("dbo.Covids", new[] { "CovidStatusId" });
            DropIndex("dbo.Covids", new[] { "CovidTypeId" });
            DropTable("dbo.CovidTypes");
            DropTable("dbo.CovidStatus");
            DropTable("dbo.Covids");
        }
    }
}
