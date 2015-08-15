namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class topstat2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Towns", "CityName", c => c.String(maxLength: 20));
            AlterColumn("dbo.Towns", "DistrictName", c => c.String(maxLength: 20));
            AlterColumn("dbo.Towns", "TownName", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Towns", "TownName", c => c.String());
            AlterColumn("dbo.Towns", "DistrictName", c => c.String());
            AlterColumn("dbo.Towns", "CityName", c => c.String());
        }
    }
}
