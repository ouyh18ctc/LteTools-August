using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Service;
using NUnit.Framework;
using Moq;

namespace Lte.Parameters.Test.Kpi.Service
{
    [TestFixture]
    public class SaveCityKpiStatsServiceTest
    {
        private Mock<ITopCellRepository<FakeCityTimeStat>> repository =
            new Mock<ITopCellRepository<FakeCityTimeStat>>();

        [SetUp]
        public void SetUp()
        {
            repository.MockOperations();
        }

        [TestCase("Guangzhou",new[]{"2001-9-4"})]
        [TestCase("Guangzhou", new[] { "2001-9-4", "2002-8-16" })]
        [TestCase("Guangzhou", new[] { "2001-9-4", "2003-10-15", "2004-7-3" })]
        public void Test_Save(string city, string[] dateStrings)
        {
            SaveTimeCityKpiStatsService<FakeCityTimeStat, FakeCarrierTimeStat> service =
                new SaveTimeCityKpiStatsService<FakeCityTimeStat, FakeCarrierTimeStat>(repository.Object)
                {
                    CurrentCity = city
                };
            List<FakeCarrierTimeStat> infos = dateStrings.Select(x =>
                new FakeCarrierTimeStat
                {
                    StatTime = DateTime.Parse(x)
                }).ToList();
            int resultCount = service.Save(infos);
            Assert.AreEqual(resultCount, dateStrings.Length);
            for (int i = 0; i < resultCount; i++)
            {
                FakeCityTimeStat stat = repository.Object.Stats.ElementAt(i);
                Assert.AreEqual(stat.StatTime, DateTime.Parse(dateStrings[i]));
                Assert.AreEqual(stat.City, city);
            }
        }
    }

    [TestFixture]
    public class SaveSingleKpiStatsServiceTest
    {
        private Mock<ITopCellRepository<FakeTimeStat>> repository =
            new Mock<ITopCellRepository<FakeTimeStat>>();

        [SetUp]
        public void SetUp()
        {
            repository.MockOperations();
        }

        [TestCase("Guangzhou", new[] { "2001-9-4" })]
        [TestCase("Guangzhou", new[] { "2001-9-4", "2002-8-16" })]
        [TestCase("Guangzhou", new[] { "2001-9-4", "2003-10-15", "2004-7-3" })]
        public void Test_Save(string city, string[] dateStrings)
        {
            SaveTimeSingleKpiStatsService<FakeTimeStat, FakeCarrierTimeStat> service =
                new SaveTimeSingleKpiStatsService<FakeTimeStat, FakeCarrierTimeStat>(repository.Object);
            List<FakeCarrierTimeStat> infos = dateStrings.Select(x =>
                new FakeCarrierTimeStat
                {
                    StatTime = DateTime.Parse(x)
                }).ToList();
            int resultCount = service.Save(infos);
            Assert.AreEqual(resultCount, dateStrings.Length);
            for (int i = 0; i < resultCount; i++)
            {
                FakeTimeStat stat = repository.Object.Stats.ElementAt(i);
                Assert.AreEqual(stat.StatTime, DateTime.Parse(dateStrings[i]));
            }
        }
    }
}
