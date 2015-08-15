namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cell : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Cells", "AntennaPortsValue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cells", "AntennaPortsValue", c => c.Int(nullable: false));
        }
    }
}
