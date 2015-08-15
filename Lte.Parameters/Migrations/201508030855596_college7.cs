namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class college7 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.CollegeRegions");
            AddColumn("dbo.CollegeInfoes", "CollegeRegion_CollegeId", c => c.Int());
            AlterColumn("dbo.CollegeRegions", "CollegeId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.CollegeRegions", "CollegeId");
            CreateIndex("dbo.CollegeInfoes", "CollegeRegion_CollegeId");
            AddForeignKey("dbo.CollegeInfoes", "CollegeRegion_CollegeId", "dbo.CollegeRegions", "CollegeId");
            DropColumn("dbo.CollegeRegions", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CollegeRegions", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.CollegeInfoes", "CollegeRegion_CollegeId", "dbo.CollegeRegions");
            DropIndex("dbo.CollegeInfoes", new[] { "CollegeRegion_CollegeId" });
            DropPrimaryKey("dbo.CollegeRegions");
            AlterColumn("dbo.CollegeRegions", "CollegeId", c => c.Int(nullable: false));
            DropColumn("dbo.CollegeInfoes", "CollegeRegion_CollegeId");
            AddPrimaryKey("dbo.CollegeRegions", "Id");
        }
    }
}
