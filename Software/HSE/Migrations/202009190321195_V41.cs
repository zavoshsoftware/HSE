namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V41 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Enviroments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CompanyId = c.Guid(nullable: false),
                        EnviromentTypeId = c.Guid(nullable: false),
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
                .ForeignKey("dbo.EnviromentTypes", t => t.EnviromentTypeId, cascadeDelete: true)
                .ForeignKey("dbo.PermitStatus", t => t.PermitStatusId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.EnviromentTypeId)
                .Index(t => t.PermitStatusId);
            
            CreateTable(
                "dbo.EnviromentTypes",
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
            DropForeignKey("dbo.Enviroments", "PermitStatusId", "dbo.PermitStatus");
            DropForeignKey("dbo.Enviroments", "EnviromentTypeId", "dbo.EnviromentTypes");
            DropForeignKey("dbo.Enviroments", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Enviroments", new[] { "PermitStatusId" });
            DropIndex("dbo.Enviroments", new[] { "EnviromentTypeId" });
            DropIndex("dbo.Enviroments", new[] { "CompanyId" });
            DropTable("dbo.EnviromentTypes");
            DropTable("dbo.Enviroments");
        }
    }
}
