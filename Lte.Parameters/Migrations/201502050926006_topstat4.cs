namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class topstat4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NeighborHourInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TopDrop2GCellDailyId = c.Int(nullable: false),
                        Hour = c.Short(nullable: false),
                        BscId = c.Short(nullable: false),
                        BtsId = c.Int(nullable: false),
                        CellId = c.Int(nullable: false),
                        SectorId = c.Byte(nullable: false),
                        Frequency = c.Short(nullable: false),
                        NeighborInfo = c.String(maxLength: 20),
                        Problem = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TopDrop2GCellDaily", t => t.TopDrop2GCellDailyId, cascadeDelete: true)
                .Index(t => t.TopDrop2GCellDailyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NeighborHourInfoes", "TopDrop2GCellDailyId", "dbo.TopDrop2GCellDaily");
            DropIndex("dbo.NeighborHourInfoes", new[] { "TopDrop2GCellDailyId" });
            DropTable("dbo.NeighborHourInfoes");
        }
    }
}
