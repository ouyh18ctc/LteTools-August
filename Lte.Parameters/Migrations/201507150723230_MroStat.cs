namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MroStat : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MroRsrpTas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RecordDate = c.DateTime(nullable: false),
                        CellId = c.Int(nullable: false),
                        SectorId = c.Byte(nullable: false),
                        RsrpInterval = c.Int(nullable: false),
                        TaTo4 = c.Int(nullable: false),
                        TaTo8 = c.Int(nullable: false),
                        TaTo16 = c.Int(nullable: false),
                        TaTo24 = c.Int(nullable: false),
                        TaTo40 = c.Int(nullable: false),
                        TaTo56 = c.Int(nullable: false),
                        TaTo80 = c.Int(nullable: false),
                        TaTo128 = c.Int(nullable: false),
                        TaAbove128 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MroRsrpTas");
        }
    }
}
