namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V14 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Acts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Code = c.Int(nullable: false),
                        OperationId = c.Guid(nullable: false),
                        ProtectionEquipment = c.String(),
                        Courses = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Operations", t => t.OperationId, cascadeDelete: true)
                .Index(t => t.OperationId);
            
            CreateTable(
                "dbo.Operations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Code = c.Int(nullable: false),
                        ProjectId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Stages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Code = c.Int(nullable: false),
                        ActId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Acts", t => t.ActId, cascadeDelete: true)
                .Index(t => t.ActId);
            
            CreateTable(
                "dbo.Risks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Code = c.Int(nullable: false),
                        StageId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stages", t => t.StageId, cascadeDelete: true)
                .Index(t => t.StageId);
            
            CreateTable(
                "dbo.RiskControlingWorks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Code = c.Int(nullable: false),
                        RiskId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Risks", t => t.RiskId, cascadeDelete: true)
                .Index(t => t.RiskId);
            
            CreateTable(
                "dbo.UserRisks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        RiskId = c.Guid(nullable: false),
                        RiskIntensityId = c.Guid(nullable: false),
                        RiskProbabilityId = c.Guid(nullable: false),
                        AfterControlRiskIntensityId = c.Guid(),
                        AfterControlRiskProbabilityId = c.Guid(),
                        RiskStatusId = c.Guid(nullable: false),
                        IsAcceptedBySupervisor = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RiskIntensities", t => t.AfterControlRiskIntensityId)
                .ForeignKey("dbo.RiskProbabilities", t => t.AfterControlRiskProbabilityId)
                .ForeignKey("dbo.RiskProbabilities", t => t.RiskProbabilityId, cascadeDelete: true)
                .ForeignKey("dbo.RiskIntensities", t => t.RiskIntensityId, cascadeDelete: true)
                .ForeignKey("dbo.Risks", t => t.RiskId, cascadeDelete: true)
                .ForeignKey("dbo.RiskStatus", t => t.RiskStatusId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RiskId)
                .Index(t => t.RiskIntensityId)
                .Index(t => t.RiskProbabilityId)
                .Index(t => t.AfterControlRiskIntensityId)
                .Index(t => t.AfterControlRiskProbabilityId)
                .Index(t => t.RiskStatusId);
            
            CreateTable(
                "dbo.RiskIntensities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Level = c.Int(nullable: false),
                        Summery = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RiskCalculators",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RiskIntensityId = c.Guid(nullable: false),
                        RiskProbabilityId = c.Guid(nullable: false),
                        RiskNumber = c.Int(nullable: false),
                        RiskNumberDescription = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RiskIntensities", t => t.RiskIntensityId, cascadeDelete: true)
                .ForeignKey("dbo.RiskProbabilities", t => t.RiskProbabilityId, cascadeDelete: true)
                .Index(t => t.RiskIntensityId)
                .Index(t => t.RiskProbabilityId);
            
            CreateTable(
                "dbo.RiskProbabilities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Level = c.Int(nullable: false),
                        Summery = c.String(),
                        Summery2 = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RiskStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Order = c.Int(nullable: false),
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
            DropForeignKey("dbo.UserRisks", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRisks", "RiskStatusId", "dbo.RiskStatus");
            DropForeignKey("dbo.UserRisks", "RiskId", "dbo.Risks");
            DropForeignKey("dbo.UserRisks", "RiskIntensityId", "dbo.RiskIntensities");
            DropForeignKey("dbo.UserRisks", "RiskProbabilityId", "dbo.RiskProbabilities");
            DropForeignKey("dbo.RiskCalculators", "RiskProbabilityId", "dbo.RiskProbabilities");
            DropForeignKey("dbo.UserRisks", "AfterControlRiskProbabilityId", "dbo.RiskProbabilities");
            DropForeignKey("dbo.RiskCalculators", "RiskIntensityId", "dbo.RiskIntensities");
            DropForeignKey("dbo.UserRisks", "AfterControlRiskIntensityId", "dbo.RiskIntensities");
            DropForeignKey("dbo.Risks", "StageId", "dbo.Stages");
            DropForeignKey("dbo.RiskControlingWorks", "RiskId", "dbo.Risks");
            DropForeignKey("dbo.Stages", "ActId", "dbo.Acts");
            DropForeignKey("dbo.Operations", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Acts", "OperationId", "dbo.Operations");
            DropIndex("dbo.RiskCalculators", new[] { "RiskProbabilityId" });
            DropIndex("dbo.RiskCalculators", new[] { "RiskIntensityId" });
            DropIndex("dbo.UserRisks", new[] { "RiskStatusId" });
            DropIndex("dbo.UserRisks", new[] { "AfterControlRiskProbabilityId" });
            DropIndex("dbo.UserRisks", new[] { "AfterControlRiskIntensityId" });
            DropIndex("dbo.UserRisks", new[] { "RiskProbabilityId" });
            DropIndex("dbo.UserRisks", new[] { "RiskIntensityId" });
            DropIndex("dbo.UserRisks", new[] { "RiskId" });
            DropIndex("dbo.UserRisks", new[] { "UserId" });
            DropIndex("dbo.RiskControlingWorks", new[] { "RiskId" });
            DropIndex("dbo.Risks", new[] { "StageId" });
            DropIndex("dbo.Stages", new[] { "ActId" });
            DropIndex("dbo.Operations", new[] { "ProjectId" });
            DropIndex("dbo.Acts", new[] { "OperationId" });
            DropTable("dbo.RiskStatus");
            DropTable("dbo.RiskProbabilities");
            DropTable("dbo.RiskCalculators");
            DropTable("dbo.RiskIntensities");
            DropTable("dbo.UserRisks");
            DropTable("dbo.RiskControlingWorks");
            DropTable("dbo.Risks");
            DropTable("dbo.Stages");
            DropTable("dbo.Projects");
            DropTable("dbo.Operations");
            DropTable("dbo.Acts");
        }
    }
}
