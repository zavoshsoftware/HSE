namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V21 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RiskMatris",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RiskNumber = c.Int(nullable: false),
                        RiskProbabilityId = c.Guid(nullable: false),
                        RiskIntensityId = c.Guid(nullable: false),
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
                .Index(t => t.RiskProbabilityId)
                .Index(t => t.RiskIntensityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RiskMatris", "RiskProbabilityId", "dbo.RiskProbabilities");
            DropForeignKey("dbo.RiskMatris", "RiskIntensityId", "dbo.RiskIntensities");
            DropIndex("dbo.RiskMatris", new[] { "RiskIntensityId" });
            DropIndex("dbo.RiskMatris", new[] { "RiskProbabilityId" });
            DropTable("dbo.RiskMatris");
        }
    }
}
