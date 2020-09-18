namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V17 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Acts", "OldId", c => c.Int(nullable: false));
            AddColumn("dbo.Operations", "OldId", c => c.Int(nullable: false));
            AddColumn("dbo.Stages", "OldId", c => c.Int(nullable: false));
            AddColumn("dbo.Risks", "OldId", c => c.Int(nullable: false));
            AddColumn("dbo.RiskControlingWorks", "OldId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RiskControlingWorks", "OldId");
            DropColumn("dbo.Risks", "OldId");
            DropColumn("dbo.Stages", "OldId");
            DropColumn("dbo.Operations", "OldId");
            DropColumn("dbo.Acts", "OldId");
        }
    }
}
