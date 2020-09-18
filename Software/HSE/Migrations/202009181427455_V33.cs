namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V33 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HsePlans",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        CompanyId = c.Guid(nullable: false),
                        FileUrl = c.String(),
                        SupervisorComment = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CompanyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HsePlans", "UserId", "dbo.Users");
            DropForeignKey("dbo.HsePlans", "CompanyId", "dbo.Companies");
            DropIndex("dbo.HsePlans", new[] { "CompanyId" });
            DropIndex("dbo.HsePlans", new[] { "UserId" });
            DropTable("dbo.HsePlans");
        }
    }
}
