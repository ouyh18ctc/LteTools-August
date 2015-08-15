namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MrsCell2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MrsCellTas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RecordDate = c.DateTime(nullable: false),
                        CellId = c.Int(nullable: false),
                        SectorId = c.Byte(nullable: false),
                        TaTo2 = c.Int(nullable: false),
                        TaTo4 = c.Int(nullable: false),
                        TaTo6 = c.Int(nullable: false),
                        TaTo8 = c.Int(nullable: false),
                        TaTo12 = c.Int(nullable: false),
                        TaTo16 = c.Int(nullable: false),
                        TaTo20 = c.Int(nullable: false),
                        TaTo24 = c.Int(nullable: false),
                        TaTo32 = c.Int(nullable: false),
                        TaTo40 = c.Int(nullable: false),
                        TaTo48 = c.Int(nullable: false),
                        TaTo56 = c.Int(nullable: false),
                        TaTo64 = c.Int(nullable: false),
                        TaTo80 = c.Int(nullable: false),
                        TaTo96 = c.Int(nullable: false),
                        TaTo128 = c.Int(nullable: false),
                        TaTo192 = c.Int(nullable: false),
                        TaTo256 = c.Int(nullable: false),
                        TaAbove256 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MrsCellTas");
        }
    }
}
