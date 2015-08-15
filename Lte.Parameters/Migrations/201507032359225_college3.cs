namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class college3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IndoorDistributions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Range = c.String(),
                        SourceName = c.String(),
                        SourceType = c.String(),
                        Longtitute = c.Double(nullable: false),
                        Lattitute = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.IndoorDistributions");
        }
    }
}
