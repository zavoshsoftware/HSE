namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V26 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccidentEmployeeTypes",
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
                "dbo.AccidentInjuries",
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
                "dbo.AccidentInjuryRelAccidents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccidentId = c.Guid(nullable: false),
                        AccidentInjuryId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accidents", t => t.AccidentId, cascadeDelete: true)
                .ForeignKey("dbo.AccidentInjuries", t => t.AccidentInjuryId, cascadeDelete: true)
                .Index(t => t.AccidentId)
                .Index(t => t.AccidentInjuryId);
            
            CreateTable(
                "dbo.Accidents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        PersonalNumber = c.String(),
                        Unit = c.String(),
                        Education = c.String(),
                        Age = c.Int(nullable: false),
                        Experience = c.String(),
                        AccidentDate = c.DateTime(nullable: false),
                        AccidentTime = c.String(),
                        IsMaried = c.Boolean(nullable: false),
                        WeekDay = c.String(),
                        Place = c.String(),
                        AccidentEmployeeTypeId = c.Guid(nullable: false),
                        Phone = c.String(),
                        Company = c.String(),
                        Job = c.String(),
                        ManageName = c.String(),
                        HospitalTime = c.String(),
                        HospitalName = c.String(),
                        NationalCode = c.String(),
                        CellNumber = c.String(),
                        Address = c.String(),
                        AccidentDesc = c.String(),
                        IsAcceptable = c.Boolean(nullable: false),
                        UserFullName = c.String(),
                        AccidentReasonConditionOther = c.String(),
                        AccidentReasonActionOther = c.String(),
                        AccidentPartOther = c.String(),
                        AccidentInjuryOther = c.String(),
                        AccidentTypeOther = c.String(),
                        AccidentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AccidentComplication = c.String(),
                        AccidentInitialComplication = c.String(),
                        Actions = c.String(),
                        ReapeatAction = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccidentEmployeeTypes", t => t.AccidentEmployeeTypeId, cascadeDelete: true)
                .Index(t => t.AccidentEmployeeTypeId);
            
            CreateTable(
                "dbo.AccidentPartRelAccidents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccidentId = c.Guid(nullable: false),
                        AccidentPartId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accidents", t => t.AccidentId, cascadeDelete: true)
                .ForeignKey("dbo.AccidentParts", t => t.AccidentPartId, cascadeDelete: true)
                .Index(t => t.AccidentId)
                .Index(t => t.AccidentPartId);
            
            CreateTable(
                "dbo.AccidentParts",
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
                "dbo.AccidentReasonActionRelAccidents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccidentId = c.Guid(nullable: false),
                        AccidentReasonActionId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accidents", t => t.AccidentId, cascadeDelete: true)
                .ForeignKey("dbo.AccidentReasonActions", t => t.AccidentReasonActionId, cascadeDelete: true)
                .Index(t => t.AccidentId)
                .Index(t => t.AccidentReasonActionId);
            
            CreateTable(
                "dbo.AccidentReasonActions",
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
                "dbo.AccidentReasonConditionRelAccidents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccidentId = c.Guid(nullable: false),
                        AccidentReasonConditionId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accidents", t => t.AccidentId, cascadeDelete: true)
                .ForeignKey("dbo.AccidentReasonConditions", t => t.AccidentReasonConditionId, cascadeDelete: true)
                .Index(t => t.AccidentId)
                .Index(t => t.AccidentReasonConditionId);
            
            CreateTable(
                "dbo.AccidentReasonConditions",
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
                "dbo.AccidentResultRelAccidents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccidentId = c.Guid(nullable: false),
                        AccidentResultId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accidents", t => t.AccidentId, cascadeDelete: true)
                .ForeignKey("dbo.AccidentResults", t => t.AccidentResultId, cascadeDelete: true)
                .Index(t => t.AccidentId)
                .Index(t => t.AccidentResultId);
            
            CreateTable(
                "dbo.AccidentResults",
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
                "dbo.AccidentTypeRelAccidents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccidentId = c.Guid(nullable: false),
                        AccidentTypeId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accidents", t => t.AccidentId, cascadeDelete: true)
                .ForeignKey("dbo.AccidentTypes", t => t.AccidentTypeId, cascadeDelete: true)
                .Index(t => t.AccidentId)
                .Index(t => t.AccidentTypeId);
            
            CreateTable(
                "dbo.AccidentTypes",
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
            DropForeignKey("dbo.AccidentInjuryRelAccidents", "AccidentInjuryId", "dbo.AccidentInjuries");
            DropForeignKey("dbo.AccidentTypeRelAccidents", "AccidentTypeId", "dbo.AccidentTypes");
            DropForeignKey("dbo.AccidentTypeRelAccidents", "AccidentId", "dbo.Accidents");
            DropForeignKey("dbo.AccidentResultRelAccidents", "AccidentResultId", "dbo.AccidentResults");
            DropForeignKey("dbo.AccidentResultRelAccidents", "AccidentId", "dbo.Accidents");
            DropForeignKey("dbo.AccidentReasonConditionRelAccidents", "AccidentReasonConditionId", "dbo.AccidentReasonConditions");
            DropForeignKey("dbo.AccidentReasonConditionRelAccidents", "AccidentId", "dbo.Accidents");
            DropForeignKey("dbo.AccidentReasonActionRelAccidents", "AccidentReasonActionId", "dbo.AccidentReasonActions");
            DropForeignKey("dbo.AccidentReasonActionRelAccidents", "AccidentId", "dbo.Accidents");
            DropForeignKey("dbo.AccidentPartRelAccidents", "AccidentPartId", "dbo.AccidentParts");
            DropForeignKey("dbo.AccidentPartRelAccidents", "AccidentId", "dbo.Accidents");
            DropForeignKey("dbo.AccidentInjuryRelAccidents", "AccidentId", "dbo.Accidents");
            DropForeignKey("dbo.Accidents", "AccidentEmployeeTypeId", "dbo.AccidentEmployeeTypes");
            DropIndex("dbo.AccidentTypeRelAccidents", new[] { "AccidentTypeId" });
            DropIndex("dbo.AccidentTypeRelAccidents", new[] { "AccidentId" });
            DropIndex("dbo.AccidentResultRelAccidents", new[] { "AccidentResultId" });
            DropIndex("dbo.AccidentResultRelAccidents", new[] { "AccidentId" });
            DropIndex("dbo.AccidentReasonConditionRelAccidents", new[] { "AccidentReasonConditionId" });
            DropIndex("dbo.AccidentReasonConditionRelAccidents", new[] { "AccidentId" });
            DropIndex("dbo.AccidentReasonActionRelAccidents", new[] { "AccidentReasonActionId" });
            DropIndex("dbo.AccidentReasonActionRelAccidents", new[] { "AccidentId" });
            DropIndex("dbo.AccidentPartRelAccidents", new[] { "AccidentPartId" });
            DropIndex("dbo.AccidentPartRelAccidents", new[] { "AccidentId" });
            DropIndex("dbo.Accidents", new[] { "AccidentEmployeeTypeId" });
            DropIndex("dbo.AccidentInjuryRelAccidents", new[] { "AccidentInjuryId" });
            DropIndex("dbo.AccidentInjuryRelAccidents", new[] { "AccidentId" });
            DropTable("dbo.AccidentTypes");
            DropTable("dbo.AccidentTypeRelAccidents");
            DropTable("dbo.AccidentResults");
            DropTable("dbo.AccidentResultRelAccidents");
            DropTable("dbo.AccidentReasonConditions");
            DropTable("dbo.AccidentReasonConditionRelAccidents");
            DropTable("dbo.AccidentReasonActions");
            DropTable("dbo.AccidentReasonActionRelAccidents");
            DropTable("dbo.AccidentParts");
            DropTable("dbo.AccidentPartRelAccidents");
            DropTable("dbo.Accidents");
            DropTable("dbo.AccidentInjuryRelAccidents");
            DropTable("dbo.AccidentInjuries");
            DropTable("dbo.AccidentEmployeeTypes");
        }
    }
}
