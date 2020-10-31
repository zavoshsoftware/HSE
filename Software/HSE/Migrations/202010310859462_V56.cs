namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V56 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnomalyAttachments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ImageUrl = c.String(),
                        AnomalyId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Anomalies", t => t.AnomalyId, cascadeDelete: true)
                .Index(t => t.AnomalyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnomalyAttachments", "AnomalyId", "dbo.Anomalies");
            DropIndex("dbo.AnomalyAttachments", new[] { "AnomalyId" });
            DropTable("dbo.AnomalyAttachments");
        }
    }
}
