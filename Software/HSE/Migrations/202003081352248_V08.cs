namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V08 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnomalyHses",
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
            
            CreateTable(
                "dbo.AnomalyLevels",
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
            
            CreateTable(
                "dbo.AnomalyResults",
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
            
            AddColumn("dbo.Anomalies", "ResponseUserId", c => c.Guid());
            AddColumn("dbo.Anomalies", "CreatorUserId", c => c.Guid());
            AddColumn("dbo.Anomalies", "AnomalyHseId", c => c.Guid(nullable: false));
            AddColumn("dbo.Anomalies", "AnomalyLevelId", c => c.Guid(nullable: false));
            AddColumn("dbo.Anomalies", "AnomalyResultId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Anomalies", "ResponseUserId");
            CreateIndex("dbo.Anomalies", "CreatorUserId");
            CreateIndex("dbo.Anomalies", "AnomalyHseId");
            CreateIndex("dbo.Anomalies", "AnomalyLevelId");
            CreateIndex("dbo.Anomalies", "AnomalyResultId");
            AddForeignKey("dbo.Anomalies", "AnomalyHseId", "dbo.AnomalyHses", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Anomalies", "AnomalyLevelId", "dbo.AnomalyLevels", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Anomalies", "AnomalyResultId", "dbo.AnomalyResults", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Anomalies", "ResponseUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.Anomalies", "CreatorUserId", "dbo.Users", "Id");
            DropColumn("dbo.Anomalies", "ResponseUser");
            DropColumn("dbo.Anomalies", "Hse");
            DropColumn("dbo.Anomalies", "ReportLevel");
            DropColumn("dbo.Anomalies", "Conclusion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Anomalies", "Conclusion", c => c.String());
            AddColumn("dbo.Anomalies", "ReportLevel", c => c.String());
            AddColumn("dbo.Anomalies", "Hse", c => c.String());
            AddColumn("dbo.Anomalies", "ResponseUser", c => c.String());
            DropForeignKey("dbo.Anomalies", "CreatorUserId", "dbo.Users");
            DropForeignKey("dbo.Anomalies", "ResponseUserId", "dbo.Users");
            DropForeignKey("dbo.Anomalies", "AnomalyResultId", "dbo.AnomalyResults");
            DropForeignKey("dbo.Anomalies", "AnomalyLevelId", "dbo.AnomalyLevels");
            DropForeignKey("dbo.Anomalies", "AnomalyHseId", "dbo.AnomalyHses");
            DropIndex("dbo.Anomalies", new[] { "AnomalyResultId" });
            DropIndex("dbo.Anomalies", new[] { "AnomalyLevelId" });
            DropIndex("dbo.Anomalies", new[] { "AnomalyHseId" });
            DropIndex("dbo.Anomalies", new[] { "CreatorUserId" });
            DropIndex("dbo.Anomalies", new[] { "ResponseUserId" });
            DropColumn("dbo.Anomalies", "AnomalyResultId");
            DropColumn("dbo.Anomalies", "AnomalyLevelId");
            DropColumn("dbo.Anomalies", "AnomalyHseId");
            DropColumn("dbo.Anomalies", "CreatorUserId");
            DropColumn("dbo.Anomalies", "ResponseUserId");
            DropTable("dbo.AnomalyResults");
            DropTable("dbo.AnomalyLevels");
            DropTable("dbo.AnomalyHses");
        }
    }
}
