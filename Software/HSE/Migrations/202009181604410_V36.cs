namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V36 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompanyUsers", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.CompanyUsers", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "UserJobRateId", "dbo.UserJobRates");
            DropIndex("dbo.Users", new[] { "UserJobRateId" });
            DropIndex("dbo.CompanyUsers", new[] { "CompanyId" });
            DropIndex("dbo.CompanyUsers", new[] { "UserId" });
            CreateTable(
                "dbo.CompanyHumanResources",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        UserJobRateId = c.Guid(nullable: false),
                        Degree = c.String(),
                        ResumeFileUrl = c.String(),
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
                .ForeignKey("dbo.UserJobRates", t => t.UserJobRateId, cascadeDelete: true)
                .Index(t => t.UserJobRateId)
                .Index(t => t.CompanyId);
            
            DropColumn("dbo.Users", "UserJobRateId");
            DropColumn("dbo.Users", "Degree");
            DropColumn("dbo.Users", "ResumeFileUrl");
            DropTable("dbo.CompanyUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CompanyUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CompanyId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        PositionTitle = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "ResumeFileUrl", c => c.String());
            AddColumn("dbo.Users", "Degree", c => c.String());
            AddColumn("dbo.Users", "UserJobRateId", c => c.Guid());
            DropForeignKey("dbo.CompanyHumanResources", "UserJobRateId", "dbo.UserJobRates");
            DropForeignKey("dbo.CompanyHumanResources", "CompanyId", "dbo.Companies");
            DropIndex("dbo.CompanyHumanResources", new[] { "CompanyId" });
            DropIndex("dbo.CompanyHumanResources", new[] { "UserJobRateId" });
            DropTable("dbo.CompanyHumanResources");
            CreateIndex("dbo.CompanyUsers", "UserId");
            CreateIndex("dbo.CompanyUsers", "CompanyId");
            CreateIndex("dbo.Users", "UserJobRateId");
            AddForeignKey("dbo.Users", "UserJobRateId", "dbo.UserJobRates", "Id");
            AddForeignKey("dbo.CompanyUsers", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CompanyUsers", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
    }
}
