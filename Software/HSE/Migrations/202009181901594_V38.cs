namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V38 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EquipmentTypes",
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
                "dbo.Documents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        DocumentTypeId = c.Guid(nullable: false),
                        FileUrl = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocumentTypes", t => t.DocumentTypeId, cascadeDelete: true)
                .Index(t => t.DocumentTypeId);
            
            CreateTable(
                "dbo.DocumentTypes",
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
            
            AddColumn("dbo.Equipments", "EquipmentTypeId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Equipments", "EquipmentTypeId");
            AddForeignKey("dbo.Equipments", "EquipmentTypeId", "dbo.EquipmentTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "DocumentTypeId", "dbo.DocumentTypes");
            DropForeignKey("dbo.Equipments", "EquipmentTypeId", "dbo.EquipmentTypes");
            DropIndex("dbo.Documents", new[] { "DocumentTypeId" });
            DropIndex("dbo.Equipments", new[] { "EquipmentTypeId" });
            DropColumn("dbo.Equipments", "EquipmentTypeId");
            DropTable("dbo.DocumentTypes");
            DropTable("dbo.Documents");
            DropTable("dbo.EquipmentTypes");
        }
    }
}
