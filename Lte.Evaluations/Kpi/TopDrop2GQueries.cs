using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Abstract;
using Lte.Parameters.Service.Public;

namespace Lte.Evaluations.Kpi
{
    public class TopStatCount
    {
        public string CarrierName { get; set; }

        public int TopDates { get; set; }

        public int SumOfTimes { get; set; }
    }

    public static class TopDrop2GQueries
    {
        public static IEnumerable<TopStatCount> QueryTopCounts<TCell, TView>(this IEnumerable<TCell> stats,
            IBtsRepository btsRepository, IENodebRepository eNodebRepository, int topCounts)
            where TCell : class, ICdmaCell, new()
            where TView : class, IGetTopCellView, new()
        {
            CdmaLteNamesService<TCell> service = new CdmaLteNamesService<TCell>(stats,
                btsRepository.GetAllList(), eNodebRepository.GetAllList());
            IEnumerable<TView> cellViews = service.Clone<TView>();
            IEnumerable<TopStatCount> statCounts = from v in cellViews
                             group v by new { v.CdmaName, v.SectorId, v.Frequency } into g
                             select new TopStatCount
                             {
                                 CarrierName = g.Key.CdmaName + "-"
                                 + g.Key.SectorId + "-" + g.Key.Frequency,
                                 TopDates = g.Count(),
                                 SumOfTimes = g.Sum(v => v.Drops)
                             };
            return statCounts.OrderByDescending(x => x.SumOfTimes).Take(topCounts);
        }

    }
}
