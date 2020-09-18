namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V24 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyStatusReports",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FileUrl = c.String(),
                        Title = c.String(),
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
                .Index(t => t.CompanyId);
            
            AddColumn("dbo.Companies", "ContractItemFileUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyStatusReports", "CompanyId", "dbo.Companies");
            DropIndex("dbo.CompanyStatusReports", new[] { "CompanyId" });
            DropColumn("dbo.Companies", "ContractItemFileUrl");
            DropTable("dbo.CompanyStatusReports");
        }
    }
}
