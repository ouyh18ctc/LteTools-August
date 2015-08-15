using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Service;
using Moq;
using NUnit.Framework;

namespace Lte.Parameters.Test.Kpi.Service
{
    internal class FakeSaveTimeStatsService : SaveTimeStatsService<FakeCarrierTimeStat, FakeCsvInfo>
    {
        public FakeSaveTimeStatsService(ITopCellRepository<FakeCarrierTimeStat> repository) : base(repository)
        {
        }

        protected override void ImportAdditionalInfos(FakeCarrierTimeStat stat)
        {
            
        }

        protected override IEnumerable<FakeCarrierTimeStat> ImportStats(IEnumerable<FakeCsvInfo> excelStats)
        {
            return excelStats.Select(x => new FakeCarrierTimeStat
            {
                StatTime = DateTime.Today,
                Carrier = x.Carrier
            });
        }
    }

    internal class FakeSaveTimeDateStatsService : SaveTimeStatsService<FakeCarrierTimeStat, FakeCarrierTimeStat>
    {
        public FakeSaveTimeDateStatsService(ITopCellRepository<FakeCarrierTimeStat> repository) : base(repository)
        {
        }

        protected override void ImportAdditionalInfos(FakeCarrierTimeStat stat)
        {
            
        }

        protected override IEnumerable<FakeCarrierTimeStat> ImportStats(IEnumerable<FakeCarrierTimeStat> excelStats)
        {
            return excelStats.Select(x => new FakeCarrierTimeStat
            {
                StatTime = x.StatTime,
                Carrier = x.Carrier
            });
        }
    }

    [TestFixture]
    public class SaveStatsServiceTest
    {
        private readonly Mock<ITopCellRepository<FakeCarrierTimeStat>> repository 
            = new Mock<ITopCellRepository<FakeCarrierTimeStat>>();

        [SetUp]
        public void SetUp()
        {
            repository.MockOperations();
        }

        [TestCase(new[] { "1-1", "2", "3", "4" }, 4)]
        [TestCase(new[] { "1-1", "2", "3", "4", "55" }, 5)]
        [TestCase(new[] { "1-1", "2", "3", "4", "97", "98" }, 6)]
        [TestCase(new[] { "1-1", "2", "3", "4", "97", "98", "7" }, 7)]
        public void Test_SaveSimpleCase(string[] carrierInfos, int count)
        {
            SaveTimeStatsService<FakeCarrierTimeStat, FakeCsvInfo> service
                = new FakeSaveTimeStatsService(repository.Object);

            List<FakeCsvInfo> infos = new List<FakeCsvInfo>();
            infos.AddRange(carrierInfos.Select(x => new FakeCsvInfo { Carrier = x }));
            int resultCount = service.Save(infos);
            Assert.AreEqual(resultCount, count);
            for (int i = 0; i < resultCount; i++)
            {
                FakeCarrierTimeStat stat = repository.Object.Stats.ElementAt(i);
                Assert.AreEqual(stat.StatTime, DateTime.Today);
                Assert.AreEqual(stat.Carrier, carrierInfos[i]);
            }
        }

        [TestCase(new[] { "1-1", "2", "3", "4" },
            new[] { "2012-1-1", "2013-1-1", "2012-4-1", "2015-1-1" }, 4)]
        [TestCase(new[] { "1-1", "2", "3", "4", "55" },
            new[] { "2012-1-3", "2013-5-1", "2012-4-22", "2015-1-1", "2015-2-1" }, 5)]
        [TestCase(new[] { "1-1", "2", "3", "4", "97", "98" },
            new[] { "2012-2-3", "2013-5-3", "2012-10-22", "2015-2-1", "2014-2-11", "2014-6-11" }, 6)]
        [TestCase(new[] { "1-1", "2", "3", "4", "97", "98", "7" },
            new[] { "2012-2-3", "2013-5-3", "2012-10-22", "2015-2-1", "2014-2-11", "2014-6-11", "2012-6-11" }, 7)]
        public void Test_SaveDateCase(string[] carrierInfos, string[] dateStrings, int count)
        {
            SaveTimeStatsService<FakeCarrierTimeStat, FakeCarrierTimeStat> service
                = new FakeSaveTimeDateStatsService(repository.Object);

            List<FakeCarrierTimeStat> infos = carrierInfos.Select((t, i) => new FakeCarrierTimeStat
            {
                Carrier = t, StatTime = DateTime.Parse(dateStrings[i])
            }).ToList();
            int resultCount = service.Save(infos);
            Assert.AreEqual(resultCount, count);
            for (int i = 0; i < resultCount; i++)
            {
                FakeCarrierTimeStat stat = repository.Object.Stats.ElementAt(i);
                Assert.AreEqual(stat.StatTime, DateTime.Parse(dateStrings[i]));
                Assert.AreEqual(stat.Carrier, carrierInfos[i]);
            }
        }

