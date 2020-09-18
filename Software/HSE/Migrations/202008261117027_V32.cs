namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V32 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accidents", "WasteDays", c => c.String());
            AddColumn("dbo.Accidents", "CompanyUser", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accidents", "CompanyUser");
            DropColumn("dbo.Accidents", "WasteDays");
        }
    }
}
