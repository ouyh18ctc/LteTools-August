namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class college6 : DbMigration
    {
        public override void Up()
        {
            //DropPrimaryKey("dbo.CollegeRegions");
            AddColumn("dbo.CollegeInfoes", "CollegeRegion_AreaId", c => c.Int());
            AddColumn("dbo.CollegeRegions", "AreaId", c => c.Int(nullable: false, identity: true));
            //AlterColumn("dbo.CollegeInfoes", "Name", c => c.String());
            //AlterColumn("dbo.CollegeRegions", "RegionType", c => c.Byte(nullable: false));
            //AlterColumn("dbo.InfrastructureInfoes", "HotspotType", c => c.Byte(nullable: false));
            //AlterColumn("dbo.InfrastructureInfoes", "InfrastructureType", c => c.Byte(nullable: false));
            AddPrimaryKey("dbo.CollegeRegions", "AreaId");
            CreateIndex("dbo.CollegeInfoes", "CollegeRegion_AreaId");
            AddForeignKey("dbo.CollegeInfoes", "CollegeRegion_AreaId", "dbo.CollegeRegions", "AreaId");
            //DropColumn("dbo.CollegeInfoes", "CityName");
            //DropColumn("dbo.CollegeInfoes", "DistrictName");
            //DropColumn("dbo.CollegeInfoes", "TownName");
            //DropColumn("dbo.CollegeInfoes", "Discriminator");
            //DropColumn("dbo.CollegeRegions", "Id");
            //DropColumn("dbo.CollegeRegions", "CollegeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CollegeRegions", "CollegeId", c => c.Int(nullable: false));
            AddColumn("dbo.CollegeRegions", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.CollegeInfoes", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.CollegeInfoes", "TownName", c => c.String());
            AddColumn("dbo.CollegeInfoes", "DistrictName", c => c.String());
            AddColumn("dbo.CollegeInfoes", "CityName", c => c.String());
            DropForeignKey("dbo.CollegeInfoes", "CollegeRegion_AreaId", "dbo.CollegeRegions");
            DropIndex("dbo.CollegeInfoes", new[] { "CollegeRegion_AreaId" });
            DropPrimaryKey("dbo.CollegeRegions");
            AlterColumn("dbo.InfrastructureInfoes", "InfrastructureType", c => c.Int(nullable: false));
            AlterColumn("dbo.InfrastructureInfoes", "HotspotType", c => c.Int(nullable: false));
            AlterColumn("dbo.CollegeRegions", "RegionType", c => c.Int(nullable: false));
            AlterColumn("dbo.CollegeInfoes", "Name", c => c.String(nullable: false));
            DropColumn("dbo.CollegeRegions", "AreaId");
            DropColumn("dbo.CollegeInfoes", "CollegeRegion_AreaId");
            AddPrimaryKey("dbo.CollegeRegions", "Id");
        }
    }
}
