namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V09 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Anomalies", "Reporter");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Anomalies", "Reporter", c => c.String());
        }
    }
}
