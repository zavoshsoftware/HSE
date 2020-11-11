namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V60 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnomalyAttachments", "IsResultAttachment", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AnomalyAttachments", "IsResultAttachment");
        }
    }
}
