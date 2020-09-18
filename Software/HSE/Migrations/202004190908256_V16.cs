namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V16 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempTables",
                c => new
                    {
                        OperationID = c.Int(nullable: false, identity: true),
                        OperationGroupID = c.Int(nullable: false),
                        CodeID = c.Int(nullable: false),
                        OperationTitle = c.String(),
                        IsAcceptedByAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OperationID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TempTables");
        }
    }
}
