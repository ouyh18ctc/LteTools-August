using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;

namespace Lte.Domain.TypeDefs
{
    public interface ITimeStat
    {
        DateTime StatTime { get; set; }
    }

    public interface IDateStat
    {
        DateTime StatDate { get; set; }
    }

    public interface IMonthStat
    {
        short Year { get; set; }

        byte Month { get; set; }
    }

    public interface IImportStat<in TExcelStat>
    {
        void Import(TExcelStat cellExcel);
    }

    public interface ICityStat
    {
        string City { get; set; }
    }

    public interface IRegionStat
    {
        string Region { get; set; }
    }

    public interface IGetTopCellView : ICdmaLteNames
    {
        short Frequency { get; }

        int Drops { get; }
    }

    public static class DateTimeStatQueries
    {
        private static IEnumerable<TStat> GetLastDateStats<TStat>(this IEnumerable<TStat> stats,
            DateTime? statDate = null)
            where TStat : ITimeStat
        {
            DateTime maxDate = statDate ?? DateTime.Today.AddDays(-1);
            stats = stats.Where(x => x.StatTime < maxDate.AddDays(1) && x.StatTime >= maxDate.AddDays(-100)).ToList();
            if (stats.Any())
            {
                DateTime lastDate = stats.Select(x => x.StatTime).Max().Date;
                return stats.Where(x => x.StatTime >= lastDate);
            }
            return new List<TStat>();
        }

        private static IEnumerable<TStat> GetLastStats<TStat>(this IEnumerable<TStat> stats,
            DateTime? statDate = null)
            where TStat : IDateStat
        {
            DateTime maxDate = statDate ?? DateTime.Today.AddDays(-1);
            stats = stats.Where(x => x.StatDate < maxDate.AddDays(1) && x.StatDate >= maxDate.AddDays(-100));
            if (stats.Any())
            {
                DateTime lastDate = stats.Select(x => x.StatDate).Max();
                return stats.Where(x => x.StatDate == lastDate).ToList();
            }
            return new List<TStat>();
        }

        public static IEnumerable<TStat> GetLastDateStatsConsideringIllegalDate<TStat>(this IEnumerable<TStat> stats,
            DateTime statDate)
            where TStat : ITimeStat
        {
            return statDate < new DateTime(2012, 1, 1) ?
                stats.GetLastDateStats().ToList() :
                stats.GetLastDateStats(statDate).ToList();
        }

        public static IEnumerable<TStat> GetLastStatsConsideringIllegalDate<TStat>(this IEnumerable<TStat> stats,
            DateTime statDate)
            where TStat : IDateStat
        {
            return statDate < new DateTime(2012, 1, 1) ?
                stats.GetLastStats().ToList() :
                stats.GetLastStats(statDate).ToList();
        }

        public static TStat QueryDateStat<TStat>(this IEnumerable<TStat> stats,
            string region, DateTime date)
            where TStat : IRegionStat, IDateStat
        {
            return stats.FirstOrDefault(x => x.Region == region && x.StatDate == date);
        }

        public static IEnumerable<TStat> QueryDateStats<TStat>(this IEnumerable<TStat> stats,
            DateTime begin, DateTime end)
            where TStat: IDateStat
        {
            return stats.Where(x => x.StatDate >= begin && x.StatDate <= end);
        }

        public static IEnumerable<TStat> QueryTimeStats<TStat>(this IEnumerable<TStat> stats,
            DateTime begin, DateTime endDate, string city)
            where TStat : ITimeStat, ICityStat
        {
            return stats.Where(x => x.StatTime >= begin && x.StatTime < endDate && x.City == city);
        }

        public static IEnumerable<TStat> QueryTimeStats<TStat>(this IEnumerable<TStat> stats,
            DateTime begin, DateTime endDate)
            where TStat : ITimeStat
        {
            return stats.Where(x => x.StatTime >= begin && x.StatTime < endDate);
        }
    }
}
