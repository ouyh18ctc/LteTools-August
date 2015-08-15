using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Service;
using Lte.Parameters.Service.Region;

namespace Lte.Parameters.Kpi.Entities
{
    public abstract class AllDailyStatList<TStat>
        where TStat : class, IDateStat, IRegionStat, new()
    {
        public Dictionary<string, DailyStatList<TStat>> Stats { get; set; }

        public IEnumerable<string> OverallDates { get; private set; }

        public IEnumerable<string> DateCategories(string region)
        {
            return Stats.ContainsKey(region)
                ? Stats[region].SummaryStats.Select(x => x.Key).OrderBy(x => x).Select(x => x.ToShortDateString())
                : new List<string>();
        }

        protected QueryNamesService service;

        protected AllDailyStatList()
        {
            Stats = new Dictionary<string, DailyStatList<TStat>>();
        }

        protected abstract IEnumerable<TStat> GetCurrentCategoryStats(IEnumerable<TStat> stats, string category);

        protected abstract DailyStatList<TStat> GetDailyStatList(IEnumerable<TStat> currentCategoryStats, string category);

        private void Import(IEnumerable<TStat> stats)
        {
            OverallDates =
                stats.Select(x => x.StatDate).Distinct().OrderBy(x => x).Select(x => x.ToShortDateString());
            IEnumerable<string> categories = service.Query();
            foreach (string category in categories)
            {
                IEnumerable<TStat> currentCategoryStats = GetCurrentCategoryStats(stats, category);
                if (currentCategoryStats.Any())
                {
                    Stats.Add(category, GetDailyStatList(currentCategoryStats, category));
                }
            }
        }

        public void Import(IEnumerable<TStat> sourceStats, DateTime begin, DateTime end)
        {
            IEnumerable<TStat> stats = sourceStats.QueryDateStats(begin, end);
            Stats.Clear();
            Import(stats.ToList());
        }

        public void Import(ITopCellRepository<TStat> repository, DateTime begin, DateTime end)
        {
            Import(repository.Stats, begin, end);
        }
    }

    public class AllCdmaDailyStatList : AllDailyStatList<CdmaRegionStat>
    {
        private readonly IEnumerable<OptimizeRegion> _regions;

        public AllCdmaDailyStatList(IEnumerable<OptimizeRegion> regions)
        {
            _regions = regions;
            service = new QueryRegionCityNamesService(_regions);
        }

        protected override IEnumerable<CdmaRegionStat> GetCurrentCategoryStats(
            IEnumerable<CdmaRegionStat> stats, string category)
        {
            return stats.GetCurrentCityStats(_regions, category);
        }

        protected override DailyStatList<CdmaRegionStat> GetDailyStatList(
            IEnumerable<CdmaRegionStat> currentCategoryStats, string category)
        {
            return new CityCdmaDailyStatList(currentCategoryStats, category);
        }
    }

    public class AllLteDailyStatList : AllDailyStatList<RegionPrecise4GStat>
    {
        private readonly IEnumerable<Town> _towns;

        public AllLteDailyStatList(IEnumerable<Town> towns)
        {
            _towns = towns;
            service = new QueryDistinctDistrictNamesService(towns);
        }

        protected override IEnumerable<RegionPrecise4GStat> GetCurrentCategoryStats(
            IEnumerable<RegionPrecise4GStat> stats, string category)
        {
            return stats.GetCurrentDistrictStats(_towns, category);
        }

        protected override DailyStatList<RegionPrecise4GStat> GetDailyStatList(
            IEnumerable<RegionPrecise4GStat> currentCategoryStats, string category)
        {
            return new DistrictPrecise4GStatList(currentCategoryStats, category);
        }

        public IEnumerable<int> GetDistrictMrs(string district)
        {
            List<int> results = new List<int>();
            if (!Stats.ContainsKey(district)) return results;
            DailyStatList<RegionPrecise4GStat> currentDistrictStats = Stats[district];
            results.AddRange(OverallDates.Select(date => 
                currentDistrictStats.SummaryStats.ContainsKey(DateTime.Parse(date)) ? 
                currentDistrictStats.SummaryStats[DateTime.Parse(date)].TotalMrs : 
                0));
            return results;
        }

        public IEnumerable<double> GetDistrictRates(string district)
        {
            List<double> results = new List<double>();
            if (!Stats.ContainsKey(district)) return results;
            DailyStatList<RegionPrecise4GStat> currentDistrictStats = Stats[district];
            results.AddRange(OverallDates.Select(date =>
                currentDistrictStats.SummaryStats.ContainsKey(DateTime.Parse(date)) ?
                currentDistrictStats.SummaryStats[DateTime.Parse(date)].PreciseRate :
                0));
            return results;
        }
    }
}