using System.Data.Entity;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Concrete
{
    public class EFRegionRepository : LightWeightRepositroyBase<OptimizeRegion>, IRegionRepository
    {
        protected override DbSet<OptimizeRegion> Entities
        {
            get { return context.OptimizeRegions; }
        }
    }

    public class EFTownRepository : LightWeightRepositroyBase<Town>, ITownRepository
    {
        protected override DbSet<Town> Entities
        {
            get { return context.Towns; }
        }
    }
}
