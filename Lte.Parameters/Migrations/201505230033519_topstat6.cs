namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class topstat6 : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.MonthPreciseCoverage4GStat",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Year = c.Short(nullable: false),
            //            Month = c.Byte(nullable: false),
            //            CellId = c.Int(nullable: false),
            //            SectorId = c.Byte(nullable: false),
            //            TotalMrs = c.Int(nullable: false),
            //            ThirdNeighbors = c.Int(nullable: false),
            //            SecondNeighbors = c.Int(nullable: false),
            //            FirstNeighbors = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.PreciseCoverage4G",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            StatTime = c.DateTime(nullable: false),
            //            CellId = c.Int(nullable: false),
            //            SectorId = c.Byte(nullable: false),
            //            TotalMrs = c.Int(nullable: false),
            //            ThirdNeighbors = c.Int(nullable: false),
            //            SecondNeighbors = c.Int(nullable: false),
            //            FirstNeighbors = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.TownPreciseCoverage4GStat",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            StatTime = c.DateTime(nullable: false),
            //            TownId = c.Int(nullable: false),
            //            TotalMrs = c.Int(nullable: false),
            //            ThirdNeighbors = c.Int(nullable: false),
            //            SecondNeighbors = c.Int(nullable: false),
            //            FirstNeighbors = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.TownPreciseCoverage4GStat");
            //DropTable("dbo.PreciseCoverage4G");
            //DropTable("dbo.MonthPreciseCoverage4GStat");
        }
    }
}
