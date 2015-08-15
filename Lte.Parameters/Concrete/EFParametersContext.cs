using System.Data.Entity;
using System.Threading;
using Abp.EntityFramework;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Entities;

namespace Lte.Parameters.Concrete
{
    public class EFParametersContext : AbpDbContext
    {
        public EFParametersContext() : base("EFParametersContext")
        { 
        }

        public DbSet<Town> Towns { get; set; }

        public DbSet<CollegeInfo> CollegeInfos { get; set; }

        public DbSet<CollegeRegion> CollegeRegions { get; set; }

        public DbSet<InfrastructureInfo> InfrastructureInfos { get; set; }

        public DbSet<IndoorDistribution> IndoorDistributions { get; set; }

        public DbSet<OptimizeRegion> OptimizeRegions { get; set; }

        public DbSet<ENodeb> ENodebs { get; set; }

        public DbSet<ENodebPhoto> ENodebPhotos { get; set; }

        public DbSet<Cell> Cells { get; set; }

        public DbSet<CdmaBts> Btss { get; set; }

        public DbSet<CdmaCell> CdmaCells { get; set; }

        public DbSet<LteNeighborCell> LteNeighborCells { get; set; }

        public DbSet<NearestPciCell> NearestPciCells { get; set; }

        public DbSet<MrsCellDate> MrsCells { get; set; }

        public DbSet<MrsCellTa> MrsTaCells { get; set; }

        public DbSet<MroRsrpTa> MroRsrpTaCells { get; set; }

        public DbSet<CoverageAdjustment> CoverageAdjustments { get; set; }

        public DbSet<InterferenceStat> InterferenceStats { get; set; }

        public DbSet<PureInterferenceStat> PureInterferenceStats { get; set; }

        public DbSet<CdmaRegionStat> CdmaRegionStats { get; set; }

        public DbSet<TopDrop2GCell> TopDrop2GStats { get; set; }

        public DbSet<TopConnection3GCell> TopConnection3GStats { get; set; }

        public DbSet<TopDrop2GCellDaily> TopDrop2GDailyStats { get; set; }

        public DbSet<AlarmHourInfo> AlarmHourInfos { get; set; }

        public DbSet<NeighborHourInfo> NeighborHourInfos { get; set; }

        public DbSet<CdrCallsDistanceInfo> CdrCallsDistanceInfos { get; set; }

        public DbSet<CdrCallsHourInfo> CdrCallsHourInfos { get; set; }

        public DbSet<CdrDropsDistanceInfo> CdrDropsDistanceInfos { get; set; }

        public DbSet<CdrDropsHourInfo> CdrDropsHourInfos { get; set; }

        public DbSet<DropEcioDistanceInfo> DropEcioDistanceInfos { get; set; }

        public DbSet<DropEcioHourInfo> DropEcioHourInfos { get; set; }

        public DbSet<ErasureDropsHourInfo> ErasureDropsHourInfos { get; set; }

        public DbSet<GoodEcioDistanceInfo> GoodEcioDistanceInfos { get; set; }

        public DbSet<KpiCallsHourInfo> KpiCallsHourInfos { get; set; }

        public DbSet<KpiDropsHourInfo> KpiDropsHourInfos { get; set; }

        public DbSet<MainRssiHourInfo> MainRssiHourInfos { get; set; }

        public DbSet<SubRssiHourInfo> SubRssiHourInfos { get;set; }

        public DbSet<PreciseCoverage4G> PrecisCoverage4Gs { get; set; }

        public DbSet<TownPreciseCoverage4GStat> TownPreciseCoverage4GStats { get; set; }

        public DbSet<MonthPreciseCoverage4GStat> MonthPreciseCoverage4GStats { get; set; }

        public void SaveChangesWithDelay()
        {
            try
            {
                SaveChanges();
            }
            catch
            {
                Thread.Sleep(60000);
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TopDrop2GCellDaily>().HasMany(p => p.AlarmHourInfos).WithRequired(
                p => p.TopDrop2GCellDaily).HasForeignKey(d => d.TopDrop2GCellDailyId);
            modelBuilder.Entity<AlarmHourInfo>().HasRequired(p => p.TopDrop2GCellDaily).WithMany(
                d => d.AlarmHourInfos).HasForeignKey(p => p.TopDrop2GCellDailyId);

            modelBuilder.Entity<TopDrop2GCellDaily>().HasMany(p => p.NeighborHourInfos).WithRequired(
                p => p.TopDrop2GCellDaily).HasForeignKey(d => d.TopDrop2GCellDailyId);
            modelBuilder.Entity<NeighborHourInfo>().HasRequired(p => p.TopDrop2GCellDaily).WithMany(
                d => d.NeighborHourInfos).HasForeignKey(p => p.TopDrop2GCellDailyId);
        }
    }
}
