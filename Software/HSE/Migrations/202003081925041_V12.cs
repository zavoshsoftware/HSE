namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContractRquirments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProjectTitle = c.String(),
                        Code = c.String(),
                        ContractDate = c.DateTime(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        CompanyId = c.Guid(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
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
            
            CreateTable(
                "dbo.RequirmentDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProgressPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProgressAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalProgressPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalProgressAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ContractRquirmenttId = c.Guid(nullable: false),
                        RequirmentId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                        ContractRquirment_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContractRquirments", t => t.ContractRquirment_Id)
                .ForeignKey("dbo.Requirments", t => t.RequirmentId, cascadeDelete: true)
                .Index(t => t.RequirmentId)
                .Index(t => t.ContractRquirment_Id);
            
            CreateTable(
                "dbo.Requirments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Order = c.Int(nullable: false),
                        Title = c.String(),
                        Weight = c.Int(nullable: false),
                        RequirmentTypeId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RequirmentTypes", t => t.RequirmentTypeId, cascadeDelete: true)
                .Index(t => t.RequirmentTypeId);
            
            CreateTable(
                "dbo.RequirmentTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Order = c.Int(nullable: false),
                        Title = c.String(),
                        Weight = c.Int(nullable: false),
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
            DropForeignKey("dbo.Requirments", "RequirmentTypeId", "dbo.RequirmentTypes");
            DropForeignKey("dbo.RequirmentDetails", "RequirmentId", "dbo.Requirments");
            DropForeignKey("dbo.RequirmentDetails", "ContractRquirment_Id", "dbo.ContractRquirments");
            DropForeignKey("dbo.ContractRquirments", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Requirments", new[] { "RequirmentTypeId" });
            DropIndex("dbo.RequirmentDetails", new[] { "ContractRquirment_Id" });
            DropIndex("dbo.RequirmentDetails", new[] { "RequirmentId" });
            DropIndex("dbo.ContractRquirments", new[] { "CompanyId" });
            DropTable("dbo.RequirmentTypes");
            DropTable("dbo.Requirments");
            DropTable("dbo.RequirmentDetails");
            DropTable("dbo.ContractRquirments");
        }
    }
}
