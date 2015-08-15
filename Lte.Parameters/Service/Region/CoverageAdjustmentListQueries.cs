using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Regular;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Service.Region
{
    public static class CoverageAdjustmentListQueries
    {
        public static CoverageAdjustment Merge(this IEnumerable<CoverageAdjustment> adjustments)
        {
            return !adjustments.Any() ? null :
                new CoverageAdjustment
                {
                    ENodebId = adjustments.ElementAt(0).ENodebId,
                    SectorId = adjustments.ElementAt(0).SectorId,
                    Frequency = adjustments.ElementAt(0).Frequency,
                    Factor75m = adjustments.AverageIgnoreZeros(x => x.Factor75m),
                    Factor75 = adjustments.AverageIgnoreZeros(x => x.Factor75),
                    Factor45m = adjustments.AverageIgnoreZeros(x => x.Factor45m),
                    Factor45 = adjustments.AverageIgnoreZeros(x => x.Factor45),
                    Factor165m = adjustments.AverageIgnoreZeros(x => x.Factor165m),
                    Factor165 = adjustments.AverageIgnoreZeros(x => x.Factor165),
                    Factor15m = adjustments.AverageIgnoreZeros(x => x.Factor15m),
                    Factor105 = adjustments.AverageIgnoreZeros(x => x.Factor105),
                    Factor105m = adjustments.AverageIgnoreZeros(x => x.Factor105m),
                    Factor135 = adjustments.AverageIgnoreZeros(x => x.Factor135),
                    Factor135m = adjustments.AverageIgnoreZeros(x => x.Factor135m),
                    Factor15 = adjustments.AverageIgnoreZeros(x => x.Factor15)
                };
        }

        public static IEnumerable<CoverageAdjustment> MergeList(
            this IEnumerable<CoverageAdjustment> adjustments)
        {
            var results = from a in adjustments
                          group a by new { a.ENodebId, a.SectorId, a.Frequency } into g
                          select new CoverageAdjustment
                          {
                              ENodebId = g.Key.ENodebId,
                              SectorId = g.Key.SectorId,
                              Frequency = g.Key.Frequency,
                              Factor75m = g.AverageIgnoreZeros(x => x.Factor75m),
                              Factor75 = g.AverageIgnoreZeros(x => x.Factor75),
                              Factor45m = g.AverageIgnoreZeros(x => x.Factor45m),
                              Factor45 = g.AverageIgnoreZeros(x => x.Factor45),
                              Factor165m = g.AverageIgnoreZeros(x => x.Factor165m),
                              Factor165 = g.AverageIgnoreZeros(x => x.Factor165),
                              Factor15m = g.AverageIgnoreZeros(x => x.Factor15m),
                              Factor105 = g.AverageIgnoreZeros(x => x.Factor105),
                              Factor105m = g.AverageIgnoreZeros(x => x.Factor105m),
                              Factor135 = g.AverageIgnoreZeros(x => x.Factor135),
                              Factor135m = g.AverageIgnoreZeros(x => x.Factor135m),
                              Factor15 = g.AverageIgnoreZeros(x => x.Factor15)
                          };
            return results;
        }
    }
}
