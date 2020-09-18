namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V28 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Accidents", "AccidentDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Accidents", "AccidentDate", c => c.DateTime(nullable: false));
        }
    }
}
