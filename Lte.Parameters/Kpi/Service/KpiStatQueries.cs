using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Regular;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Entities;
using Lte.Parameters.Service.Region;

namespace Lte.Parameters.Kpi.Service
{
    public static class KpiStatQueries
    {
        public static IEnumerable<CdmaRegionStat> GetLastDateStats(this IEnumerable<CdmaRegionStat> stats,
            IEnumerable<OptimizeRegion> regions, DateTime? statDate = null)
        {
            DateTime maxDate = statDate ?? DateTime.Today.AddDays(-1);
            IEnumerable<CdmaRegionStat> lastDateStats = stats.GetLastStatsConsideringIllegalDate(maxDate);
            if (!lastDateStats.Any()) return new List<CdmaRegionStat>();
            QueryNamesService service = new QueryRegionCityNamesService(regions);
            IEnumerable<CdmaRegionStat> cityStats = 
                from city in service.Query() 
                let currentCityStats = lastDateStats.GetCurrentCityStats(regions, city) 
                select currentCityStats.GetMergeStat(city);
            return cityStats.Concat(lastDateStats);
        }

        public static IEnumerable<TownPrecise4GView> GetLastDateStats(
            this IEnumerable<TownPreciseCoverage4GStat> stats, IEnumerable<Town> towns,
            DateTime? statDate = null)
        {
            DateTime maxDate = statDate ?? DateTime.Today.AddDays(-1);
            return stats.GetLastDateStatsConsideringIllegalDate(maxDate).Select(
                x => new TownPrecise4GView(x, towns));
        }

        public static IEnumerable<CdmaRegionStat> GetCurrentCityStats(this IEnumerable<CdmaRegionStat> lastDateStats, 
            IEnumerable<OptimizeRegion> regions, string city)
        {
            var currentCityRegions
                = regions.Where(x => x.City == city).Select(x => new { x.City, x.Region }).Distinct();
            IEnumerable<CdmaRegionStat> currentCityStats = from r in currentCityRegions
                                                           join s in lastDateStats
                                                           on r.Region equals s.Region
                                                           select s;
            return currentCityStats;
        }

        public static IEnumerable<RegionPrecise4GStat> GetCurrentDistrictStats(
            this IEnumerable<RegionPrecise4GStat> lastDateStats, IEnumerable<Town> towns, string district)
        {
            var currentDistrictRegions
                = towns.Where(x => x.DistrictName == district).Select(x => new {x.DistrictName, x.TownName}).Distinct();
            IEnumerable<RegionPrecise4GStat> currentDistrictStats
                = from r in currentDistrictRegions
                    join s in lastDateStats
                        on r.TownName equals s.Region
                    select s;
            return currentDistrictStats;
        }

        public static TRegionStat GetMergeStat<TRegionStat>(this IEnumerable<TRegionStat> currentCityStats, string city)
            where TRegionStat: class, IRegionStat, new()
        {
            TRegionStat mergedStat = currentCityStats.ArraySum();
            mergedStat.Region = city;
            return mergedStat;
        }

        public static int SaveStats(this ITopCellRepository<CdmaRegionStat> repository,
            IEnumerable<CdmaRegionStat> stats)
        {
            int result = 0;
            IEnumerable<CdmaRegionStat> existedStats = repository.Stats.ToList();
            foreach (CdmaRegionStat stat in stats.Where(stat => existedStats.QueryDateStat(stat.Region, stat.StatDate) == null))
            {
                repository.AddOneStat(stat);
                result++;
            }
            repository.SaveChanges();
            return result;
        }
    }
}
