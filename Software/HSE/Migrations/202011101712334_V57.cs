namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V57 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Equipments", "FinishDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Equipments", "FinishDate");
        }
    }
}
