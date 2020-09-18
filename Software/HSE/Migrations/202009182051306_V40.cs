namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V40 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PermitStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Code = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Permits", "PermitStatusId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Permits", "PermitStatusId");
            AddForeignKey("dbo.Permits", "PermitStatusId", "dbo.PermitStatus", "Id", cascadeDelete: true);
            DropColumn("dbo.Permits", "IsAcceptBySupervisor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Permits", "IsAcceptBySupervisor", c => c.Boolean());
            DropForeignKey("dbo.Permits", "PermitStatusId", "dbo.PermitStatus");
            DropIndex("dbo.Permits", new[] { "PermitStatusId" });
            DropColumn("dbo.Permits", "PermitStatusId");
            DropTable("dbo.PermitStatus");
        }
    }
}
