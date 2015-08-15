using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.TypeDefs;
using Lte.Parameters.Kpi.Entities;
using Lte.Parameters.Service.Region;
using NUnit.Framework;

namespace Lte.Parameters.Test.Kpi.Entities
{
    internal class FakeStat : IDateStat, IRegionStat
    {
        public DateTime StatDate { get; set; }

        public string Region { get; set; }

        public int KpiValue { get; set; }
    }

    internal class StubDailyStatList : AllDailyStatList<FakeStat>
    {
        protected override IEnumerable<FakeStat> GetCurrentCategoryStats(
            IEnumerable<FakeStat> stats, string category)
        {
            return stats.Where(x => x.Region.IndexOf(category, StringComparison.Ordinal) >= 0);
        }

        protected override DailyStatList<FakeStat> GetDailyStatList(
            IEnumerable<FakeStat> currentCategoryStats, string category)
        {
            return new FakeRegionDailyStatList(currentCategoryStats, category);
        }

        public StubDailyStatList()
        {
            service = new FakeNamesService();
        }
    }

    internal class FakeRegionDailyStatList : DailyStatList<FakeStat>
    {
        public FakeRegionDailyStatList(IEnumerable<FakeStat> stats, string district)
            : base(stats, district)
        {
        }
    }

    internal class FakeNamesService : QueryNamesService
    {
        public override IEnumerable<string> Query()
        {
            return new[] {"aa", "bb", "cc", "dd"};
        }
    }

    [TestFixture]
    public class AllDailyStatListTest
    {
        private StubDailyStatList statList = new StubDailyStatList();

        [Test]
        public void TestImportStats()
        {
            IEnumerable<FakeStat> stats = new List<FakeStat>
            {
                new FakeStat {StatDate = DateTime.Parse("2011-1-1"), Region = "aa1", KpiValue = 1}
            };
            statList.Import(stats, DateTime.Parse("2011-1-1"), DateTime.Parse("2011-1-2"));
            IEnumerable<string> dates = statList.DateCategories("aa");
            Assert.IsNotNull(dates);
            Assert.AreEqual(dates.Count(), 1);
        }
    }
}
