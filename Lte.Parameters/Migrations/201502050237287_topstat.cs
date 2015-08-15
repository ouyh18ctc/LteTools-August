namespace Lte.Parameters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class topstat : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlarmHourInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TopDrop2GCellDailyId = c.Int(nullable: false),
                        Hour = c.Short(nullable: false),
                        AlarmType = c.Short(nullable: false),
                        Alarms = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TopDrop2GCellDaily", t => t.TopDrop2GCellDailyId, cascadeDelete: true)
                .Index(t => t.TopDrop2GCellDailyId);
            
            CreateTable(
                "dbo.TopDrop2GCellDaily",
                c => new
                    {
                        TopDrop2GCellDailyId = c.Int(nullable: false, identity: true),
                        StatDate = c.DateTime(nullable: false),
                        BscId = c.Short(nullable: false),
                        BtsId = c.Int(nullable: false),
                        CellId = c.Int(nullable: false),
                        SectorId = c.Byte(nullable: false),
                        Frequency = c.Int(nullable: false),
                        CdrCalls = c.Int(nullable: false),
                        CdrDrops = c.Int(nullable: false),
                        KpiCalls = c.Int(nullable: false),
                        KpiDrops = c.Int(nullable: false),
                        ErasureDrops = c.Int(nullable: false),
                        AverageRssi = c.Double(nullable: false),
                        MainRssi = c.Double(nullable: false),
                        SubRssi = c.Double(nullable: false),
                        AverageDropEcio = c.Double(nullable: false),
                        AverageDropDistance = c.Double(nullable: false),
                        DropCause = c.String(),
                        CdrCallsDistanceInfo_TopDrop2GCellDailyId = c.Int(),
                        CdrCallsHourInfo_TopDrop2GCellDailyId = c.Int(),
                        CdrDropsDistanceInfo_TopDrop2GCellDailyId = c.Int(),
                        CdrDropsHourInfo_TopDrop2GCellDailyId = c.Int(),
                        DropEcioDistanceInfo_TopDrop2GCellDailyId = c.Int(),
                        DropEcioHourInfo_TopDrop2GCellDailyId = c.Int(),
                        ErasureDropsHourInfo_TopDrop2GCellDailyId = c.Int(),
                        GoodEcioDistanceInfo_TopDrop2GCellDailyId = c.Int(),
                        KpiCallsHourInfo_TopDrop2GCellDailyId = c.Int(),
                        KpiDropsHourInfo_TopDrop2GCellDailyId = c.Int(),
                        MainRssiHourInfo_TopDrop2GCellDailyId = c.Int(),
                        SubRssiHourInfo_TopDrop2GCellDailyId = c.Int(),
                    })
                .PrimaryKey(t => t.TopDrop2GCellDailyId)
                .ForeignKey("dbo.CdrCallsDistanceInfoes", t => t.CdrCallsDistanceInfo_TopDrop2GCellDailyId)
                .ForeignKey("dbo.CdrCallsHourInfoes", t => t.CdrCallsHourInfo_TopDrop2GCellDailyId)
                .ForeignKey("dbo.CdrDropsDistanceInfoes", t => t.CdrDropsDistanceInfo_TopDrop2GCellDailyId)
                .ForeignKey("dbo.CdrDropsHourInfoes", t => t.CdrDropsHourInfo_TopDrop2GCellDailyId)
                .ForeignKey("dbo.DropEcioDistanceInfoes", t => t.DropEcioDistanceInfo_TopDrop2GCellDailyId)
                .ForeignKey("dbo.DropEcioHourInfoes", t => t.DropEcioHourInfo_TopDrop2GCellDailyId)
                .ForeignKey("dbo.ErasureDropsHourInfoes", t => t.ErasureDropsHourInfo_TopDrop2GCellDailyId)
                .ForeignKey("dbo.GoodEcioDistanceInfoes", t => t.GoodEcioDistanceInfo_TopDrop2GCellDailyId)
                .ForeignKey("dbo.KpiCallsHourInfoes", t => t.KpiCallsHourInfo_TopDrop2GCellDailyId)
                .ForeignKey("dbo.KpiDropsHourInfoes", t => t.KpiDropsHourInfo_TopDrop2GCellDailyId)
                .ForeignKey("dbo.MainRssiHourInfoes", t => t.MainRssiHourInfo_TopDrop2GCellDailyId)
                .ForeignKey("dbo.SubRssiHourInfoes", t => t.SubRssiHourInfo_TopDrop2GCellDailyId)
                .Index(t => t.CdrCallsDistanceInfo_TopDrop2GCellDailyId)
                .Index(t => t.CdrCallsHourInfo_TopDrop2GCellDailyId)
                .Index(t => t.CdrDropsDistanceInfo_TopDrop2GCellDailyId)
                .Index(t => t.CdrDropsHourInfo_TopDrop2GCellDailyId)
                .Index(t => t.DropEcioDistanceInfo_TopDrop2GCellDailyId)
                .Index(t => t.DropEcioHourInfo_TopDrop2GCellDailyId)
                .Index(t => t.ErasureDropsHourInfo_TopDrop2GCellDailyId)
                .Index(t => t.GoodEcioDistanceInfo_TopDrop2GCellDailyId)
                .Index(t => t.KpiCallsHourInfo_TopDrop2GCellDailyId)
                .Index(t => t.KpiDropsHourInfo_TopDrop2GCellDailyId)
                .Index(t => t.MainRssiHourInfo_TopDrop2GCellDailyId)
                .Index(t => t.SubRssiHourInfo_TopDrop2GCellDailyId);
            
            CreateTable(
                "dbo.CdrCallsDistanceInfoes",
                c => new
                    {
                        TopDrop2GCellDailyId = c.Int(nullable: false, identity: true),
                        DistanceTo1000Info = c.Int(nullable: false),
                        DistanceTo1200Info = c.Int(nullable: false),
                        DistanceTo1400Info = c.Int(nullable: false),
                        DistanceTo1600Info = c.Int(nullable: false),
                        DistanceTo1800Info = c.Int(nullable: false),
                        DistanceTo2000Info = c.Int(nullable: false),
                        DistanceTo200Info = c.Int(nullable: false),
                        DistanceTo2200Info = c.Int(nullable: false),
                        DistanceTo2400Info = c.Int(nullable: false),
                        DistanceTo2600Info = c.Int(nullable: false),
                        DistanceTo2800Info = c.Int(nullable: false),
                        DistanceTo3000Info = c.Int(nullable: false),
                        DistanceTo4000Info = c.Int(nullable: false),
                        DistanceTo400Info = c.Int(nullable: false),
                        DistanceTo5000Info = c.Int(nullable: false),
                        DistanceTo6000Info = c.Int(nullable: false),
                        DistanceTo600Info = c.Int(nullable: false),
                        DistanceTo7000Info = c.Int(nullable: false),
                        DistanceTo8000Info = c.Int(nullable: false),
                        DistanceTo800Info = c.Int(nullable: false),
                        DistanceTo9000Info = c.Int(nullable: false),
                        DistanceToInfInfo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TopDrop2GCellDailyId);
            
            CreateTable(
                "dbo.CdrCallsHourInfoes",
                c => new
                    {
                        TopDrop2GCellDailyId = c.Int(nullable: false, identity: true),
                        Hour0Info = c.Int(nullable: false),
                        Hour10Info = c.Int(nullable: false),
                        Hour11Info = c.Int(nullable: false),
                        Hour12Info = c.Int(nullable: false),
                        Hour13Info = c.Int(nullable: false),
                        Hour14Info = c.Int(nullable: false),
                        Hour15Info = c.Int(nullable: false),
                        Hour16Info = c.Int(nullable: false),
                        Hour17Info = c.Int(nullable: false),
                        Hour18Info = c.Int(nullable: false),
                        Hour19Info = c.Int(nullable: false),
                        Hour1Info = c.Int(nullable: false),
                        Hour20Info = c.Int(nullable: false),
                        Hour21Info = c.Int(nullable: false),
                        Hour22Info = c.Int(nullable: false),
                        Hour23Info = c.Int(nullable: false),
                        Hour2Info = c.Int(nullable: false),
                        Hour3Info = c.Int(nullable: false),
                        Hour4Info = c.Int(nullable: false),
                        Hour5Info = c.Int(nullable: false),
                        Hour6Info = c.Int(nullable: false),
                        Hour7Info = c.Int(nullable: false),
                        Hour8Info = c.Int(nullable: false),
                        Hour9Info = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TopDrop2GCellDailyId);
            
            CreateTable(
                "dbo.CdrDropsDistanceInfoes",
                c => new
                    {
                        TopDrop2GCellDailyId = c.Int(nullable: false, identity: true),
                        DistanceTo1000Info = c.Int(nullable: false),
                        DistanceTo1200Info = c.Int(nullable: false),
                        DistanceTo1400Info = c.Int(nullable: false),
                        DistanceTo1600Info = c.Int(nullable: false),
                        DistanceTo1800Info = c.Int(nullable: false),
                        DistanceTo2000Info = c.Int(nullable: false),
                        DistanceTo200Info = c.Int(nullable: false),
                        DistanceTo2200Info = c.Int(nullable: false),
                        DistanceTo2400Info = c.Int(nullable: false),
                        DistanceTo2600Info = c.Int(nullable: false),
                        DistanceTo2800Info = c.Int(nullable: false),
                        DistanceTo3000Info = c.Int(nullable: false),
                        DistanceTo4000Info = c.Int(nullable: false),
                        DistanceTo400Info = c.Int(nullable: false),
                        DistanceTo5000Info = c.Int(nullable: false),
                        DistanceTo6000Info = c.Int(nullable: false),
                        DistanceTo600Info = c.Int(nullable: false),
                        DistanceTo7000Info = c.Int(nullable: false),
                        DistanceTo8000Info = c.Int(nullable: false),
                        DistanceTo800Info = c.Int(nullable: false),
                        DistanceTo9000Info = c.Int(nullable: false),
                        DistanceToInfInfo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TopDrop2GCellDailyId);
            
            CreateTable(
                "dbo.CdrDropsHourInfoes",
                c => new
                    {
                        TopDrop2GCellDailyId = c.Int(nullable: false, identity: true),
                        Hour0Info = c.Int(nullable: false),
                        Hour10Info = c.Int(nullable: false),
                        Hour11Info = c.Int(nullable: false),
                        Hour12Info = c.Int(nullable: false),
                        Hour13Info = c.Int(nullable: false),
                        Hour14Info = c.Int(nullable: false),
                        Hour15Info = c.Int(nullable: false),
                        Hour16Info = c.Int(nullable: false),
                        Hour17Info = c.Int(nullable: false),
                        Hour18Info = c.Int(nullable: false),
                        Hour19Info = c.Int(nullable: false),
                        Hour1Info = c.Int(nullable: false),
                        Hour20Info = c.Int(nullable: false),
                        Hour21Info = c.Int(nullable: false),
                        Hour22Info = c.Int(nullable: false),
                        Hour23Info = c.Int(nullable: false),
                        Hour2Info = c.Int(nullable: false),
                        Hour3Info = c.Int(nullable: false),
                        Hour4Info = c.Int(nullable: false),
                        Hour5Info = c.Int(nullable: false),
                        Hour6Info = c.Int(nullable: false),
                        Hour7Info = c.Int(nullable: false),
                        Hour8Info = c.Int(nullable: false),
                        Hour9Info = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TopDrop2GCellDailyId);
            
            CreateTable(
                "dbo.DropEcioDistanceInfoes",
                c => new
                    {
                        TopDrop2GCellDailyId = c.Int(nullable: false, identity: true),
                        DistanceTo1000Info = c.Double(nullable: false),
                        DistanceTo1200Info = c.Double(nullable: false),
                        DistanceTo1400Info = c.Double(nullable: false),
                        DistanceTo1600Info = c.Double(nullable: false),
                        DistanceTo1800Info = c.Double(nullable: false),
                        DistanceTo2000Info = c.Double(nullable: false),
                        DistanceTo200Info = c.Double(nullable: false),
                        DistanceTo2200Info = c.Double(nullable: false),
                        DistanceTo2400Info = c.Double(nullable: false),
                        DistanceTo2600Info = c.Double(nullable: false),
                        DistanceTo2800Info = c.Double(nullable: false),
                        DistanceTo3000Info = c.Double(nullable: false),
                        DistanceTo4000Info = c.Double(nullable: false),
                        DistanceTo400Info = c.Double(nullable: false),
                        DistanceTo5000Info = c.Double(nullable: false),
                        DistanceTo6000Info = c.Double(nullable: false),
                        DistanceTo600Info = c.Double(nullable: false),
                        DistanceTo7000Info = c.Double(nullable: false),
                        DistanceTo8000Info = c.Double(nullable: false),
                        DistanceTo800Info = c.Double(nullable: false),
                        DistanceTo9000Info = c.Double(nullable: false),
                        DistanceToInfInfo = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.TopDrop2GCellDailyId);
            
            CreateTable(
                "dbo.DropEcioHourInfoes",
                c => new
                    {
                        TopDrop2GCellDailyId = c.Int(nullable: false, identity: true),
                        Hour0Info = c.Double(nullable: false),
                        Hour10Info = c.Double(nullable: false),
                        Hour11Info = c.Double(nullable: false),
                        Hour12Info = c.Double(nullable: false),
                        Hour13Info = c.Double(nullable: false),
                        Hour14Info = c.Double(nullable: false),
                        Hour15Info = c.Double(nullable: false),
                        Hour16Info = c.Double(nullable: false),
                        Hour17Info = c.Double(nullable: false),
                        Hour18Info = c.Double(nullable: false),
                        Hour19Info = c.Double(nullable: false),
                        Hour1Info = c.Double(nullable: false),
                        Hour20Info = c.Double(nullable: false),
                        Hour21Info = c.Double(nullable: false),
                        Hour22Info = c.Double(nullable: false),
                        Hour23Info = c.Double(nullable: false),
                        Hour2Info = c.Double(nullable: false),
                        Hour3Info = c.Double(nullable: false),
                        Hour4Info = c.Double(nullable: false),
                        Hour5Info = c.Double(nullable: false),
                        Hour6Info = c.Double(nullable: false),
                        Hour7Info = c.Double(nullable: false),
                        Hour8Info = c.Double(nullable: false),
                        Hour9Info = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.TopDrop2GCellDailyId);
            
            CreateTable(
                "dbo.ErasureDropsHourInfoes",
                c => new
                    {
                        TopDrop2GCellDailyId = c.Int(nullable: false, identity: true),
                        Hour0Info = c.Int(nullable: false),
                        Hour10Info = c.Int(nullable: false),
                        Hour11Info = c.Int(nullable: false),
                        Hour12Info = c.Int(nullable: false),
                        Hour13Info = c.Int(nullable: false),
                        Hour14Info = c.Int(nullable: false),
                        Hour15Info = c.Int(nullable: false),
                        Hour16Info = c.Int(nullable: false),
                        Hour17Info = c.Int(nullable: false),
                        Hour18Info = c.Int(nullable: false),
                        Hour19Info = c.Int(nullable: false),
                        Hour1Info = c.Int(nullable: false),
                        Hour20Info = c.Int(nullable: false),
                        Hour21Info = c.Int(nullable: false),
                        Hour22Info = c.Int(nullable: false),
                        Hour23Info = c.Int(nullable: false),
                        Hour2Info = c.Int(nullable: false),
                        Hour3Info = c.Int(nullable: false),
                        Hour4Info = c.Int(nullable: false),
                        Hour5Info = c.Int(nullable: false),
                        Hour6Info = c.Int(nullable: false),
                        Hour7Info = c.Int(nullable: false),
                        Hour8Info = c.Int(nullable: false),
                        Hour9Info = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TopDrop2GCellDailyId);
            
            CreateTable(
                "dbo.GoodEcioDistanceInfoes",
                c => new
                    {
                        TopDrop2GCellDailyId = c.Int(nullable: false, identity: true),
                        DistanceTo1000Info = c.Double(nullable: false),
                        DistanceTo1200Info = c.Double(nullable: false),
                        DistanceTo1400Info = c.Double(nullable: false),
                        DistanceTo1600Info = c.Double(nullable: false),
                        DistanceTo1800Info = c.Double(nullable: false),
                        DistanceTo2000Info = c.Double(nullable: false),
                        DistanceTo200Info = c.Double(nullable: false),
                        DistanceTo2200Info = c.Double(nullable: false),
                        DistanceTo2400Info = c.Double(nullable: false),
                        DistanceTo2600Info = c.Double(nullable: false),
                        DistanceTo2800Info = c.Double(nullable: false),
                        DistanceTo3000Info = c.Double(nullable: false),
                        DistanceTo4000Info = c.Double(nullable: false),
                        DistanceTo400Info = c.Double(nullable: false),
                        DistanceTo5000Info = c.Double(nullable: false),
                        DistanceTo6000Info = c.Double(nullable: false),
                        DistanceTo600Info = c.Double(nullable: false),
                        DistanceTo7000Info = c.Double(nullable: false),
                        DistanceTo8000Info = c.Double(nullable: false),
                        DistanceTo800Info = c.Double(nullable: false),
                        DistanceTo9000Info = c.Double(nullable: false),
                        DistanceToInfInfo = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.TopDrop2GCellDailyId);
            
            CreateTable(
                "dbo.KpiCallsHourInfoes",
                c => new
                    {
                        TopDrop2GCellDailyId = c.Int(nullable: false, identity: true),
                        Hour0Info = c.Int(nullable: false),
                        Hour10Info = c.Int(nullable: false),
                        Hour11Info = c.Int(nullable: false),
                        Hour12Info = c.Int(nullable: false),
                        Hour13Info = c.Int(nullable: false),
                        Hour14Info = c.Int(nullable: false),
                        Hour15Info = c.Int(nullable: false),
                        Hour16Info = c.Int(nullable: false),
                        Hour17Info = c.Int(nullable: false),
                        Hour18Info = c.Int(nullable: false),
                        Hour19Info = c.Int(nullable: false),
                        Hour1Info = c.Int(nullable: false),
                        Hour20Info = c.Int(nullable: false),
                        Hour21Info = c.Int(nullable: false),
                        Hour22Info = c.Int(nullable: false),
                        Hour23Info = c.Int(nullable: false),
                        Hour2Info = c.Int(nullable: false),
                        Hour3Info = c.Int(nullable: false),
                        Hour4Info = c.Int(nullable: false),
                        Hour5Info = c.Int(nullable: false),
                        Hour6Info = c.Int(nullable: false),
                        Hour7Info = c.Int(nullable: false),
                        Hour8Info = c.Int(nullable: false),
                        Hour9Info = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TopDrop2GCellDailyId);
            
            CreateTable(
                "dbo.KpiDropsHourInfoes",
                c => new
                    {
                        TopDrop2GCellDailyId = c.Int(nullable: false, identity: true),
                        Hour0Info = c.Int(nullable: false),
                        Hour10Info = c.Int(nullable: false),
                        Hour11Info = c.Int(nullable: false),
                        Hour12Info = c.Int(nullable: false),
                        Hour13Info = c.Int(nullable: false),
                        Hour14Info = c.Int(nullable: false),
                        Hour15Info = c.Int(nullable: false),
                        Hour16Info = c.Int(nullable: false),
                        Hour17Info = c.Int(nullable: false),
                        Hour18Info = c.Int(nullable: false),
                        Hour19Info = c.Int(nullable: false),
                        Hour1Info = c.Int(nullable: false),
                        Hour20Info = c.Int(nullable: false),
                        Hour21Info = c.Int(nullable: false),
                        Hour22Info = c.Int(nullable: false),
                        Hour23Info = c.Int(nullable: false),
                        Hour2Info = c.Int(nullable: false),
                        Hour3Info = c.Int(nullable: false),
                        Hour4Info = c.Int(nullable: false),
                        Hour5Info = c.Int(nullable: false),
                        Hour6Info = c.Int(nullable: false),
                        Hour7Info = c.Int(nullable: false),
                        Hour8Info = c.Int(nullable: false),
                        Hour9Info = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TopDrop2GCellDailyId);
            
            CreateTable(
                "dbo.MainRssiHourInfoes",
                c => new
                    {
                        TopDrop2GCellDailyId = c.Int(nullable: false, identity: true),
                        Hour0Info = c.Double(nullable: false),
                        Hour10Info = c.Double(nullable: false),
                        Hour11Info = c.Double(nullable: false),
                        Hour12Info = c.Double(nullable: false),
                        Hour13Info = c.Double(nullable: false),
                        Hour14Info = c.Double(nullable: false),
                        Hour15Info = c.Double(nullable: false),
                        Hour16Info = c.Double(nullable: false),
                        Hour17Info = c.Double(nullable: false),
                        Hour18Info = c.Double(nullable: false),
                        Hour19Info = c.Double(nullable: false),
                        Hour1Info = c.Double(nullable: false),
                        Hour20Info = c.Double(nullable: false),
                        Hour21Info = c.Double(nullable: false),
                        Hour22Info = c.Double(nullable: false),
                        Hour23Info = c.Double(nullable: false),
                        Hour2Info = c.Double(nullable: false),
                        Hour3Info = c.Double(nullable: false),
                        Hour4Info = c.Double(nullable: false),
                        Hour5Info = c.Double(nullable: false),
                        Hour6Info = c.Double(nullable: false),
                        Hour7Info = c.Double(nullable: false),
                        Hour8Info = c.Double(nullable: false),
                        Hour9Info = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.TopDrop2GCellDailyId);
            
            CreateTable(
                "dbo.SubRssiHourInfoes",
                c => new
                    {
                        TopDrop2GCellDailyId = c.Int(nullable: false, identity: true),
                        Hour0Info = c.Double(nullable: false),
                        Hour10Info = c.Double(nullable: false),
                        Hour11Info = c.Double(nullable: false),
                        Hour12Info = c.Double(nullable: false),
                        Hour13Info = c.Double(nullable: false),
                        Hour14Info = c.Double(nullable: false),
                        Hour15Info = c.Double(nullable: false),
                        Hour16Info = c.Double(nullable: false),
                        Hour17Info = c.Double(nullable: false),
                        Hour18Info = c.Double(nullable: false),
                        Hour19Info = c.Double(nullable: false),
                        Hour1Info = c.Double(nullable: false),
                        Hour20Info = c.Double(nullable: false),
                        Hour21Info = c.Double(nullable: false),
                        Hour22Info = c.Double(nullable: false),
                        Hour23Info = c.Double(nullable: false),
                        Hour2Info = c.Double(nullable: false),
                        Hour3Info = c.Double(nullable: false),
                        Hour4Info = c.Double(nullable: false),
                        Hour5Info = c.Double(nullable: false),
                        Hour6Info = c.Double(nullable: false),
                        Hour7Info = c.Double(nullable: false),
                        Hour8Info = c.Double(nullable: false),
                        Hour9Info = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.TopDrop2GCellDailyId);
            
            //CreateTable(
            //    "dbo.CdmaRegionStats",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Region = c.String(),
            //            StatDate = c.DateTime(nullable: false),
            //            ErlangIncludingSwitch = c.Double(nullable: false),
            //            ErlangExcludingSwitch = c.Double(nullable: false),
            //            Drop2GNum = c.Int(nullable: false),
            //            Drop2GDem = c.Int(nullable: false),
            //            CallSetupNum = c.Int(nullable: false),
            //            CallSetupDem = c.Int(nullable: false),
            //            EcioNum = c.Long(nullable: false),
            //            EcioDem = c.Long(nullable: false),
            //            Utility2GNum = c.Int(nullable: false),
            //            Utility2GDem = c.Int(nullable: false),
            //            Flow = c.Double(nullable: false),
            //            Erlang3G = c.Double(nullable: false),
            //            Drop3GNum = c.Int(nullable: false),
            //            Drop3GDem = c.Int(nullable: false),
            //            ConnectionNum = c.Int(nullable: false),
            //            ConnectionDem = c.Int(nullable: false),
            //            CiNum = c.Long(nullable: false),
            //            CiDem = c.Long(nullable: false),
            //            LinkBusyNum = c.Int(nullable: false),
            //            LinkBusyDem = c.Int(nullable: false),
            //            DownSwitchNum = c.Long(nullable: false),
            //            DownSwitchDem = c.Int(nullable: false),
            //            Utility3GNum = c.Int(nullable: false),
            //            Utility3GDem = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.OptimizeRegions",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            City = c.String(),
            //            Region = c.String(),
            //            District = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.TopConnection3GCell",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            WirelessDrop = c.Int(nullable: false),
            //            ConnectionAttempts = c.Int(nullable: false),
            //            ConnectionFails = c.Int(nullable: false),
            //            LinkBusyRate = c.Double(nullable: false),
            //            StatTime = c.DateTime(nullable: false),
            //            City = c.String(),
            //            BtsId = c.Int(nullable: false),
            //            CellId = c.Int(nullable: false),
            //            SectorId = c.Byte(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.TopDrop2GCell",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Frequency = c.Short(nullable: false),
            //            Drops = c.Int(nullable: false),
            //            MoAssignmentSuccess = c.Int(nullable: false),
            //            MtAssignmentSuccess = c.Int(nullable: false),
            //            TrafficAssignmentSuccess = c.Int(nullable: false),
            //            CallAttempts = c.Int(nullable: false),
            //            StatTime = c.DateTime(nullable: false),
            //            City = c.String(),
            //            BtsId = c.Int(nullable: false),
            //            CellId = c.Int(nullable: false),
            //            SectorId = c.Byte(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //DropColumn("dbo.CdmaBts", "UpdateName");
            //DropColumn("dbo.CdmaCells", "UpdateFirstFrequency");
            //DropColumn("dbo.CdmaCells", "ImportNewInfo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CdmaCells", "ImportNewInfo", c => c.Boolean(nullable: false));
            AddColumn("dbo.CdmaCells", "UpdateFirstFrequency", c => c.Boolean(nullable: false));
            AddColumn("dbo.CdmaBts", "UpdateName", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.AlarmHourInfoes", "TopDrop2GCellDailyId", "dbo.TopDrop2GCellDaily");
            DropForeignKey("dbo.TopDrop2GCellDaily", "SubRssiHourInfo_TopDrop2GCellDailyId", "dbo.SubRssiHourInfoes");
            DropForeignKey("dbo.TopDrop2GCellDaily", "MainRssiHourInfo_TopDrop2GCellDailyId", "dbo.MainRssiHourInfoes");
            DropForeignKey("dbo.TopDrop2GCellDaily", "KpiDropsHourInfo_TopDrop2GCellDailyId", "dbo.KpiDropsHourInfoes");
            DropForeignKey("dbo.TopDrop2GCellDaily", "KpiCallsHourInfo_TopDrop2GCellDailyId", "dbo.KpiCallsHourInfoes");
            DropForeignKey("dbo.TopDrop2GCellDaily", "GoodEcioDistanceInfo_TopDrop2GCellDailyId", "dbo.GoodEcioDistanceInfoes");
            DropForeignKey("dbo.TopDrop2GCellDaily", "ErasureDropsHourInfo_TopDrop2GCellDailyId", "dbo.ErasureDropsHourInfoes");
            DropForeignKey("dbo.TopDrop2GCellDaily", "DropEcioHourInfo_TopDrop2GCellDailyId", "dbo.DropEcioHourInfoes");
            DropForeignKey("dbo.TopDrop2GCellDaily", "DropEcioDistanceInfo_TopDrop2GCellDailyId", "dbo.DropEcioDistanceInfoes");
            DropForeignKey("dbo.TopDrop2GCellDaily", "CdrDropsHourInfo_TopDrop2GCellDailyId", "dbo.CdrDropsHourInfoes");
            DropForeignKey("dbo.TopDrop2GCellDaily", "CdrDropsDistanceInfo_TopDrop2GCellDailyId", "dbo.CdrDropsDistanceInfoes");
            DropForeignKey("dbo.TopDrop2GCellDaily", "CdrCallsHourInfo_TopDrop2GCellDailyId", "dbo.CdrCallsHourInfoes");
            DropForeignKey("dbo.TopDrop2GCellDaily", "CdrCallsDistanceInfo_TopDrop2GCellDailyId", "dbo.CdrCallsDistanceInfoes");
            DropIndex("dbo.TopDrop2GCellDaily", new[] { "SubRssiHourInfo_TopDrop2GCellDailyId" });
            DropIndex("dbo.TopDrop2GCellDaily", new[] { "MainRssiHourInfo_TopDrop2GCellDailyId" });
            DropIndex("dbo.TopDrop2GCellDaily", new[] { "KpiDropsHourInfo_TopDrop2GCellDailyId" });
            DropIndex("dbo.TopDrop2GCellDaily", new[] { "KpiCallsHourInfo_TopDrop2GCellDailyId" });
            DropIndex("dbo.TopDrop2GCellDaily", new[] { "GoodEcioDistanceInfo_TopDrop2GCellDailyId" });
            DropIndex("dbo.TopDrop2GCellDaily", new[] { "ErasureDropsHourInfo_TopDrop2GCellDailyId" });
            DropIndex("dbo.TopDrop2GCellDaily", new[] { "DropEcioHourInfo_TopDrop2GCellDailyId" });
            DropIndex("dbo.TopDrop2GCellDaily", new[] { "DropEcioDistanceInfo_TopDrop2GCellDailyId" });
            DropIndex("dbo.TopDrop2GCellDaily", new[] { "CdrDropsHourInfo_TopDrop2GCellDailyId" });
            DropIndex("dbo.TopDrop2GCellDaily", new[] { "CdrDropsDistanceInfo_TopDrop2GCellDailyId" });
            DropIndex("dbo.TopDrop2GCellDaily", new[] { "CdrCallsHourInfo_TopDrop2GCellDailyId" });
            DropIndex("dbo.TopDrop2GCellDaily", new[] { "CdrCallsDistanceInfo_TopDrop2GCellDailyId" });
            DropIndex("dbo.AlarmHourInfoes", new[] { "TopDrop2GCellDailyId" });
            DropTable("dbo.TopDrop2GCell");
            DropTable("dbo.TopConnection3GCell");
            DropTable("dbo.OptimizeRegions");
            DropTable("dbo.CdmaRegionStats");
            DropTable("dbo.SubRssiHourInfoes");
            DropTable("dbo.MainRssiHourInfoes");
            DropTable("dbo.KpiDropsHourInfoes");
            DropTable("dbo.KpiCallsHourInfoes");
            DropTable("dbo.GoodEcioDistanceInfoes");
            DropTable("dbo.ErasureDropsHourInfoes");
            DropTable("dbo.DropEcioHourInfoes");
            DropTable("dbo.DropEcioDistanceInfoes");
            DropTable("dbo.CdrDropsHourInfoes");
            DropTable("dbo.CdrDropsDistanceInfoes");
            DropTable("dbo.CdrCallsHourInfoes");
            DropTable("dbo.CdrCallsDistanceInfoes");
            DropTable("dbo.TopDrop2GCellDaily");
            DropTable("dbo.AlarmHourInfoes");
        }
    }
}
