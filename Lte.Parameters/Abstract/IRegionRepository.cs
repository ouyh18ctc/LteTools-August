using Abp.Domain.Repositories;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Abstract
{
    public interface IRegionRepository : IRepository<OptimizeRegion>
    {
    }

    public interface ITownRepository : IRepository<Town>
    {
    }

    public interface ICollegeRepository : IRepository<CollegeInfo>
    {
        CollegeRegion GetRegion(int id);
    }
}
