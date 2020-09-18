namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V171 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempTable_Act",
                c => new
                    {
                        ActID = c.Int(nullable: false, identity: true),
                        OperationID = c.Int(nullable: false),
                        CodeID = c.Int(nullable: false),
                        ActTitle = c.String(),
                        IsAcceptedByAdmin = c.Boolean(nullable: false),
                        ProtectionEQP = c.String(),
                        Curses = c.String(),
                    })
                .PrimaryKey(t => t.ActID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TempTable_Act");
        }
    }
}
