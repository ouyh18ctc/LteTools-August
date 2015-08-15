namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class topstat1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TopDrop2GCellDaily", "Frequency", c => c.Short(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TopDrop2GCellDaily", "Frequency", c => c.Int(nullable: false));
        }
    }
}
