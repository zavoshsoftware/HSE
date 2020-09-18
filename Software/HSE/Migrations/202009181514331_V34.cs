namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V34 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserJobRates",
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
            
            AddColumn("dbo.Users", "UserJobRateId", c => c.Guid());
            AddColumn("dbo.Users", "Degree", c => c.String());
            AddColumn("dbo.Users", "ResumeFileUrl", c => c.String());
            CreateIndex("dbo.Users", "UserJobRateId");
            AddForeignKey("dbo.Users", "UserJobRateId", "dbo.UserJobRates", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "UserJobRateId", "dbo.UserJobRates");
            DropIndex("dbo.Users", new[] { "UserJobRateId" });
            DropColumn("dbo.Users", "ResumeFileUrl");
            DropColumn("dbo.Users", "Degree");
            DropColumn("dbo.Users", "UserJobRateId");
            DropTable("dbo.UserJobRates");
        }
    }
}
