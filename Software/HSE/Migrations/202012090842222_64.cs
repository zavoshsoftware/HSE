namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _64 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Progresses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Start = c.DateTime(nullable: false),
                        Finish = c.DateTime(nullable: false),
                        CompanyPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SupPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AdminPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CompanyId = c.Guid(nullable: false),
                        ProgressGroupId = c.Guid(nullable: false),
                        ImageUrl = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.ProgressGroups", t => t.ProgressGroupId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.ProgressGroupId);
            
            CreateTable(
                "dbo.ProgressGroups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        MaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
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
            DropForeignKey("dbo.Progresses", "ProgressGroupId", "dbo.ProgressGroups");
            DropForeignKey("dbo.Progresses", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Progresses", new[] { "ProgressGroupId" });
            DropIndex("dbo.Progresses", new[] { "CompanyId" });
            DropTable("dbo.ProgressGroups");
            DropTable("dbo.Progresses");
        }
    }
}
