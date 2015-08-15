using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Service;
using NUnit.Framework;
using Moq;

namespace Lte.Parameters.Test.Kpi.Service
{
    internal class FakeImportStatsService : ImportStatsService<FakeCsvInfo, FakeStat>
    {
        public FakeImportStatsService(ITopCellRepository<FakeStat> repository) : base(repository)
        {
        }

        protected override string Import(FakeStat stat, List<FakeCsvInfo> csvStats, ref int beginIndex,
            string oldCarrier)
        {
            beginIndex++;
            return oldCarrier;
        }
    }

    [TestFixture]
    public class ImportStatsServiceTest
    {
        private Mock<ITopCellRepository<FakeStat>> repository = new Mock<ITopCellRepository<FakeStat>>();

        [SetUp]
        public void SetUp()
        {
            repository.MockOperations();
        }

        [TestCase(new[] {"1-1", "2", "3", "4"}, 1000, 2015, 1, 1, 4)]
        [TestCase(new[] {"1-1", "2", "3", "4", "55"}, 1000, 2014, 12, 1, 5)]
        [TestCase(new[] {"1-1", "2", "3", "4", "97", "98"}, 1000, 2013, 5, 18, 6)]
        [TestCase(new[] {"1-1", "2", "3", "4", "97", "98", "7"}, 6, 2013, 5, 18, 6)]
        public void Test_ImportStats(string[] carrierInfos, int maxIndex, int year, int month, int day, int count)
        {
            FakeImportStatsService service = new FakeImportStatsService(repository.Object);

            List<FakeCsvInfo> infos = new List<FakeCsvInfo>();
            infos.AddRange(carrierInfos.Select(x => new FakeCsvInfo { Carrier = x }));
            int resultCount = service.ImportStats(infos, maxIndex, new DateTime(year, month, day));
            Assert.AreEqual(resultCount, count);
            for (int i = 0; i < resultCount; i++)
            {
                FakeStat stat = repository.Object.Stats.ElementAt(i);
                Assert.AreEqual(stat.StatTime, new DateTime(year, month, day));
            }
        }
    }
}
