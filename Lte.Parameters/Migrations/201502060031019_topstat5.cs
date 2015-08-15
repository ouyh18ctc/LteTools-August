namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class topstat5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TopDrop2GCellDaily", "StatTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.TopDrop2GCellDaily", "StatDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TopDrop2GCellDaily", "StatDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.TopDrop2GCellDaily", "StatTime");
        }
    }
}
