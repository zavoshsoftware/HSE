namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V10 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Anomalies", "EffectivnessDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Anomalies", "EffectivnessDate", c => c.DateTime(nullable: false));
        }
    }
}
