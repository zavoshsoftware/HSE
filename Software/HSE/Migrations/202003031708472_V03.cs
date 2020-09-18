namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V03 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Anomalies",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EventDate = c.DateTime(nullable: false),
                        Code = c.String(),
                        Summery = c.String(),
                        Place = c.String(),
                        Reporter = c.String(),
                        ResponseUser = c.String(),
                        Hse = c.String(),
                        ReportLevel = c.String(),
                        Deadline = c.DateTime(nullable: false),
                        Conclusion = c.String(),
                        EffectivnessDate = c.DateTime(nullable: false),
                        ImageUrl = c.String(),
                        Notes = c.String(),
                        SupervisorConfirm = c.Boolean(),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Anomalies", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Anomalies", new[] { "CompanyId" });
            DropTable("dbo.Anomalies");
        }
    }
}
