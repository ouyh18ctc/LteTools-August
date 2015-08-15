using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Lte.Parameters.Entities;
using Lte.Parameters.Kpi.Abstract;
using Lte.Parameters.Kpi.Service;
using Moq;
using NUnit.Framework;

namespace Lte.Parameters.Test.Kpi.Service
{
    [TestFixture]
    public class SaveTownStatsServiceTest
    {
        private readonly Mock<ITopCellRepository<FakeTownTimeStat>> repository=
            new Mock<ITopCellRepository<FakeTownTimeStat>>();

        private IEnumerable<ENodeb> eNodebs;
            
        [SetUp]
        public void SetUp()
        {
            repository.MockOperations();
        }

        [TestCase(new[] { 1 }, new[] { 1 }, new byte[] { 1 }, new[] { 1 }, 
            new[] { 1 }, new[] { 1 }, 1)]
        [TestCase(new[] { 1, 3 }, new[] { 1, 2 }, new byte[] { 1, 2 }, new[] { 1, 2 },
            new[] { 1, 2 }, new[] { 1, 2 }, 2)]
        [TestCase(new[] { 1, 2 }, new[] { 1, 1 }, new byte[] { 1, 3 }, new[] { 1, 2 },
            new[] { 1 }, new[] { 3 }, 1)]
        [TestCase(new[] { 1, 2, 3 }, new[] { 1, 1, 4 }, new byte[] { 1, 3, 2 }, new[] { 1, 2, 4 },
            new[] { 1, 4 }, new[] { 3, 4 }, 2)]
        [TestCase(new[] { 1, 2, 3, 8, 9 }, new[] { 1, 1, 4, 4, 4 }, new byte[] { 1, 3, 2, 1, 3 }, 
            new[] { 1, 2, 4, 8, 6 },
            new[] { 1, 4 }, new[] { 3, 18 }, 2)]
        public void Test_Save(int[] eNodebIds, int[] townIds, byte[] sectorIds, int[] kpis,
            int[] resultTownIds, int[] resultKpis, int count)
        {
            eNodebs = eNodebIds.Select((e, i) => new ENodeb
            {
                ENodebId = e, TownId = townIds[i]
            });

            SaveTimeTownStatsService<FakeTownTimeStat, FakeCell> service =
                new SaveTimeTownStatsService<FakeTownTimeStat, FakeCell>(repository.Object, eNodebs);
            List<FakeCell> infos = eNodebIds.Select((c, i) => new FakeCell
            {
                CellId = c, SectorId = sectorIds[i], Kpi = kpis[i],
                StatTime = DateTime.Today
            }).ToList();
            int resultCount = service.Save(infos);
            Assert.AreEqual(resultCount, count);
            for (int i = 0; i < count; i++)
            {
                FakeTownTimeStat stat = repository.Object.Stats.ElementAt(i);
                Assert.AreEqual(stat.TownId,resultTownIds[i]);
                Assert.AreEqual(stat.Kpi,resultKpis[i]);
            }
        }

        [TestCase(new[] { 1, 2 }, new[] { 1, 1 }, new byte[] { 1, 3 }, new[] { 1, 2 },
            new[]{"2002-1-1", "2003-2-2"},
            new[] { 1, 1 }, new[] { 1, 2 }, new[]{"2002-1-1", "2003-2-2"},
            new[] { "2002-6-1" }, 2)]
        [TestCase(new[] { 1, 2 }, new[] { 1, 1 }, new byte[] { 1, 3 }, new[] { 1, 2 },
            new[] { "2002-1-1", "2003-2-2" },
            new[] { 1 }, new[] { 2 }, new[] { "2003-2-2" },
            new[] { "2002-1-1" }, 1)]
        [TestCase(new[] { 1, 2, 3 }, new[] { 1, 1, 2 }, new byte[] { 1, 3, 4 }, new[] { 1, 2, 4 },
            new[] { "2002-1-1", "2003-2-2", "2003-2-2" },
            new[] { 1, 2 }, new[] { 2, 4 }, new[] { "2003-2-2", "2003-2-2" },
            new[] { "2002-1-1" }, 2)]
        [TestCase(new[] { 1, 2, 3 }, new[] { 1, 1, 1 }, new byte[] { 1, 3, 6 }, new[] { 1, 2, 4 },
            new[] { "2002-1-1", "2003-2-2", "2003-2-2" },
            new[] { 1 }, new[] { 6 }, new[] { "2003-2-2" },
            new[] { "2002-1-1" }, 1)]
        [TestCase(new[] { 1, 2, 3, 4 }, new[] { 1, 1, 1, 2 }, new byte[] { 1, 3, 6, 4 }, new[] { 1, 2, 4, 5 },
            new[] { "2002-1-1", "2003-2-2", "2003-2-2", "2003-7-15" },
            new[] { 1, 2 }, new[] { 6, 5 }, new[] { "2003-2-2", "2003-7-15" },
            new[] { "2002-1-1" }, 2)]
        [TestCase(new[] { 1, 2, 3, 4 }, new[] { 1, 1, 1, 2 }, new byte[] { 1, 3, 6, 4 }, new[] { 1, 2, 4, 5 },
            new[] { "2002-1-1", "2003-2-2", "2003-2-2", "2003-7-15" },
            new[] { 1 }, new[] { 6 }, new[] { "2003-2-2" },
            new[] { "2002-1-1", "2003-7-15" }, 1)]
        public void Test_Save_ExistedDateConsidered(
            int[] eNodebIds, int[] townIds, byte[] sectorIds, int[] kpis, string[] dateStrings,
            int[] resultTownIds, int[] resultKpis, string[] resultDates,
            string[] existedDates, int count)
        {
            eNodebs = eNodebIds.Select((e, i) => new ENodeb
            {
                ENodebId = e,
                TownId = townIds[i]
            });

            repository.SetupGet(x => x.Stats).Returns(existedDates.Select(x =>
                new FakeTownTimeStat
                {
                    StatTime = DateTime.Parse(x),
                    TownId = 101,
                    Kpi = 1002
                }).AsQueryable());
            SaveTimeTownStatsService<FakeTownTimeStat, FakeCell> service =
                new SaveTimeTownStatsService<FakeTownTimeStat, FakeCell>(repository.Object, eNodebs);
            List<FakeCell> infos = eNodebIds.Select((c, i) => new FakeCell
            {
                CellId = c,
                SectorId = sectorIds[i],
                Kpi = kpis[i],
                StatTime = DateTime.Parse(dateStrings[i])
            }).ToList();
            int resultCount = service.Save(infos);
            Assert.AreEqual(resultCount, count);
            for (int i = 0; i < count; i++)
            {
                FakeTownTimeStat stat = repository.Object.Stats.ElementAt(i + existedDates.Length);
                Assert.AreEqual(stat.TownId, resultTownIds[i]);
                Assert.AreEqual(stat.Kpi, resultKpis[i]);
                Assert.AreEqual(stat.StatTime, DateTime.Parse(resultDates[i]));
            }
        }
    }
}
