namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class interference : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PureInterferenceStats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RecordDate = c.DateTime(nullable: false),
                        CellId = c.Int(nullable: false),
                        SectorId = c.Byte(nullable: false),
                        VictimCells = c.Int(nullable: false),
                        InterferenceCells = c.Int(nullable: false),
                        FirstVictimCellId = c.Int(nullable: false),
                        FirstVictimSectorId = c.Byte(nullable: false),
                        FirstVictimTimes = c.Int(nullable: false),
                        FirstInterferenceTimes = c.Int(nullable: false),
                        SecondVictimCellId = c.Int(nullable: false),
                        SecondVictimSectorId = c.Byte(nullable: false),
                        SecondVictimTimes = c.Int(nullable: false),
                        SecondInterferenceTimes = c.Int(nullable: false),
                        ThirdVictimCellId = c.Int(nullable: false),
                        ThirdVictimSectorId = c.Byte(nullable: false),
                        ThirdVictimTimes = c.Int(nullable: false),
                        ThirdInterferenceTimes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PureInterferenceStats");
        }
    }
}
