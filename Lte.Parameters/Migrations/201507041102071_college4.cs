namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class college4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IndoorDistributions", "CollegeName", c => c.String());
            AddColumn("dbo.IndoorDistributions", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.IndoorDistributions", "Discriminator");
            DropColumn("dbo.IndoorDistributions", "CollegeName");
        }
    }
}
