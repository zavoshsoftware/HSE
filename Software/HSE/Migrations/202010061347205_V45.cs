namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V45 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "ChartFileUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "ChartFileUrl");
        }
    }
}
