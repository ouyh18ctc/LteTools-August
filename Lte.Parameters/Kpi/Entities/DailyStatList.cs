using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Kpi.Service;

namespace Lte.Parameters.Kpi.Entities
{
    public abstract class DailyStatList<TStat>
        where TStat: class, IDateStat, IRegionStat, new() 
    {
        private Dictionary<DateTime, IEnumerable<TStat>> RegionStats { get; set; }

        public Dictionary<DateTime, TStat> SummaryStats { get; private set; }

        public List<string> Regions { get; private set; }

        public IEnumerable<T> GetSummaryStats<T>(Func<TStat, T> selector)
        {
            return SummaryStats.Select(x => x.Value).OrderBy(x=>x.StatDate).Select(selector);
        }

        public IEnumerable<T> GetRegionStats<T>(Func<TStat, T> selector, 
            Func<TStat, string> regionSelector, string region)
        {
            return RegionStats.Select(x => x.Value).Select(stats => 
                stats.FirstOrDefault(x => regionSelector(x) == region)).Select(
                stat => stat != null ? selector(stat) : default(T)).ToList();
        }

        protected DailyStatList()
        {
            RegionStats = new Dictionary<DateTime, IEnumerable<TStat>>();
            SummaryStats = new Dictionary<DateTime, TStat>();
            Regions = new List<string>();
        }
        
        protected DailyStatList(IEnumerable<TStat> stats, string regionName)
            : this()
        {
            IEnumerable<DateTime> dates = stats.Select(x => x.StatDate).Distinct();
            foreach (DateTime date in dates)
            {
                IEnumerable<TStat> oneDayStats = stats.Where(x => x.StatDate == date);
                TStat currentCityStat = oneDayStats.GetMergeStat(regionName);
                SummaryStats.Add(date, currentCityStat);
                RegionStats.Add(date, oneDayStats);
                IEnumerable<string> existedRegions = oneDayStats.Select(x => x.Region).Distinct();
                foreach (string region in existedRegions)
                {
                    if (Regions.FirstOrDefault(x => x == region) == null)
                    {
                        Regions.Add(region);
                    }
                }
            }
        }
    }

    public class CityCdmaDailyStatList : DailyStatList<CdmaRegionStat>
    {
        public CityCdmaDailyStatList(IEnumerable<CdmaRegionStat> stats, string city)
            : base(stats, city)
        {
        }
    }

    public class DistrictPrecise4GStatList : DailyStatList<RegionPrecise4GStat>
    {
        public DistrictPrecise4GStatList(IEnumerable<RegionPrecise4GStat> stats, string district)
            : base(stats, district)
        {
        }
    }

}
