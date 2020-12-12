namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _65 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Progresses", "SupPercent", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Progresses", "AdminPercent", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Progresses", "AdminPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Progresses", "SupPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
