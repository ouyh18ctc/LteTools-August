using System.Collections.Generic;
using Abp.Domain.Repositories;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Abstract
{
    public interface ICoverageRepository : IRepository<CoverageAdjustment>
    {
        void Save(IEnumerable<CoverageAdjustment> adjustments);
    }

    public interface IInterferenceStatRepository : IRepository<InterferenceStat>
    {
        void Save(IEnumerable<InterferenceStat> stats);
    }

    public interface IPureInterferenceStatRepository : IRepository<PureInterferenceStat>
    {
        void Save(IEnumerable<PureInterferenceStat> stats);
    }
}
