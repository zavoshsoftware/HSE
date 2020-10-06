namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V48 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SafetyFileTypes", "Code", c => c.Int(nullable: false));
            AlterColumn("dbo.Safeties", "IsAccepteBySupervisor", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Safeties", "IsAccepteBySupervisor", c => c.Boolean(nullable: false));
            DropColumn("dbo.SafetyFileTypes", "Code");
        }
    }
}
