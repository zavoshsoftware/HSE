namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V44 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.HsePlans", newName: "HseDocuments");
            CreateTable(
                "dbo.HseDocumentTypes",
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
            
            AddColumn("dbo.HseDocuments", "HseDocumentTypeId", c => c.Guid(nullable: false));
            CreateIndex("dbo.HseDocuments", "HseDocumentTypeId");
            AddForeignKey("dbo.HseDocuments", "HseDocumentTypeId", "dbo.HseDocumentTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HseDocuments", "HseDocumentTypeId", "dbo.HseDocumentTypes");
            DropIndex("dbo.HseDocuments", new[] { "HseDocumentTypeId" });
            DropColumn("dbo.HseDocuments", "HseDocumentTypeId");
            DropTable("dbo.HseDocumentTypes");
            RenameTable(name: "dbo.HseDocuments", newName: "HsePlans");
        }
    }
}
