namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V20 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Acts", "OldId", c => c.Int());
            AlterColumn("dbo.Operations", "OldId", c => c.Int());
            AlterColumn("dbo.Stages", "OldId", c => c.Int());
            AlterColumn("dbo.Risks", "OldId", c => c.Int());
            AlterColumn("dbo.RiskControlingWorks", "OldId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RiskControlingWorks", "OldId", c => c.Int(nullable: false));
            AlterColumn("dbo.Risks", "OldId", c => c.Int(nullable: false));
            AlterColumn("dbo.Stages", "OldId", c => c.Int(nullable: false));
            AlterColumn("dbo.Operations", "OldId", c => c.Int(nullable: false));
            AlterColumn("dbo.Acts", "OldId", c => c.Int(nullable: false));
        }
    }
}
