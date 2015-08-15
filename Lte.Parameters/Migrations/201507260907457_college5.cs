namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class college5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CollegeRegions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CollegeId = c.Int(nullable: false),
                        Area = c.Double(nullable: false),
                        RegionType = c.Int(nullable: false),
                        Info = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CollegeRegions");
        }
    }
}
