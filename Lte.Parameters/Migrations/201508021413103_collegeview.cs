namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class collegeview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CollegeInfoes", "LastModificationTime", c => c.DateTime());
            AddColumn("dbo.CollegeInfoes", "LastModifierUserId", c => c.Long());
            AddColumn("dbo.CollegeInfoes", "CreationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.CollegeInfoes", "CreatorUserId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CollegeInfoes", "CreatorUserId");
            DropColumn("dbo.CollegeInfoes", "CreationTime");
            DropColumn("dbo.CollegeInfoes", "LastModifierUserId");
            DropColumn("dbo.CollegeInfoes", "LastModificationTime");
        }
    }
}
