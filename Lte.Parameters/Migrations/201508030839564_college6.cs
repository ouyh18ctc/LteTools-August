namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class college6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CollegeInfoes", "Name", c => c.String());
            AlterColumn("dbo.CollegeRegions", "RegionType", c => c.Byte(nullable: false));
            AlterColumn("dbo.InfrastructureInfoes", "InfrastructureType", c => c.Byte(nullable: false));
            DropColumn("dbo.CollegeInfoes", "CityName");
            DropColumn("dbo.CollegeInfoes", "DistrictName");
            DropColumn("dbo.CollegeInfoes", "TownName");
            DropColumn("dbo.CollegeInfoes", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CollegeInfoes", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.CollegeInfoes", "TownName", c => c.String());
            AddColumn("dbo.CollegeInfoes", "DistrictName", c => c.String());
            AddColumn("dbo.CollegeInfoes", "CityName", c => c.String());
            AlterColumn("dbo.InfrastructureInfoes", "InfrastructureType", c => c.Int(nullable: false));
            AlterColumn("dbo.CollegeRegions", "RegionType", c => c.Int(nullable: false));
            AlterColumn("dbo.CollegeInfoes", "Name", c => c.String(nullable: false));
        }
    }
}
