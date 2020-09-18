namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V06 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WbsDocuments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        WbsUserTypeId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WbsUserTypes", t => t.WbsUserTypeId, cascadeDelete: true)
                .Index(t => t.WbsUserTypeId);
            
            CreateTable(
                "dbo.WbsUserTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        WbsRequirmentId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WbsRequirments", t => t.WbsRequirmentId, cascadeDelete: true)
                .Index(t => t.WbsRequirmentId);
            
            CreateTable(
                "dbo.WbsRequirments",
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
            DropForeignKey("dbo.WbsUserTypes", "WbsRequirmentId", "dbo.WbsRequirments");
            DropForeignKey("dbo.WbsDocuments", "WbsUserTypeId", "dbo.WbsUserTypes");
            DropIndex("dbo.WbsUserTypes", new[] { "WbsRequirmentId" });
            DropIndex("dbo.WbsDocuments", new[] { "WbsUserTypeId" });
            DropTable("dbo.WbsRequirments");
            DropTable("dbo.WbsUserTypes");
            DropTable("dbo.WbsDocuments");
        }
    }
}
