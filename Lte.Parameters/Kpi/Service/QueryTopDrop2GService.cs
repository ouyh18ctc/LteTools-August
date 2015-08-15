using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Entities;

namespace Lte.Parameters.Kpi.Service
{
    public class QueryTopDrop2GService
    {
        private readonly ITopCellRepository<TopDrop2GCellDaily> _dailyStatRepository;
        private readonly int _cellId;
        private readonly byte _sectorId;
        private readonly short _frequency;
        private readonly DateTime _statDate;

        public QueryTopDrop2GService(ITopCellRepository<TopDrop2GCellDaily> dailyStatRepository,
            int cellId, byte sectorId, short frequency, DateTime statDate)
        {
            _dailyStatRepository = dailyStatRepository;
            _cellId = cellId;
            _sectorId = sectorId;
            _frequency = frequency;
            _statDate = statDate;
        }

        public TopDrop2GCellDaily QueryStat()
        {
            return _dailyStatRepository.Stats.FirstOrDefault(x => 
                x.StatTime == _statDate && x.CellId == _cellId && x.SectorId == _sectorId
                           && x.Frequency == _frequency);
        }

        public List<DistanceDistribution> GenerateDistanceDistribution()
        {
            var stats = _dailyStatRepository.Stats.Select(x =>
                new
                {
                    Stat = x,
                    CdrCalls = x.CdrCallsDistanceInfo,
                    CdrDrops = x.CdrDropsDistanceInfo,
                    DropEcio = x.DropEcioDistanceInfo,
                    GoodEcio = x.GoodEcioDistanceInfo
                });
            var stat = stats.FirstOrDefault(x =>
                x.Stat.StatTime == _statDate && x.Stat.CellId == _cellId && x.Stat.SectorId == _sectorId
                           && x.Stat.Frequency == _frequency);
            List<DistanceDistribution> result = new List<DistanceDistribution>();
            if (stat != null)
                result.Import(stat.CdrCalls, stat.CdrDrops, stat.DropEcio, stat.GoodEcio);
            return result;
        }

        public List<CoverageInterferenceDistribution> GenerateCoverageInterferenceDistribution()
        {
            var stats = _dailyStatRepository.Stats.Select(x =>
                new
                {
                    Stat = x,
                    DropEcio = x.DropEcioHourInfo,
                    MainRssi = x.MainRssiHourInfo,
                    SubRssi = x.SubRssiHourInfo
                });
            var stat = stats.FirstOrDefault(x =>
                x.Stat.StatTime == _statDate && x.Stat.CellId == _cellId && x.Stat.SectorId == _sectorId
                           && x.Stat.Frequency == _frequency);
            List<CoverageInterferenceDistribution> result = new List<CoverageInterferenceDistribution>();
            if (stat != null)
                result.Import(stat.DropEcio, stat.MainRssi, stat.SubRssi);
            return result;
        }

        public List<DropsHourDistribution> GenerateDropsHourDistribution()
        {
            var stats = _dailyStatRepository.Stats.Select(x =>
                new
                {
                    Stat = x,
                    CdrCalls = x.CdrCallsHourInfo,
                    CdrDrops = x.CdrDropsHourInfo,
                    ErasureDrops = x.ErasureDropsHourInfo,
                    KpiCalls = x.KpiCallsHourInfo,
                    KpiDrops = x.KpiDropsHourInfo
                });
            var stat = stats.FirstOrDefault(x =>
                x.Stat.StatTime == _statDate && x.Stat.CellId == _cellId && x.Stat.SectorId == _sectorId
                           && x.Stat.Frequency == _frequency);
            List<DropsHourDistribution> result = new List<DropsHourDistribution>();
            if (stat != null)
                result.Import(stat.CdrCalls, stat.CdrDrops,
                    stat.ErasureDrops, stat.KpiCalls, stat.KpiDrops);
            return result;
        }

        public AlarmHourDistribution GenerateAlarmHourDistribution()
        {
            var stats = _dailyStatRepository.Stats.Select(x =>
                new
                {
                    Stat = x,
                    Alarm = x.AlarmHourInfos
                });
            var stat = stats.FirstOrDefault(x => 
                x.Stat.StatTime == _statDate && x.Stat.CellId == _cellId && x.Stat.SectorId == _sectorId
                           && x.Stat.Frequency == _frequency);
            AlarmHourDistribution distribution = new AlarmHourDistribution();
            if (stat != null)
                distribution.Import(stat.Alarm);
            return distribution;
        }
    }
}
