namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V27 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccidentReportRelAccidents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccidentId = c.Guid(nullable: false),
                        AccidentReportId = c.Guid(nullable: false),
                        FileUrl = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accidents", t => t.AccidentId, cascadeDelete: true)
                .ForeignKey("dbo.AccidentReports", t => t.AccidentReportId, cascadeDelete: true)
                .Index(t => t.AccidentId)
                .Index(t => t.AccidentReportId);
            
            CreateTable(
                "dbo.AccidentReports",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Deadline = c.String(),
                        DeliveryFormat = c.String(),
                        ResponsibleReporter = c.String(),
                        No = c.Int(nullable: false),
                        SendPlace = c.String(),
                        Attachments = c.String(),
                        BaseFileUrl = c.String(),
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
            DropForeignKey("dbo.AccidentReportRelAccidents", "AccidentReportId", "dbo.AccidentReports");
            DropForeignKey("dbo.AccidentReportRelAccidents", "AccidentId", "dbo.Accidents");
            DropIndex("dbo.AccidentReportRelAccidents", new[] { "AccidentReportId" });
            DropIndex("dbo.AccidentReportRelAccidents", new[] { "AccidentId" });
            DropTable("dbo.AccidentReports");
            DropTable("dbo.AccidentReportRelAccidents");
        }
    }
}
