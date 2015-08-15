namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class topstat3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CdmaBts", "Name", c => c.String(maxLength: 50));
            AlterColumn("dbo.ENodebs", "Name", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ENodebs", "Name", c => c.String());
            AlterColumn("dbo.CdmaBts", "Name", c => c.String());
        }
    }
}
