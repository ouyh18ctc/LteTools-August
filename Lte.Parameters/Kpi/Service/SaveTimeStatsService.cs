using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Lte.Domain.Geo.Abstract;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Abstract;

namespace Lte.Parameters.Kpi.Service
{
    public abstract class SaveTimeStatsService<TStat, TExcel>
        where TStat : class, ITimeStat, new()
    {
        private readonly ITopCellRepository<TStat> _repository;

        protected SaveTimeStatsService(ITopCellRepository<TStat> repository)
        {
            _repository = repository;
        }

        protected abstract void ImportAdditionalInfos(TStat stat);

        protected abstract IEnumerable<TStat> ImportStats(IEnumerable<TExcel> excelStats);

        public int Save(IEnumerable<TExcel> excelStats)
        {
            int result = 0;
            IEnumerable<TStat> stats = ImportStats(excelStats);
            IEnumerable<DateTime> times = stats.Select(x => x.StatTime).Distinct();
            foreach (TStat stat in times.Where(time => 
                !_repository.Stats.Any(x => x.StatTime == time)).Select(
                time => stats.Where(x => x.StatTime == time)).SelectMany(
                statsInCurrentTime => statsInCurrentTime))
            {
                ImportAdditionalInfos(stat);
                _repository.AddOneStat(stat);
                result++;
                if (result%1000 != 0) continue;
                _repository.SaveChanges();
            }
            _repository.SaveChanges();
            return result;
        }
    }

    public class SaveTimeTownStatsService<TStat, TExcel> : SaveTimeStatsService<TStat, TExcel>
        where TStat : class, ITimeStat, ITownId, IImportStat<IEnumerable<TExcel>>, new()
        where TExcel : ICell, ITimeStat
    {
        private readonly IEnumerable<Tuple<int, int>> townENodebIdPairs;

        public SaveTimeTownStatsService(ITopCellRepository<TStat> repository, IEnumerable<ENodeb> eNodebs) 
            : base(repository)
        {
            townENodebIdPairs = eNodebs.Select(x => new Tuple<int, int>(x.ENodebId, x.TownId)).Distinct();
        }

        protected override void ImportAdditionalInfos(TStat stat)
        {
            
        }

        protected override IEnumerable<TStat> ImportStats(IEnumerable<TExcel> excelStats)
        {
            List<TStat> stats = new List<TStat>();
            IEnumerable<int> townIds = townENodebIdPairs.Select(x => x.Item2).Distinct();
            foreach (int townId in townIds)
            {
                IEnumerable<int> eNodebIds = townENodebIdPairs.Where(x => x.Item2 == townId).Select(x => x.Item1);
                IEnumerable<TExcel> currentTownInfos
                    = from e in eNodebIds
                      join info in excelStats
                      on e equals info.CellId
                      select info;
                foreach (DateTime time in currentTownInfos.Select(x => x.StatTime).Distinct())
                {
                    TStat stat = new TStat { TownId = townId, StatTime = time };
                    stat.Import(currentTownInfos.Where(x => x.StatTime == time));
                    stats.Add(stat);
                }
            }
            return stats;
        }
    }

    public abstract class SaveTimeKpiStatsService<TStat, TExcel> : SaveTimeStatsService<TStat, TExcel>
        where TStat : class, IImportStat<TExcel>, ITimeStat, new()
    {
        protected SaveTimeKpiStatsService(ITopCellRepository<TStat> repository) : base(repository)
        {
        }

        protected override IEnumerable<TStat> ImportStats(IEnumerable<TExcel> excelStats)
        {
            return excelStats.Select(x =>
            {
                TStat stat = new TStat();
                stat.Import(x);
                return stat;
            });
        }
    }

    public class SaveTimeCityKpiStatsService<TStat, TExcel> : SaveTimeKpiStatsService<TStat, TExcel>
        where TStat : class, IImportStat<TExcel>, ITimeStat, ICityStat, new()
    {
        public SaveTimeCityKpiStatsService(ITopCellRepository<TStat> repository) : base(repository)
        {
        }

        public string CurrentCity { get; set; }

        protected override void ImportAdditionalInfos(TStat stat)
        {
            stat.City = CurrentCity;
        }
    }

    public class SaveTimeSingleKpiStatsService<TStat, TExcel> : SaveTimeKpiStatsService<TStat, TExcel>
        where TStat : class, IImportStat<TExcel>, ITimeStat, new()
    {
        public SaveTimeSingleKpiStatsService(ITopCellRepository<TStat> repository) : base(repository)
        {
        }

        protected override void ImportAdditionalInfos(TStat stat)
        {
        }
    }
}