        [TestCase(new[] { "1-1", "2", "3", "4" },
            new[] { "2012-1-1", "2013-1-1", "2012-4-1", "2015-1-1" }, 
            new[] { "2012-6-1" }, 4)]
        [TestCase(new[] { "1-1", "2", "3", "4" },
            new[] { "2012-1-1", "2013-1-1", "2012-4-1", "2015-1-1" },
            new[] { "2012-1-1" }, 3)]
        [TestCase(new[] { "1-1", "2", "3", "4" },
            new[] { "2012-1-1", "2013-1-1", "2012-4-1", "2015-1-1" },
            new[] { "2013-1-1" }, 3)]
        [TestCase(new[] { "1-1", "2", "3", "4" },
            new[] { "2012-1-1", "2013-1-1", "2012-4-1", "2015-1-1" },
            new[] { "2013-1-1", "2011-2-23" }, 3)]
        [TestCase(new[] { "1-1", "2", "3", "4" },
            new[] { "2012-1-1", "2013-1-1", "2012-4-1", "2015-1-1" },
            new[] { "2013-1-1", "2012-4-1" }, 2)]
        [TestCase(new[] { "1-1", "2", "3", "4", "55" },
            new[] { "2012-1-3", "2013-5-1", "2012-4-22", "2015-1-1", "2015-2-1" },
            new[] { "2013-1-1" }, 5)]
        [TestCase(new[] { "1-1", "2", "3", "4", "55" },
            new[] { "2012-1-3", "2013-5-1", "2012-4-22", "2015-1-1", "2015-2-1" },
            new[] { "2012-1-3" }, 4)]
        [TestCase(new[] { "1-1", "2", "3", "4", "97", "98" },
            new[] { "2012-2-3", "2013-5-3", "2012-10-22", "2015-2-1", "2014-2-11", "2014-6-11" },
            new[] { "2012-1-3" }, 6)]
        [TestCase(new[] { "1-1", "2", "3", "4", "97", "98" },
            new[] { "2012-2-3", "2013-5-3", "2012-10-22", "2015-2-1", "2014-2-11", "2014-6-11" }, 
            new[] { "2012-2-3", "2013-5-3" }, 4)]
        [TestCase(new[] { "1-1", "2", "3", "4", "97", "98", "7" },
            new[] { "2012-2-3", "2013-5-3", "2012-10-22", "2015-2-1", "2014-2-11", "2014-6-11", "2012-6-11" },
            new[] { "2012-1-3" }, 7)]
        [TestCase(new[] { "1-1", "2", "3", "4", "97", "98", "7" },
            new[] { "2012-2-3", "2013-5-3", "2012-10-22", "2015-2-1", "2014-2-11", "2014-6-11", "2012-6-11" },
            new[] { "2013-5-3" }, 6)]
        public void Test_SaveDateConsideredCase(string[] carrierInfos, string[] dateStrings, 
            string[] existedDates, int count)
        {
            repository.SetupGet(x => x.Stats).Returns(existedDates.Select(x =>
                new FakeCarrierTimeStat {Carrier = "c", StatTime = DateTime.Parse(x)}).AsQueryable());
            SaveTimeStatsService<FakeCarrierTimeStat, FakeCarrierTimeStat> service
                = new FakeSaveTimeDateStatsService(repository.Object);

            List<FakeCarrierTimeStat> infos = carrierInfos.Select((t, i) => new FakeCarrierTimeStat
            {
                Carrier = t,
                StatTime = DateTime.Parse(dateStrings[i])
            }).ToList();
            int resultCount = service.Save(infos);
            Assert.AreEqual(resultCount, count, existedDates[0]);
        }
    }
}
