namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V37 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Equipments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        CertificateFileUrl = c.String(),
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
            DropForeignKey("dbo.Equipments", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Equipments", new[] { "CompanyId" });
            DropTable("dbo.Equipments");
        }
    }
}
