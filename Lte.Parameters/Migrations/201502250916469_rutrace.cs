namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rutrace : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InterferenceStats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CellId = c.Int(nullable: false),
                        SectorId = c.Byte(nullable: false),
                        VictimCells = c.Int(nullable: false),
                        InterferenceCells = c.Int(nullable: false),
                        SumRtds = c.Double(nullable: false),
                        TotalRtds = c.Int(nullable: false),
                        MinRtd = c.Double(nullable: false),
                        TaOuterIntervalNum = c.Int(nullable: false),
                        TaInnerIntervalNum = c.Int(nullable: false),
                        TaSum = c.Double(nullable: false),
                        TaOuterIntervalExcessNum = c.Int(nullable: false),
                        TaInnerIntervalExcessNum = c.Int(nullable: false),
                        TaMax = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.InterferenceStats");
        }
    }
}
