using System.Linq;
using Lte.Parameters.Concrete;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Entities;

namespace Lte.Parameters.Kpi.Concrete
{
    public class EFCdmaRegionStatRepository : ITopCellRepository<CdmaRegionStat>
    {
        private readonly EFParametersContext context = new EFParametersContext();

        public IQueryable<CdmaRegionStat> Stats
        {
            get
            {
                return context.CdmaRegionStats.AsQueryable();
            }
        }

        public void AddOneStat(CdmaRegionStat stat)
        {
            context.CdmaRegionStats.Add(stat);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
