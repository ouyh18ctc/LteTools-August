namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class college : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CollegeInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TownId = c.Int(nullable: false),
                        Name = c.String(),
                        TotalStudents = c.Int(nullable: false),
                        CurrentSubscribers = c.Int(nullable: false),
                        GraduateStudents = c.Int(nullable: false),
                        NewSubscribers = c.Int(nullable: false),
                        OldOpenDate = c.DateTime(nullable: false),
                        NewOpenDate = c.DateTime(nullable: false),
                        CityName = c.String(),
                        DistrictName = c.String(),
                        TownName = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CollegeInfoes");
        }
    }
}
