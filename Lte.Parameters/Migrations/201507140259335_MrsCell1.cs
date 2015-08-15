namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MrsCell1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MrsCellDates", "RsrpTo120", c => c.Int(nullable: false));
            AddColumn("dbo.MrsCellDates", "RsrpTo115", c => c.Int(nullable: false));
            AddColumn("dbo.MrsCellDates", "RsrpTo110", c => c.Int(nullable: false));
            AddColumn("dbo.MrsCellDates", "RsrpTo105", c => c.Int(nullable: false));
            AddColumn("dbo.MrsCellDates", "RsrpTo100", c => c.Int(nullable: false));
            AddColumn("dbo.MrsCellDates", "RsrpTo95", c => c.Int(nullable: false));
            AddColumn("dbo.MrsCellDates", "RsrpTo90", c => c.Int(nullable: false));
            AddColumn("dbo.MrsCellDates", "RsrpTo85", c => c.Int(nullable: false));
            AddColumn("dbo.MrsCellDates", "RsrpTo80", c => c.Int(nullable: false));
            AddColumn("dbo.MrsCellDates", "RsrpTo70", c => c.Int(nullable: false));
            AddColumn("dbo.MrsCellDates", "RsrpTo60", c => c.Int(nullable: false));
            AddColumn("dbo.MrsCellDates", "RsrpAbove60", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MrsCellDates", "RsrpAbove60");
            DropColumn("dbo.MrsCellDates", "RsrpTo60");
            DropColumn("dbo.MrsCellDates", "RsrpTo70");
            DropColumn("dbo.MrsCellDates", "RsrpTo80");
            DropColumn("dbo.MrsCellDates", "RsrpTo85");
            DropColumn("dbo.MrsCellDates", "RsrpTo90");
            DropColumn("dbo.MrsCellDates", "RsrpTo95");
            DropColumn("dbo.MrsCellDates", "RsrpTo100");
            DropColumn("dbo.MrsCellDates", "RsrpTo105");
            DropColumn("dbo.MrsCellDates", "RsrpTo110");
            DropColumn("dbo.MrsCellDates", "RsrpTo115");
            DropColumn("dbo.MrsCellDates", "RsrpTo120");
        }
    }
}
