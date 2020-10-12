namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V49 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyTypes",
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
            
            AddColumn("dbo.Companies", "CompanyTypeId", c => c.Guid());
            CreateIndex("dbo.Companies", "CompanyTypeId");
            AddForeignKey("dbo.Companies", "CompanyTypeId", "dbo.CompanyTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Companies", "CompanyTypeId", "dbo.CompanyTypes");
            DropIndex("dbo.Companies", new[] { "CompanyTypeId" });
            DropColumn("dbo.Companies", "CompanyTypeId");
            DropTable("dbo.CompanyTypes");
        }
    }
}
