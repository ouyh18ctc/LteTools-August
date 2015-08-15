using System;
using System.Collections.Generic;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.Regular;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Entities;

namespace Lte.Parameters.Kpi.Service
{
    public abstract class ImportStatsService<TCsvInfo, TStat>
        where TCsvInfo : ICarrierName
        where TStat : class, ITimeStat, new()
    {
        private readonly ITopCellRepository<TStat> _repository;

        protected ImportStatsService(ITopCellRepository<TStat> repository)
        {
            _repository = repository;
        }

        protected abstract string Import(TStat stat, List<TCsvInfo> csvStats,
            ref int beginIndex, string oldCarrier);

        public int ImportStats(List<TCsvInfo> csvStats, int maxIndex, DateTime statDate = default(DateTime))
        {
            if (csvStats.Count == 0) { return 0; }
            int count = 0;
            int beginIndex = 0;
            string oldCarrier = csvStats[0].Carrier;
            maxIndex = Math.Min(maxIndex, csvStats.Count);
            while (beginIndex < maxIndex)
            {
                TStat stat = new TStat { StatTime = statDate };
                oldCarrier = Import(stat, csvStats, ref beginIndex, oldCarrier);
                _repository.AddOneStat(stat);
                count++;
            }
            _repository.SaveChanges();
            return count;
        }
    }

    public class TopCellRepositoryDropDailyService : ImportStatsService<TopDrop2GCellCsv, TopDrop2GCellDaily>
    {
        public TopCellRepositoryDropDailyService(ITopCellRepository<TopDrop2GCellDaily> repository)
            :base(repository)
        {
        }

        protected override string Import(TopDrop2GCellDaily stat, List<TopDrop2GCellCsv> csvStats, 
            ref int beginIndex, string oldCarrier)
        {
            stat.ImportCarrierInfo(oldCarrier.GetSplittedFields('_'));
            return stat.Import(csvStats, ref beginIndex, oldCarrier);
        }
    }
}
