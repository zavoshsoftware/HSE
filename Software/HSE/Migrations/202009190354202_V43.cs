namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V43 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Relations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RelationTypeId = c.Guid(nullable: false),
                        Title = c.String(),
                        Body = c.String(storeType: "ntext"),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RelationTypes", t => t.RelationTypeId, cascadeDelete: true)
                .Index(t => t.RelationTypeId);
            
            CreateTable(
                "dbo.RelationTypes",
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Relations", "RelationTypeId", "dbo.RelationTypes");
            DropIndex("dbo.Relations", new[] { "RelationTypeId" });
            DropTable("dbo.RelationTypes");
            DropTable("dbo.Relations");
        }
    }
}
