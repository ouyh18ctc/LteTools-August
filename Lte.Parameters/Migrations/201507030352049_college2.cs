namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class college2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InfrastructureInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HotspotType = c.Int(nullable: false),
                        HotspotName = c.String(),
                        InfrastructureType = c.Int(nullable: false),
                        InfrastructureId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.InfrastructureInfoes");
        }
    }
}
