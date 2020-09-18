namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V29 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Accidents", "AccidentDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Accidents", "AccidentDate", c => c.DateTime());
        }
    }
}
