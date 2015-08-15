namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class college1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CollegeInfoes", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CollegeInfoes", "Name", c => c.String());
        }
    }
}
