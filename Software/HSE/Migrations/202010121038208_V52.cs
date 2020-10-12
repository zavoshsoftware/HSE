namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V52 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Relations", "FileUrl", c => c.String());
            DropColumn("dbo.Relations", "Body");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Relations", "Body", c => c.String(storeType: "ntext"));
            DropColumn("dbo.Relations", "FileUrl");
        }
    }
}
