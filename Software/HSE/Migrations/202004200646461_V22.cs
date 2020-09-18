namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V22 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserRisks", "RiskStatusId", "dbo.RiskStatus");
            DropForeignKey("dbo.UserRisks", "UserId", "dbo.Users");
            DropIndex("dbo.UserRisks", new[] { "UserId" });
            DropIndex("dbo.UserRisks", new[] { "RiskStatusId" });
            CreateTable(
                "dbo.UserStages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        StageId = c.Guid(),
                        RiskStatusId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RiskStatus", t => t.RiskStatusId, cascadeDelete: true)
                .ForeignKey("dbo.Stages", t => t.StageId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.StageId)
                .Index(t => t.RiskStatusId);
            
            AddColumn("dbo.UserRisks", "UserStageId", c => c.Guid(nullable: false));
            CreateIndex("dbo.UserRisks", "UserStageId");
            AddForeignKey("dbo.UserRisks", "UserStageId", "dbo.UserStages", "Id", cascadeDelete: true);
            DropColumn("dbo.UserRisks", "UserId");
            DropColumn("dbo.UserRisks", "RiskStatusId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserRisks", "RiskStatusId", c => c.Guid(nullable: false));
            AddColumn("dbo.UserRisks", "UserId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.UserRisks", "UserStageId", "dbo.UserStages");
            DropForeignKey("dbo.UserStages", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserStages", "StageId", "dbo.Stages");
            DropForeignKey("dbo.UserStages", "RiskStatusId", "dbo.RiskStatus");
            DropIndex("dbo.UserStages", new[] { "RiskStatusId" });
            DropIndex("dbo.UserStages", new[] { "StageId" });
            DropIndex("dbo.UserStages", new[] { "UserId" });
            DropIndex("dbo.UserRisks", new[] { "UserStageId" });
            DropColumn("dbo.UserRisks", "UserStageId");
            DropTable("dbo.UserStages");
            CreateIndex("dbo.UserRisks", "RiskStatusId");
            CreateIndex("dbo.UserRisks", "UserId");
            AddForeignKey("dbo.UserRisks", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserRisks", "RiskStatusId", "dbo.RiskStatus", "Id", cascadeDelete: true);
        }
    }
}
