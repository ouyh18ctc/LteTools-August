namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewCellWithOpenDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ENodebs", "OpenDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ENodebs", "OpenDate");
        }
    }
}
