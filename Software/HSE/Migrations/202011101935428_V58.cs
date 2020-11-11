namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V58 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RelationImages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RelationId = c.Guid(nullable: false),
                        ImageUrl = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Relations", t => t.RelationId, cascadeDelete: true)
                .Index(t => t.RelationId);
            
            AddColumn("dbo.Relations", "TeacherName", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RelationImages", "RelationId", "dbo.Relations");
            DropIndex("dbo.RelationImages", new[] { "RelationId" });
            DropColumn("dbo.Relations", "TeacherName");
            DropTable("dbo.RelationImages");
        }
    }
}
