﻿namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V46 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Equipments", "SupervisorComment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Equipments", "SupervisorComment");
        }
    }
}
