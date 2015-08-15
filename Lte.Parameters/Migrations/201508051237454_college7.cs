namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class college7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CollegeRegions", "RegionType", c => c.Byte(nullable: false));
            AlterColumn("dbo.InfrastructureInfoes", "HotspotType", c => c.Byte(nullable: false));
            AlterColumn("dbo.InfrastructureInfoes", "InfrastructureType", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InfrastructureInfoes", "InfrastructureType", c => c.Int(nullable: false));
            AlterColumn("dbo.InfrastructureInfoes", "HotspotType", c => c.Int(nullable: false));
            AlterColumn("dbo.CollegeRegions", "RegionType", c => c.Int(nullable: false));
        }
    }
}
