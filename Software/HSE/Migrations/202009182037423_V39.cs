namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V39 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Permits",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CompanyId = c.Guid(nullable: false),
                        PermitTypeId = c.Guid(nullable: false),
                        FileUrl = c.String(),
                        IsAcceptBySupervisor = c.Boolean(),
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
                .ForeignKey("dbo.PermitTypes", t => t.PermitTypeId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.PermitTypeId);
            
            CreateTable(
                "dbo.PermitTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        FileUrl = c.String(),
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
            DropForeignKey("dbo.Permits", "PermitTypeId", "dbo.PermitTypes");
            DropForeignKey("dbo.Permits", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Permits", new[] { "PermitTypeId" });
            DropIndex("dbo.Permits", new[] { "CompanyId" });
            DropTable("dbo.PermitTypes");
            DropTable("dbo.Permits");
        }
    }
}
