using System.Linq;
using Lte.Parameters.Concrete;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Entities;

namespace Lte.Parameters.Kpi.Concrete
{
    public class EFPreciseCoverage4GRepository : ITopCellRepository<PreciseCoverage4G>
    {
        private readonly EFParametersContext context = new EFParametersContext();

        public IQueryable<PreciseCoverage4G> Stats
        {
            get
            {
                return context.PrecisCoverage4Gs.AsQueryable();
            }
        }

        public void AddOneStat(PreciseCoverage4G stat)
        {
            context.PrecisCoverage4Gs.Add(stat);
        }

        public void SaveChanges()
        {
            context.SaveChangesWithDelay();
        }
    }

    public class EFTownPreciseCoverage4GStatRepository : ITopCellRepository<TownPreciseCoverage4GStat>
    {
        private EFParametersContext context = new EFParametersContext();

        public IQueryable<TownPreciseCoverage4GStat> Stats
        {
            get
            {
                return context.TownPreciseCoverage4GStats.AsQueryable();
            }
        }

        public void AddOneStat(TownPreciseCoverage4GStat stat)
        {
            context.TownPreciseCoverage4GStats.Add(stat);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }

    public class EFMonthPreciseCoverage4GStatRepository : ITopCellRepository<MonthPreciseCoverage4GStat>
    {
        private EFParametersContext context = new EFParametersContext();

        public IQueryable<MonthPreciseCoverage4GStat> Stats
        {
            get
            {
                return context.MonthPreciseCoverage4GStats.AsQueryable();
            }
        }

        public void AddOneStat(MonthPreciseCoverage4GStat stat)
        {
            context.MonthPreciseCoverage4GStats.Add(stat);
        }

        public void SaveChanges()
        {
            context.SaveChangesWithDelay();
        }
    }
}
