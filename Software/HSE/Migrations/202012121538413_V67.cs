namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V67 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Covids", "SickDate", c => c.DateTime());
            AlterColumn("dbo.Covids", "SafeDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Covids", "SafeDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Covids", "SickDate", c => c.DateTime(nullable: false));
        }
    }
}
