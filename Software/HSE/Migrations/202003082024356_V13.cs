namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V13 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Requirments", "Weight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.RequirmentTypes", "Weight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RequirmentTypes", "Weight", c => c.Int(nullable: false));
            AlterColumn("dbo.Requirments", "Weight", c => c.Int(nullable: false));
        }
    }
}
